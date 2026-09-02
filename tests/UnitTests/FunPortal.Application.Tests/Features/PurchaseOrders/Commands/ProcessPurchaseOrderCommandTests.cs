using AutoFixture;
using FluentAssertions;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Features.PurchaseOrders.Commands;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing;
using FunPortal.Application.Interfaces;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace FunPortal.Application.Tests.Features.PurchaseOrders.Commands
{
    [TestClass]
    public sealed class ProcessPurchaseOrderCommandTests
    {
        private ProcessPurchaseOrderCommandFixture _fixture = default!;

        [TestInitialize]
        public void TestInitialize()
            => _fixture = new ProcessPurchaseOrderCommandFixture();

        [TestMethod]
        public async Task Handle_UserNotFound_ShouldNotProcessPurchaseOrder()
        {
            // Arrange
            var command = _fixture.CreateCommand();

            Mock.Get(_fixture.UserRepository)
                .Setup(x => x.GetByIdAsync(_fixture.IdentityContext.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var action = async () => await _fixture.Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("User is not authorized to place orders");

            Mock.Get(_fixture.PurchaseOrderRepository)
                .Verify(x => x.Add(It.IsAny<PurchaseOrder>()), Times.Never);

            Mock.Get(_fixture.PurchaseOrderProcessor)
                .Verify(x => x.ProcessAsync(It.IsAny<OrderProcessingContext>(), It.IsAny<CancellationToken>()), Times.Never);

            Mock.Get(_fixture.UnitOfWork)
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_ProductNotFound_ShouldNotProcessPurchaseOrder()
        {
            // Arrange
            var command = _fixture.CreateCommand();

            Mock.Get(_fixture.ProductRepository)
                .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act
            var action = async () => await _fixture.Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Product with ID {command.Request.Items.First().ProductId} not found");

            Mock.Get(_fixture.PurchaseOrderRepository)
                .Verify(x => x.Add(It.IsAny<PurchaseOrder>()), Times.Never);

            Mock.Get(_fixture.PurchaseOrderProcessor)
                .Verify(x => x.ProcessAsync(It.IsAny<OrderProcessingContext>(), It.IsAny<CancellationToken>()), Times.Never);

            Mock.Get(_fixture.UnitOfWork)
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_RequestIsValid_ShouldProcessPurchaseOrder()
        {
            // Arrange
            var command = _fixture.CreateCommand();

            var price = 10.0m;

            foreach (var item in command.Request.Items)
            {
                var product = new Book
                {
                    ProductId = item.ProductId,
                    Name = $"Product {item.ProductId}",
                    Price = price,
                    Author = $"Author {item.ProductId}",
                    ISBN = $"ISBN-{item.ProductId}",
                    ProductType = ProductType.PhysicalBook,
                    CreatedOn = DateTime.UtcNow,
                };

                Mock.Get(_fixture.ProductRepository)
                    .Setup(x => x.GetByIdAsync(item.ProductId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(product);
            }

            var expectedSavedOrder = new PurchaseOrder
            {
                PurchaseOrderId = 1,
                UserId = _fixture.IdentityContext.UserId,
                TotalPrice = command.Request.Items.Sum(i => price * i.Quantity),
                ItemLines = [.. command.Request.Items
                    .Select(i => new OrderItemLine
                    {
                        ProductId = i.ProductId,
                        ProductName = $"Product {i.ProductId}",
                        Quantity = i.Quantity,
                        Price = price,
                    })],
            };

            Mock.Get(_fixture.PurchaseOrderRepository)
                .Setup(x => x.Add(It.IsAny<PurchaseOrder>()))
                .Returns(expectedSavedOrder);

            // Act
            var result = await _fixture.Handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(expectedSavedOrder.UserId);
            result.TotalPrice.Should().Be(expectedSavedOrder.TotalPrice);
            result.OrderedOn.Should().Be(expectedSavedOrder.OrderedOn);
            result.Status.Should().Be(expectedSavedOrder.Status);

            result.Items.Should().HaveCount(expectedSavedOrder.ItemLines.Count);

            Mock.Get(_fixture.PurchaseOrderRepository)
                .Verify(x => x.Add(It.IsAny<PurchaseOrder>()), Times.Once);

            Mock.Get(_fixture.PurchaseOrderProcessor)
                .Verify(
                    x => x.ProcessAsync(It.IsAny<OrderProcessingContext>(), It.IsAny<CancellationToken>()),
                    Times.Once);

            Mock.Get(_fixture.UnitOfWork)
                .Verify(
                    x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                    Times.AtLeastOnce);

            Mock.Get(_fixture.UnitOfWork)
                .Verify(
                    x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [TestMethod]
        public async Task Handle_RequestIsValidButProcessingFails_ShouldRollbackTransaction()
        {
            // Arrange
            var command = _fixture.CreateCommand();

            var price = 10.0m;

            foreach (var item in command.Request.Items)
            {
                var product = new Book
                {
                    ProductId = item.ProductId,
                    Name = $"Product {item.ProductId}",
                    Price = price,
                    Author = $"Author {item.ProductId}",
                    ISBN = $"ISBN-{item.ProductId}",
                    ProductType = ProductType.PhysicalBook,
                    CreatedOn = DateTime.UtcNow,
                };

                Mock.Get(_fixture.ProductRepository)
                    .Setup(x => x.GetByIdAsync(item.ProductId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(product);
            }

            Mock.Get(_fixture.PurchaseOrderRepository)
                .Setup(x => x.Add(It.IsAny<PurchaseOrder>()))
                .Returns(It.IsAny<PurchaseOrder>());

            Mock.Get(_fixture.PurchaseOrderProcessor)
                .Setup(x => x.ProcessAsync(It.IsAny<OrderProcessingContext>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Processing failed"));

            // Act
            var action = async () => await _fixture.Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<Exception>().WithMessage("Processing failed");

            Mock.Get(_fixture.UnitOfWork)
                .Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

            Mock.Get(_fixture.Logger)
                .Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while processing the purchase order. Transaction rolled back.")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
        }

        private sealed class ProcessPurchaseOrderCommandFixture
        {
            public const int UserId = 1;

            private Fixture Fixture { get; }

            public IIdentityContext IdentityContext { get; }

            public IPurchaseOrderRepository PurchaseOrderRepository { get; }

            public IUserRepository UserRepository { get; }

            public IProductRepository ProductRepository { get; }

            public IPurchaseOrderProcessor PurchaseOrderProcessor { get; }

            public IUnitOfWork UnitOfWork { get; }

            public ILogger<ProcessPurchaseOrderCommandHandler> Logger { get; }

            public ProcessPurchaseOrderCommandHandler Handler { get; }

            public ProcessPurchaseOrderCommandFixture()
            {
                Fixture = new Fixture();
                IdentityContext = Mock.Of<IIdentityContext>();
                PurchaseOrderRepository = Mock.Of<IPurchaseOrderRepository>();
                UserRepository = Mock.Of<IUserRepository>();
                ProductRepository = Mock.Of<IProductRepository>();
                PurchaseOrderProcessor = Mock.Of<IPurchaseOrderProcessor>();
                UnitOfWork = Mock.Of<IUnitOfWork>();
                Logger = Mock.Of<ILogger<ProcessPurchaseOrderCommandHandler>>();

                Handler = new ProcessPurchaseOrderCommandHandler(
                    IdentityContext,
                    PurchaseOrderRepository,
                    UserRepository,
                    ProductRepository,
                    PurchaseOrderProcessor,
                    UnitOfWork,
                    Logger);

                SetUpMocks();
            }

            public ProcessPurchaseOrderCommand CreateCommand()
            {
                var request = Fixture.Create<CreatePurchaseOrderRequest>();
                return new ProcessPurchaseOrderCommand(request);
            }

            private void SetUpMocks()
            {
                Mock.Get(IdentityContext)
                    .SetupGet(x => x.UserId)
                    .Returns(UserId);

                Mock.Get(UserRepository)
                    .Setup(x => x.GetByIdAsync(UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new User
                    {
                        UserId = UserId,
                        Email = "user@example.com",
                        IsActive = true,
                        Role = UserRole.Admin,
                    });
            }
        }
    }
}
