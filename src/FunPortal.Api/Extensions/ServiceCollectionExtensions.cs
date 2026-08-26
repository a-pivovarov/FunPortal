using FluentValidation;
using FunPortal.Application.Behaviors;
using FunPortal.Application.Interfaces.Repositories;
using FunPortal.Application.Interfaces.Services;
using FunPortal.Application.PurchaseOrders.Commands.Processing;
using FunPortal.Application.PurchaseOrders.Commands.Processing.Rules;
using FunPortal.Infrastructure.Repositories;
using FunPortal.Infrastructure.Services;
using MediatR;
using System.Reflection;

namespace FunPortal.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
                {
                    Title = "FunBooksAndVideos API",
                    Version = "v1",
                    Description = "E-commerce API for books, videos, and memberships with automated business rules"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        internal static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            // Add MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(Application.Customers.Commands.CreateCustomerCommand).Assembly));

            // Add FluentValidation
            services.AddValidatorsFromAssembly(
                typeof(Application.Validators.Customers.CreateCustomerRequestValidator).Assembly);

            // Add FluentValidation to MediatR pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IShippingSlipRepository, ShippingSlipRepository>();

            return services;
        }

        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMembershipActivationService, MembershipActivationService>();
            services.AddScoped<IShippingSlipGenerationService, ShippingSlipGenerationService>();

            // Purchase Order Processing Rules Engine
            services.AddScoped<IPurchaseOrderProcessor, PurchaseOrderProcessor>();
            services.AddScoped<IPurchaseOrderRule, ActivateMembershipRule>();
            services.AddScoped<IPurchaseOrderRule, GenerateShippingSlipRule>();
            services.AddScoped<IPurchaseOrderRule, CompleteOrderRule>();

            return services;
        }
    }
}
