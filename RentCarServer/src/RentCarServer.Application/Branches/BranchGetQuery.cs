﻿using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Users;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Branches;
public sealed record BranchGetQuery(
    Guid Id) : IRequest<Result<BranchDto>>;

internal sealed class BranchGetQueryHandler(
    IBranchRepository branchRepository) : IRequestHandler<BranchGetQuery, Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository
            .GetAllWithAudit()
            .MapTo()
            .Where(i => i.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (branch is null)
        {
            return Result<BranchDto>.Failure("Şube bulunamadı");
        }

        return branch;
    }
}