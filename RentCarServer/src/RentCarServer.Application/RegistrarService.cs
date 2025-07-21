﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Application.Behaviors;
using TS.MediatR;

namespace RentCarServer.Application;
public static class RegistrarService
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfr =>
        {
            cfr.RegisterServicesFromAssembly(typeof(RegistrarService).Assembly);
            cfr.AddOpenBehavior(typeof(ValidationBehavior<,>)); // ressponce olduğu için "," koy response olmasa "," koyulmazdı
            cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(RegistrarService).Assembly); // Fluent Validation oto çalışması için eklemek lazım

        return services;
    }
}
