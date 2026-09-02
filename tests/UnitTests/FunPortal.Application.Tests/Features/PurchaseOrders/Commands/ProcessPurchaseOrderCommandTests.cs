using AutoFixture;
using FluentAssertions;
using FunPortal.Application.DTOs.PurchaseOrders;
using FunPortal.Application.Features.PurchaseOrders.Commands;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
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
        public async Task Handle_CustomerNotFound_ShouldNotProcessPurchaseOrder()
        {
            // Arrange
            var command = _fixture.CreateCommand();

            Mock.Get(_fixture.UserRepository)
                .Setup(x => x.ExistsAsync(command.Request.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var action = async () => await _fixture.Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with ID {command.Request.UserId} not found");

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

            Mock.Get(_fixture.UserRepository)
                .Setup(x => x.ExistsAsync(command.Request.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

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
            Mock.Get(_fixture.UserRepository)
                .Setup(x => x.ExistsAsync(command.Request.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

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
                UserId = command.Request.UserId,
                TotalPrice = command.Request.Items.Sum(i => price * i.Quantity),
                ItemLines = command.Request.Items
                    .Select(i => new OrderItemLine
                    {
                        ProductId = i.ProductId,
                        ProductName = $"Product {i.ProductId}",
                        Quantity = i.Quantity,
                        Price = price,
                    }).ToArray(),
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
            Mock.Get(_fixture.UserRepository)
                .Setup(x => x.ExistsAsync(command.Request.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

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
        }

        private sealed class ProcessPurchaseOrderCommandFixture
        {
            private Fixture Fixture { get; }

            public IPurchaseOrderRepository PurchaseOrderRepository { get; }

            public IUserRepository UserRepository { get; }

            public IProductRepository ProductRepository { get; }

            public IPurchaseOrderProcessor PurchaseOrderProcessor { get; }

            public IUnitOfWork UnitOfWork { get; }

            public ProcessPurchaseOrderCommandHandler Handler { get; }

            public ProcessPurchaseOrderCommandFixture()
            {
                Fixture = new Fixture();
                PurchaseOrderRepository = Mock.Of<IPurchaseOrderRepository>();
                UserRepository = Mock.Of<IUserRepository>();
                ProductRepository = Mock.Of<IProductRepository>();
                PurchaseOrderProcessor = Mock.Of<IPurchaseOrderProcessor>();
                UnitOfWork = Mock.Of<IUnitOfWork>();

                Handler = new ProcessPurchaseOrderCommandHandler(
                    PurchaseOrderRepository,
                    UserRepository,
                    ProductRepository,
                    PurchaseOrderProcessor,
                    UnitOfWork);
            }

            public ProcessPurchaseOrderCommand CreateCommand()
            {
                var request = Fixture.Create<CreatePurchaseOrderRequest>();
                return new ProcessPurchaseOrderCommand(request);
            }
        }
    }
}
