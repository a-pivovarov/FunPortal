using FluentAssertions;
using FunPortal.Application.PurchaseOrders.Commands.Processing;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;
using Moq;

namespace FunPortal.Application.Tests.PurchaseOrders.Commands.Processing
{
    [TestClass]
    public sealed class PurchaseOrderProcessorTests
    {
        [TestMethod]
        public async Task ProcessAsync_ShouldExecuteApplicableRulesInOrder()
        {
            // Arrange
            var rule1 = new Mock<IPurchaseOrderRule>();
            rule1.Setup(r => r.CanExecute(It.IsAny<OrderProcessingContext>())).Returns(true);
            rule1.Setup(r => r.Order).Returns(2);

            var rule2 = new Mock<IPurchaseOrderRule>();
            rule2.Setup(r => r.CanExecute(It.IsAny<OrderProcessingContext>())).Returns(true);
            rule2.Setup(r => r.Order).Returns(1);

            var rule3 = new Mock<IPurchaseOrderRule>();
            rule3.Setup(r => r.CanExecute(It.IsAny<OrderProcessingContext>())).Returns(false);

            var rules = new List<IPurchaseOrderRule> { rule1.Object, rule2.Object, rule3.Object };
            
            var processor = new PurchaseOrderProcessor(rules);
            var context = new OrderProcessingContext {
                Order = It.IsAny<PurchaseOrder>(),
                Products = It.IsAny<Dictionary<int, Product>>()
            };

            var executionOrder = new List<string>();

            rule2.Setup(r => r.ExecuteAsync(context, It.IsAny<CancellationToken>()))
                 .Callback(() => executionOrder.Add(nameof(rule2)))
                 .Returns(Task.CompletedTask);

            rule1.Setup(r => r.ExecuteAsync(context, It.IsAny<CancellationToken>()))
                 .Callback(() => executionOrder.Add(nameof(rule1)))
                 .Returns(Task.CompletedTask);

            // Act
            await processor.ProcessAsync(context, CancellationToken.None);

            // Assert
            Mock.Get(rule2.Object).Verify(r => r.ExecuteAsync(context, It.IsAny<CancellationToken>()), Times.Once);
            Mock.Get(rule1.Object).Verify(r => r.ExecuteAsync(context, It.IsAny<CancellationToken>()), Times.Once);
            Mock.Get(rule3.Object).Verify(r => r.ExecuteAsync(context, It.IsAny<CancellationToken>()), Times.Never);
            executionOrder.Should().BeEquivalentTo([nameof(rule2), nameof(rule1)]);
        }
    }
}
