using AutoFixture;
using FluentAssertions;
using FunPortal.Application.Customers.Commands;
using FunPortal.Application.DTOs.Customers;
using FunPortal.Application.Interfaces.Persistence;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Domain.Entities;
using Moq;

namespace FunPortal.Application.Tests.Customers.Commands
{
    [TestClass]
    public sealed class CreateCustomerCommandTests
    {
        private CreateCustomerCommandFixture _fixture = default!;

        [TestInitialize]
        public void TestInitialize()
            => _fixture = new CreateCustomerCommandFixture();

        [TestMethod]
        public async Task Handle_CustomerWithTheSameEmailExists_ShouldNotCreateCustomer()
        {
            // Arrange
            var command = _fixture.CreateCommand();

            var existingCustomer = _fixture.CreateCustomer(command.Request.Email);

            Mock.Get(_fixture.CustomerRepository)
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCustomer);

            // Act
            var action = async () => await _fixture.Handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage($"Customer with email '{command.Request.Email}' already exists.");

            Mock.Get(_fixture.CustomerRepository)
                .Verify(x => x.Add(It.IsAny<Customer>()), Times.Never);

            Mock.Get(_fixture.UnitOfWork)
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_RequestIsValid_ShouldCreateCustomer()
        {
            // Arrange
            var command = _fixture.CreateCommand();

            var expectedCustomer = new Customer
            {
                Name = command.Request.Name,
                Email = command.Request.Email,
                Phone = command.Request.Phone,
                Address = command.Request.Address,
                CreatedOn = DateTime.UtcNow
            };

            Mock.Get(_fixture.CustomerRepository)
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer?)null);

            Mock.Get(_fixture.CustomerRepository)
                .Setup(x => x.Add(It.IsAny<Customer>()))
                .Returns(expectedCustomer);

            // Act
            var result = await _fixture.Handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedCustomer);

            Mock.Get(_fixture.CustomerRepository)
                .Verify(
                    x => x.Add(It.Is<Customer>(c =>
                        c.Name == command.Request.Name &&
                        c.Email == command.Request.Email &&
                        c.Phone == command.Request.Phone &&
                        c.Address == command.Request.Address)),
                    Times.Once);

            Mock.Get(_fixture.UnitOfWork)
                .Verify(
                    x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        private sealed class CreateCustomerCommandFixture
        {
            private Fixture Fixture { get; }

            public ICustomerRepository CustomerRepository { get; }

            public IUnitOfWork UnitOfWork { get; }

            public CreateCustomerCommandHandler Handler { get; }

            public CreateCustomerCommandFixture()
            {
                Fixture = new Fixture();
                CustomerRepository = Mock.Of<ICustomerRepository>();
                UnitOfWork = Mock.Of<IUnitOfWork>();
                Handler = new CreateCustomerCommandHandler(CustomerRepository, UnitOfWork);
            }

            public CreateCustomerCommand CreateCommand()
            {
                var request = Fixture.Create<CreateCustomerRequest>();
                return new CreateCustomerCommand(request);
            }

            public Customer CreateCustomer(string email)
                => Fixture.Build<Customer>()
                    .With(c => c.Email, email)
                    .Create();
        }
    }
}
