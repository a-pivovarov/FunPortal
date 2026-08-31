using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FunPortal.Application.DTOs.Customers;

[SwaggerSchema(Description = "Represents a request to update an existing customer.")]
public class UpdateCustomerRequest
{
    [Required]
    [SwaggerSchema(Description = "The name of the customer.")]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    [SwaggerSchema(Description = "The email address of the customer.")]
    public required string Email { get; set; }

    [SwaggerSchema(Description = "The phone number of the customer.")]
    public string? Phone { get; set; }

    [SwaggerSchema(Description = "The address of the customer.")]
    public string? Address { get; set; }
}
