namespace ElevatorSim.Cons.Display;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Domain.Interfaces;

public sealed class ConsoleStatusDisplay
{
    private readonly BuildingSettings _settings;

    public ConsoleStatusDisplay(BuildingSettings settings)
    {
        _settings = settings;
    }

    public void Render(IReadOnlyList<IElevator> elevators)
    {
        Console.Clear();
        Console.WriteLine("=== Elevator Simulation ===");
        Console.WriteLine($"Floors: {_settings.MinFloor}-{_settings.MaxFloor}  |  Elevators: {elevators.Count}");
        Console.WriteLine(new string('-', 70));
        Console.WriteLine($"{"Id",-4} {"Floor",-7} {"Direction",-12} {"State",-12} {"Passengers",-12}");
        Console.WriteLine(new string('-', 70));

        foreach (var elevator in elevators)
        {
            Console.WriteLine(
                $"{elevator.Id,-4} {elevator.CurrentFloor,-7} {elevator.Direction,-12} {elevator.State,-12} " +
                $"{elevator.PassengerCount}/{elevator.MaxCapacity}");
        }

        Console.WriteLine();
    }
}