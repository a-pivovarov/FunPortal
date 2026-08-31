using FluentAssertions;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing.Rules;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;
using Moq;

namespace FunPortal.Application.Tests.Features.PurchaseOrders.Commands.Processing.Rules
{
    [TestClass]
    public sealed class GenerateShippingSlipRuleTests
    {
        private GenerateShippingSlipRuleFixture _fixture = default!;

        [TestInitialize]
        public void TestInitialize()
            => _fixture = new GenerateShippingSlipRuleFixture();

        [TestMethod]
        public void Order_ShouldReturnCorrectValue()
        {
            // Assert
            _fixture.Rule.Order.Should().Be(2);
        }

        [TestMethod]
        public void CanExecute_ShouldReturnTrue_WhenOrderContainsPhysicalBookProduct()
        {
            // Arrange
            var context = new OrderProcessingContext
            {
                Order = new PurchaseOrder
                {
                    ItemLines =
                    [
                        new() { ProductId = 1 }
                    ]
                },
                Products = new Dictionary<int, Product>
                {
                    { 1, new Book() }
                }
            };

            // Act
            var result = _fixture.Rule.CanExecute(context);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CanExecute_ShouldReturnFalse_WhenOrderDoesNotContainPhysicalBookProduct()
        {
            // Arrange
            var context = new OrderProcessingContext
            {
                Order = new PurchaseOrder
                {
                    ItemLines =
                    [
                        new() { ProductId = 1 }
                    ]
                },
                Products = new Dictionary<int, Product>
                {
                    { 1, new MembershipProduct() }
                }
            };

            // Act
            var result = _fixture.Rule.CanExecute(context);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task ExecuteAsync_ShouldGenerateShippingSlip_WhenOrderContainsPhysicalBookProduct()
        {
            // Arrange
            var context = new OrderProcessingContext
            {
                Order = new PurchaseOrder
                {
                    ItemLines =
                    [
                        new() { ProductId = 1 }
                    ]
                },
                Products = new Dictionary<int, Product>
                {
                    { 1, new Book() }
                }
            };

            // Act
            await _fixture.Rule.ExecuteAsync(context, CancellationToken.None);

            // Assert
            Mock.Get(_fixture.ShippingSlipGenerationService)
                .Verify(s => s.GenerateShippingSlipAsync(
                    context.Order,
                    It.IsAny<List<OrderItemLine>>(),
                    It.IsAny<CancellationToken>()),
                    Times.Once);

            context.GeneratedShippingSlipsCount.Should().Be(1);
        }

        private sealed class GenerateShippingSlipRuleFixture
        {
            public IShippingSlipGenerationService ShippingSlipGenerationService { get; }

            public GenerateShippingSlipRule Rule { get; }

            public GenerateShippingSlipRuleFixture()
            {
                ShippingSlipGenerationService = Mock.Of<IShippingSlipGenerationService>();
                Rule = new GenerateShippingSlipRule(ShippingSlipGenerationService);
            }
        }
    }
}
