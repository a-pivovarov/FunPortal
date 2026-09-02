using FluentAssertions;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing;
using FunPortal.Application.Features.PurchaseOrders.Commands.Processing.Rules;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Domain.Entities;
using FunPortal.Domain.Entities.Products;
using FunPortal.Domain.Enums;
using Moq;

namespace FunPortal.Application.Tests.Features.PurchaseOrders.Commands.Processing.Rules
{
    [TestClass]
    public sealed class ActivateMembershipRuleTests
    {
        private ActivateMembershipRuleFixture _fixture = default!;

        [TestInitialize]
        public void TestInitialize()
            => _fixture = new ActivateMembershipRuleFixture();

        [TestMethod]
        public void Order_ShouldReturnCorrectValue()
        {
            // Assert
            _fixture.Rule.Order.Should().Be(1);
        }

        [TestMethod]
        public void CanExecute_ShouldReturnTrue_WhenOrderContainsMembershipProduct()
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
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CanExecute_ShouldReturnFalse_WhenOrderDoesNotContainMembershipProduct()
        {
            // Arrange
            var context = new OrderProcessingContext
            {
                Order = new PurchaseOrder
                {
                    ItemLines =
                    [
                        new() { ProductId = 1 },
                        new() { ProductId = 2 }
                    ]
                },
                Products = new Dictionary<int, Product>
                {
                    { 1, new Book() },
                    { 2, new Video() },
                }
            };

            // Act
            var result = _fixture.Rule.CanExecute(context);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task ExecuteAsync_ShouldActivateMemberships_ForMembershipProductsInOrder()
        {
            // Arrange
            var membershipProduct = new MembershipProduct
            {
                ProductId = 1,
                MembershipType = MembershipType.Premium,
                DurationMonths = 12
            };

            var context = new OrderProcessingContext
            {
                Order = new PurchaseOrder
                {
                    UserId = 123,
                    ItemLines =
                    [
                        new() { ProductId = 1 }
                    ]
                },
                Products = new Dictionary<int, Product>
                {
                    { 1, membershipProduct }
                }
            };

            Mock.Get(_fixture.MembershipActivationService)
                .Setup(s => s.ActivateMembershipAsync(
                    context.Order.UserId,
                    membershipProduct.MembershipType,
                    membershipProduct.DurationMonths,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Membership());

            // Act
            await _fixture.Rule.ExecuteAsync(context, CancellationToken.None);

            // Assert
            Mock.Get(_fixture.MembershipActivationService)
                .Verify(
                    s => s.ActivateMembershipAsync(
                            context.Order.UserId,
                            membershipProduct.MembershipType,
                            membershipProduct.DurationMonths,
                        It.IsAny<CancellationToken>()),
                    Times.Once);

            context.ActivatedMembershipsCount.Should().Be(1);
        }

        private sealed class ActivateMembershipRuleFixture
        {
            public IMembershipActivationService MembershipActivationService { get; }

            public ActivateMembershipRule Rule { get; }

            public ActivateMembershipRuleFixture()
            {
                MembershipActivationService = Mock.Of<IMembershipActivationService>();
                Rule = new ActivateMembershipRule(MembershipActivationService);
            }
        }
    }
}
