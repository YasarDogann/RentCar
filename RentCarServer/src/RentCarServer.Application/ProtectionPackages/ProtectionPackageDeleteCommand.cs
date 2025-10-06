﻿using GenericRepository;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.ProtectionPackages;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.ProtectionPackages;

[Permission("protection_package:delete")]
public sealed record ProtectionPackageDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class ProtectionPackageDeleteCommandHandler(
    IProtectionPackageRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProtectionPackageDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ProtectionPackageDeleteCommand request, CancellationToken cancellationToken)
    {
        var package = await repository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (package is null)
            return Result<string>.Failure("Güvence paketi bulunamadı");

        package.Delete();
        repository.Update(package);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Güvence paketi başarıyla silindi";
    }
}