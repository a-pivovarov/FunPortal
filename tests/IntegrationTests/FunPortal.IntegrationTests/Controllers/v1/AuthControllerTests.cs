using FluentAssertions;
using FunPortal.Api.IntegrationTests.Seed;
using FunPortal.Application.DTOs.Auth;
using FunPortal.Application.DTOs.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace FunPortal.Api.IntegrationTests.Controllers.v1
{
    [TestClass]
    public sealed class AuthControllerTests
    {
        private static TestClassFixture _fixture = default!;

        [ClassInitialize]
        public static void TestInitialize(TestContext context)
            => _fixture = new TestClassFixture();

        [ClassCleanup]
        public static void TestCleanup()
            => _fixture.Dispose();

        [TestMethod]
        public async Task RegisterUser_ReturnsCreatedStatusCode_And_CreatesUserInDb()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "TestPassword123!",
                Role = UserRole.Admin,
                Phone = "123-456-7890",
                Address = "123 Test St, Test City, Test Country"
            };

            // Act
            var response = await _fixture.Client
                .PostAsJsonAsync("/api/v1/auth/register", request, CancellationToken.None);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdUser = await _fixture.DbContext.Users
                .SingleOrDefaultAsync(u => u.Email == request.Email, CancellationToken.None);

            createdUser.Should().NotBeNull();
            createdUser.Username.Should().Be(request.Username);
            createdUser.Email.Should().Be(request.Email);
            createdUser.Role.Should().Be((Domain.Enums.UserRole)request.Role);
            createdUser.Phone.Should().Be(request.Phone);
            createdUser.Address.Should().Be(request.Address);
        }
    }
}
