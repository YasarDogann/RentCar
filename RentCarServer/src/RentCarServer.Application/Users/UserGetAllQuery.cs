﻿using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Users;
using TS.MediatR;

namespace RentCarServer.Application.Users;
public sealed record UserGetAllQuery : IRequest<IQueryable<UserDto>>;

internal sealed class UserGetAllQueryHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IBranchRepository branchRepository) : IRequestHandler<UserGetAllQuery, IQueryable<UserDto>>
{
    public Task<IQueryable<UserDto>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
        => Task.FromResult(
            userRepository
            .GetAllWithAudit()
            .MapTo(roleRepository
            .GetAll(), branchRepository.GetAll()));
}