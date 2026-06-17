using ElevatorSim.Application.Interfaces;
using ElevatorSim.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddElevatorSimulation();
var serviceProvider = services.BuildServiceProvider();

var controller = serviceProvider.GetRequiredService<IElevatorController>();

Console.WriteLine("=== Elevator Simulation ===");
Console.WriteLine($"Fleet size: {controller.Elevators.Count}");
Console.WriteLine();

foreach (var elevator in controller.Elevators)
{
    Console.WriteLine(
        $"Elevator {elevator.Id} | Floor {elevator.CurrentFloor,2} | " +
        $"{elevator.Direction,-11} | {elevator.State,-10} | " +
        $"Passengers {elevator.PassengerCount}/{elevator.MaxCapacity}");
}