namespace ElevatorSim.Cons.Display;

using System.Text;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Interfaces;

/// <summary>
/// Renders the elevator shaft and status table to the console.
/// </summary>
public sealed class ConsoleStatusDisplay
{
    private const int TableWidth = 72;
    private readonly BuildingSettings _settings;

    /// <summary>
    /// Creates a display bound to the given building settings.
    /// </summary>
    /// <param name="settings">Floor range and other building configuration.</param>
    public ConsoleStatusDisplay(BuildingSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Clears the console and redraws the full elevator status screen.
    /// </summary>
    /// <param name="footerLines">Optional messages shown below the status table.</param>
    public void Render(IReadOnlyList<IElevator> elevators, params FooterLine[] footerLines)
    {
        Console.Clear();
        ConsoleUI.WriteLine(new string('=', TableWidth), ConsoleColor.DarkCyan);
        ConsoleUI.WriteLine("  ELEVATOR SIMULATION", ConsoleColor.Cyan);
        ConsoleUI.WriteLine(
            $"  Floors {_settings.MinFloor}-{_settings.MaxFloor}  |  Fleet size: {elevators.Count}",
            ConsoleColor.DarkGray,
            indent: 0);
        ConsoleUI.WriteLine(new string('=', TableWidth), ConsoleColor.DarkCyan);
        Console.WriteLine();
        RenderShaft(elevators);
        ConsoleUI.WriteLine(
            $"  {"ID",-4} {"Floor",-7} {"Direction",-14} {"State",-14} {"Load",-10}",
            ConsoleColor.Gray);
        ConsoleUI.WriteLine($"  {new string('-', TableWidth - 2)}", ConsoleColor.DarkGray);

        foreach (var elevator in elevators)
        {
            var rowStyle = GetRowStyle(elevator);
            ConsoleUI.WriteLine(
                $"  {elevator.Id,-4} {elevator.CurrentFloor,-7} {FormatDirection(elevator.Direction),-14} " +
                $"{FormatState(elevator.State),-14} {elevator.PassengerCount}/{elevator.MaxCapacity}",
                ConsoleUI.GetElevatorColor(rowStyle));
        }

        Console.WriteLine();

        foreach (var line in footerLines)
        {
            if (string.IsNullOrEmpty(line.Text))
            {
                Console.WriteLine();
                continue;
            }

            ConsoleUI.WriteLine($"  {line.Text}", line.Kind, indent: 0);
        }
    }

    private void RenderShaft(IReadOnlyList<IElevator> elevators)
    {
        const int columnWidth = 8;

        var header = new StringBuilder("       |");
        foreach (var elevator in elevators)
        {
            header.Append(Center($"E{elevator.Id}", columnWidth));
        }

        ConsoleUI.WriteLine(header.ToString(), ConsoleColor.DarkGray);
        ConsoleUI.WriteLine($"       +{new string('-', columnWidth * elevators.Count)}", ConsoleColor.DarkGray);

        for (var floor = _settings.MaxFloor; floor >= _settings.MinFloor; floor--)
        {
            var row = new StringBuilder($"  {floor,4} |");

            foreach (var elevator in elevators)
            {
                var cell = elevator.CurrentFloor == floor ? FormatCar(elevator) : ".";
                row.Append(Center(cell, columnWidth));
            }

            ConsoleUI.WriteLine(row.ToString(), ConsoleColor.White);
        }

        Console.WriteLine();
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

    private static string FormatCar(IElevator elevator)
    {
        var door = elevator.State == ElevatorState.DoorsOpen ? 'o' : '|';
        return $"[{door}{elevator.Id}{door}]";
    }

    private static string Center(string text, int width)
    {
        if (text.Length >= width)
        {
            return text;
        }

        var leftPad = (width - text.Length) / 2;
        var rightPad = width - text.Length - leftPad;
        return new string(' ', leftPad) + text + new string(' ', rightPad);
    }
}
