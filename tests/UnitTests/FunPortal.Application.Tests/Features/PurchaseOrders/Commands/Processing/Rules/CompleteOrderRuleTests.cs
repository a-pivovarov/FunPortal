using FluentAssertions;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing.Rules;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Enums;

namespace FunPortal.Application.Tests.Features.PurchaseOrders.Commands.Processing.Rules
{
    [TestClass]
    public sealed class CompleteOrderRuleTests
    {
        [TestMethod]
        public void Order_ShouldReturnCorrectValue()
        {
            // Arrange
            var rule = new CompleteOrderRule();

            // Assert
            rule.Order.Should().Be(999);
        }

        [TestMethod]
        public void CanExecute_ShouldAlwaysReturnTrue()
        {
            // Arrange
            var rule = new CompleteOrderRule();
            var context = new OrderProcessingContext
            {
                Order = new PurchaseOrder(),
                Products = []
            };

            // Act
            var result = rule.CanExecute(context);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task ExecuteAsync_ShouldMarkOrderAsCompleted()
        {
            // Arrange
            var rule = new CompleteOrderRule();
            var context = new OrderProcessingContext
            {
                Order = new PurchaseOrder { Status = OrderStatus.Pending },
                Products = []
            };

            // Act
            await rule.ExecuteAsync(context, CancellationToken.None);
            
            // Assert
            context.Order.Status.Should().Be(OrderStatus.Completed);
        }
    }
}
