﻿using FluentValidation;
using GenericFileService.Files;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;
using RentCarServer.Domain.Vehicles;
using RentCarServer.Domain.Vehicles.ValueObjects;
using TS.MediatR;
using TS.Result;

namespace RentCarServer.Application.Vehicles;

[Permission("vehicle:update")]
public sealed record VehicleUpdateCommand(
    Guid Id,
    string Brand,
    string Model,
    int ModelYear,
    string Color,
    string Plate,
    Guid CategoryId,
    Guid BranchId,
    string VinNumber,
    string EngineNumber,
    string Description,
    string FuelType,
    string Transmission,
    decimal EngineVolume,
    int EnginePower,
    string TractionType,
    decimal FuelConsumption,
    int SeatCount,
    int Kilometer,
    decimal DailyPrice,
    decimal WeeklyDiscountRate,
    decimal MonthlyDiscountRate,
    string InsuranceType,
    DateTimeOffset LastMaintenanceDate,
    int LastMaintenanceKm,
    int NextMaintenanceKm,
    DateTimeOffset InspectionDate,
    DateTimeOffset InsuranceEndDate,
    DateTimeOffset CascoEndDate,
    string TireStatus,
    string GeneralStatus,
    List<string> Features,
    IFormFile? File,
    bool IsActive
) : IRequest<Result<string>>;

public sealed class VehicleUpdateCommandValidator : AbstractValidator<VehicleUpdateCommand>
{
    public VehicleUpdateCommandValidator()
    {
        RuleFor(p => p.Brand).NotEmpty();
        RuleFor(p => p.Model).NotEmpty();
        RuleFor(p => p.ModelYear).GreaterThan(1900);
        RuleFor(p => p.Plate).NotEmpty();
    }
}

internal sealed class VehicleUpdateCommandHandler(
    IVehicleRepository vehicleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<VehicleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(VehicleUpdateCommand request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await vehicleRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (vehicle is null)
            return Result<string>.Failure("Araç bulunamadı");

        if (!string.Equals(vehicle.Plate.Value, request.Plate, StringComparison.OrdinalIgnoreCase))
        {
            bool plateExists = await vehicleRepository.AnyAsync(
                p => p.Plate.Value == request.Plate && p.Id != request.Id, cancellationToken);
            if (plateExists)
                return Result<string>.Failure("Bu plaka ile kayıtlı başka bir araç var.");
        }

        string fileName = vehicle.ImageUrl.Value;
        if (request.File is not null && request.File.Length > 0)
        {
            fileName = FileService.FileSaveToServer(request.File, "wwwroot/images/");
        }

        Brand brand = new(request.Brand);
        Model model = new(request.Model);
        ModelYear modelYear = new(request.ModelYear);
        Color color = new(request.Color);
        Plate plate = new(request.Plate);
        IdentityId categoryId = new(request.CategoryId);
        IdentityId branchId = new(request.BranchId);
        VinNumber vinNumber = new(request.VinNumber);
        EngineNumber engineNumber = new(request.EngineNumber);
        Description description = new(request.Description);
        ImageUrl imageUrl = new(fileName);
        FuelType fuelType = new(request.FuelType);
        Transmission transmission = new(request.Transmission);
        EngineVolume engineVolume = new(request.EngineVolume);
        EnginePower enginePower = new(request.EnginePower);
        TractionType tractionType = new(request.TractionType);
        FuelConsumption fuelConsumption = new(request.FuelConsumption);
        SeatCount seatCount = new(request.SeatCount);
        Kilometer kilometer = new(request.Kilometer);
        DailyPrice dailyPrice = new(request.DailyPrice);
        WeeklyDiscountRate weeklyDiscountRate = new(request.WeeklyDiscountRate);
        MonthlyDiscountRate monthlyDiscountRate = new(request.MonthlyDiscountRate);
        InsuranceType insuranceType = new(request.InsuranceType);
        LastMaintenanceDate lastMaintenanceDate = new(request.LastMaintenanceDate);
        LastMaintenanceKm lastMaintenanceKm = new(request.LastMaintenanceKm);
        NextMaintenanceKm nextMaintenanceKm = new(request.NextMaintenanceKm);
        InspectionDate inspectionDate = new(request.InspectionDate);
        InsuranceEndDate insuranceEndDate = new(request.InsuranceEndDate);
        CascoEndDate cascoEndDate = new(request.CascoEndDate);
        TireStatus tireStatus = new(request.TireStatus);
        GeneralStatus generalStatus = new(request.GeneralStatus);
        IEnumerable<Feature> features = request.Features.Select(f => new Feature(f));

        vehicle.SetBrand(brand);
        vehicle.SetModel(model);
        vehicle.SetModelYear(modelYear);
        vehicle.SetColor(color);
        vehicle.SetPlate(plate);
        vehicle.SetCategoryId(categoryId);
        vehicle.SetBranchId(branchId);
        vehicle.SetVinNumber(vinNumber);
        vehicle.SetEngineNumber(engineNumber);
        vehicle.SetDescription(description);
        vehicle.SetImageUrl(imageUrl);
        vehicle.SetFuelType(fuelType);
        vehicle.SetTransmission(transmission);
        vehicle.SetEngineVolume(engineVolume);
        vehicle.SetEnginePower(enginePower);
        vehicle.SetTractionType(tractionType);
        vehicle.SetFuelConsumption(fuelConsumption);
        vehicle.SetSeatCount(seatCount);
        vehicle.SetKilometer(kilometer);
        vehicle.SetDailyPrice(dailyPrice);
        vehicle.SetWeeklyDiscountRate(weeklyDiscountRate);
        vehicle.SetMonthlyDiscountRate(monthlyDiscountRate);
        vehicle.SetInsuranceType(insuranceType);
        vehicle.SetLastMaintenanceDate(lastMaintenanceDate);
        vehicle.SetLastMaintenanceKm(lastMaintenanceKm);
        vehicle.SetNextMaintenanceKm(nextMaintenanceKm);
        vehicle.SetInspectionDate(inspectionDate);
        vehicle.SetInsuranceEndDate(insuranceEndDate);
        vehicle.SetCascoEndDate(cascoEndDate);
        vehicle.SetTireStatus(tireStatus);
        vehicle.SetGeneralStatus(generalStatus);
        vehicle.SetFeatures(features);
        vehicle.SetStatus(request.IsActive);

        vehicleRepository.Update(vehicle);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Araç başarıyla güncellendi";
    }
}