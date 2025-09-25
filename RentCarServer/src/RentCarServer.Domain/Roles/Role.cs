using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;

namespace RentCarServer.Domain.Roles;
public sealed class Role : Entity
{
    private Role() { }
    public Role(Name name)
    {
        SetName(name);
    }
    public Name Name { get; private set; } = default!;

    #region Behaviors
    public void SetName(Name name)
    {
        Name = name;
    }
    #endregion
}