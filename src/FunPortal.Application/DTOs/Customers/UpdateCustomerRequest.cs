namespace FunPortal.Application.DTOs.Customers;

public class UpdateCustomerRequest
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Address { get; set; }
}
