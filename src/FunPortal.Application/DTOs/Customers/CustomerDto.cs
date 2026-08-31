using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FunPortal.Application.DTOs.Customers;

[SwaggerSchema(Description = "Represents a customer in the system.")]
public class CustomerDto
{
    [SwaggerSchema(Description = "The unique identifier of the customer.")]
    public int CustomerId { get; set; }
    
    [Required]
    [SwaggerSchema(Description = "The name of the customer.")]
    public required string Name { get; set; }

    [Required]
    [SwaggerSchema(Description = "The email address of the customer.")]
    public required string Email { get; set; }

    [SwaggerSchema(Description = "The phone number of the customer.")]
    public string? Phone { get; set; }

    [SwaggerSchema(Description = "The address of the customer.")]
    public string? Address { get; set; }

    [SwaggerSchema(Description = "The date and time when the customer was created.")]
    public DateTime CreatedOn { get; set; }
}
