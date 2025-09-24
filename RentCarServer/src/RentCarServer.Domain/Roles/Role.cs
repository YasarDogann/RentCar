using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;

namespace RentCarServer.Domain.Roles;
public sealed class Role : Entity
{
    public Name Name { get; private set; } = default!;
}