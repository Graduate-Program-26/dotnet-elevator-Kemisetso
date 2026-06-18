namespace ElevatorSim.Cons.Display;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Interfaces;

public sealed class ConsoleStatusDisplay
{
    private const int TableWidth = 72;
    private readonly BuildingSettings _settings;

    public ConsoleStatusDisplay(BuildingSettings settings)
    {
        _settings = settings;
    }

    public void Render(IReadOnlyList<IElevator> elevators, params FooterLine[] footerLines)
    {
        Console.Clear();
        ConsoleUi.WriteLine(new string('=', TableWidth), ConsoleColor.DarkCyan);
        ConsoleUi.WriteLine("  ELEVATOR SIMULATION", ConsoleColor.Cyan);
        ConsoleUi.WriteLine(
            $"  Floors {_settings.MinFloor}-{_settings.MaxFloor}  |  Fleet size: {elevators.Count}",
            ConsoleColor.DarkGray,
            indent: 0);
        ConsoleUi.WriteLine(new string('=', TableWidth), ConsoleColor.DarkCyan);
        Console.WriteLine();
        ConsoleUi.WriteLine(
            $"  {"ID",-4} {"Floor",-7} {"Direction",-14} {"State",-14} {"Load",-10}",
            ConsoleColor.Gray);
        ConsoleUi.WriteLine($"  {new string('-', TableWidth - 2)}", ConsoleColor.DarkGray);

        foreach (var elevator in elevators)
        {
            var rowStyle = GetRowStyle(elevator);
            ConsoleUi.WriteLine(
                $"  {elevator.Id,-4} {elevator.CurrentFloor,-7} {FormatDirection(elevator.Direction),-14} " +
                $"{FormatState(elevator.State),-14} {elevator.PassengerCount}/{elevator.MaxCapacity}",
                ConsoleUi.GetElevatorColor(rowStyle));
        }

        Console.WriteLine();

        foreach (var line in footerLines)
        {
            if (string.IsNullOrEmpty(line.Text))
            {
                Console.WriteLine();
                continue;
            }

            ConsoleUi.WriteLine($"  {line.Text}", line.Kind, indent: 0);
        }
    }

    private static ElevatorRowStyle GetRowStyle(IElevator elevator)
    {
        if (elevator.PassengerCount >= elevator.MaxCapacity)
        {
            return ElevatorRowStyle.Full;
        }

        return elevator.State switch
        {
            ElevatorState.Moving => ElevatorRowStyle.Moving,
            ElevatorState.DoorsOpen => ElevatorRowStyle.DoorsOpen,
            _ => ElevatorRowStyle.Available
        };
    }

    private static string FormatDirection(ElevatorDirection direction) => direction switch
    {
        ElevatorDirection.Up => "Up",
        ElevatorDirection.Down => "Down",
        _ => "Idle"
    };

    private static string FormatState(ElevatorState state) => state switch
    {
        ElevatorState.Moving => "Moving",
        ElevatorState.DoorsOpen => "Doors open",
        _ => "Available"
    };
}
