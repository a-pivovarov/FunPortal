using FluentAssertions;
using FunPortal.Api.IntegrationTests.Seed;
using FunPortal.Application.DTOs.Enums;
using FunPortal.Application.DTOs.Products;
using System.Net;
using System.Net.Http.Json;

namespace FunPortal.Api.IntegrationTests.Controllers.v1
{
    [TestClass]
    public sealed class ProductsControllerTests
    {
        private static TestClassFixture _fixture = default!;

        [ClassInitialize]
        public static void TestInitialize(TestContext context)
            => _fixture = new TestClassFixture();

        [ClassCleanup]
        public static void TestCleanup()
            => _fixture.Dispose();

        [TestMethod]
        public async Task GetProducts_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = "/api/v1/products";

            // Act
            var response = await _fixture.Client.GetAsync(request, CancellationToken.None);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task CreateProduct_ReturnsOkStatusCode_And_CreatesProductInDb()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Test Product",
                Price = 9.99m,
                ProductType = ProductType.PhysicalBook,
                Author = "Test Author",
                ISBN = "1234567890"
            };

            // Act
            var response = await _fixture.Client
                .PostAsJsonAsync("/api/v1/products", request, CancellationToken.None);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var existingProductsResponse = await _fixture.Client
                .GetAsync("/api/v1/products", CancellationToken.None);

            var existingProducts = await existingProductsResponse.Content
                .ReadFromJsonAsync<List<ProductDto>>(cancellationToken: CancellationToken.None);

            existingProducts
                .Should()
                .ContainSingle(p => p.Name == request.Name &&
                    p.Price == request.Price &&
                    p.ProductType == request.ProductType &&
                    p.Author == request.Author &&
                    p.ISBN == request.ISBN);
        }
    }
}
