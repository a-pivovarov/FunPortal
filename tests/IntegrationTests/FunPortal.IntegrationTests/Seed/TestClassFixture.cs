using FunPortal.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FunPortal.Api.IntegrationTests.Seed
{
    internal class TestClassFixture : IDisposable
    {
        private readonly FunPortalWebApplicationFactory<Program> _factory;

        public HttpClient Client { get; }

        public FunPortalDbContext DbContext
            => _factory.Services.GetRequiredService<FunPortalDbContext>();

        public TestClassFixture()
        {
            _factory = new FunPortalWebApplicationFactory<Program>();
            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _factory.Dispose();
        }
    }
}
