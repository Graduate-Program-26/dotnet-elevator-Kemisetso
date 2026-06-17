namespace ElevatorSim.Infrastructure.DependencyInjection;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Application.Interfaces;
using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Interfaces;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElevatorSimulation(
        this IServiceCollection services,
        Action<BuildingSettings>? configure = null)
    {
        var settings = new BuildingSettings();
        configure?.Invoke(settings);

        services.AddSingleton(settings);
        services.AddSingleton<IDispatch, NearestElevator>();
        services.AddSingleton<IElevatorController>(serviceProvider =>
        {
            var dispatch = serviceProvider.GetRequiredService<IDispatch>();
            var buildingSettings = serviceProvider.GetRequiredService<BuildingSettings>();

            IEnumerable<IElevator> elevators = Enumerable
                .Range(1, buildingSettings.ElevatorCount)
                .Select(id => (IElevator)new PassengerElevator(id));

            return new ElevatorController(
                elevators,
                dispatch,
                buildingSettings.MinFloor,
                buildingSettings.MaxFloor);
        });

        return services;
    }
}