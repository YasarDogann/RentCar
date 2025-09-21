namespace RentCarServer.Domain.Branches.ValueObjects;

public sealed record Address(
    string City,
    string District,
    string FullAddress,
    string PhoneNumber1,
    string PhoneNumber2,
    string Email
    );