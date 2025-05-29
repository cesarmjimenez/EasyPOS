namespace Application.Customers.Common;

public record CustomerResponse : IEquatable<CustomerResponse>
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public AddressResponse Address { get; init; }
    public bool Active { get; init; }

    // Constructor to match the usage in GetAllCustomerCommandHandler  
    public CustomerResponse(Guid id, string fullName, string email, string phoneNumber, AddressResponse address, bool active)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        Active = active;
    }
}

public record AddressResponse(
    string Country,
    string Line1,
    string Line2,
    string City,
    string State,
    string ZipCode
    );