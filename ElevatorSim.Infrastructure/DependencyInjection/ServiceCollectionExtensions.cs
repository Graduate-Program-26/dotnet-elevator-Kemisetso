namespace ElevatorSim.Infrastructure.DependencyInjection;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Application.Interfaces;
using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Interfaces;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Registers elevator simulation services with dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the elevator simulation stack to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configure">Callback to customize building settings.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddElevatorSimulation(
        this IServiceCollection services,
        Action<BuildingSettings>? configure = null)
    {
        var settings = new BuildingSettings();
        configure?.Invoke(settings);

        services.AddSingleton(settings);
        services.AddSingleton(sp =>
        {
            var buildingSettings = sp.GetRequiredService<BuildingSettings>();
            return new FloorManager(buildingSettings.MinFloor, buildingSettings.MaxFloor);
        });
        services.AddSingleton<IDispatchStrategy, NearestElevatorStrategy>();
        services.AddSingleton<IElevatorController>(serviceProvider =>
        {
            var dispatch = serviceProvider.GetRequiredService<IDispatchStrategy>();
            var floorManager = serviceProvider.GetRequiredService<FloorManager>();
            var buildingSettings = serviceProvider.GetRequiredService<BuildingSettings>();

            IEnumerable<IElevator> elevators = Enumerable
                .Range(1, buildingSettings.ElevatorCount)
                .Select(id => (IElevator)new PassengerElevator(id, buildingSettings.TravelDelayMilliseconds));

            return new ElevatorController(elevators, dispatch, floorManager);
        });

        return services;
    }
}
