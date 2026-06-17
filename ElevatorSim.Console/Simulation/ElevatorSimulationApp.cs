namespace ElevatorSim.Cons.Simulation;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Application.Interfaces;
using ElevatorSim.Cons.Display;
using ElevatorSim.Domain.Exceptions;

public sealed class ElevatorSimulationApp
{
    private readonly IElevatorController _controller;
    private readonly BuildingSettings _settings;
    private readonly ConsoleStatusDisplay _display;

    public ElevatorSimulationApp(
        IElevatorController controller,
        BuildingSettings settings,
        ConsoleStatusDisplay display)
    {
        _controller = controller;
        _settings = settings;
        _display = display;
    }

    public async Task RunAsync()
    {
        _display.Render(_controller.Elevators);
        PrintMenu();

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim().ToUpperInvariant();

            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            switch (input)
            {
                case "C":
                    await HandleCallElevatorAsync();
                    break;
                case "S":
                    _display.Render(_controller.Elevators);
                    PrintMenu();
                    break;
                case "Q":
                    Console.WriteLine("Goodbye.");
                    return;
                default:
                    Console.WriteLine("Unknown command. Use C, S, or Q.");
                    break;
            }
        }
    }

    private async Task HandleCallElevatorAsync()
    {
        var pickupFloor = ReadFloor("Pickup floor");
        if (pickupFloor is null)
        {
            return;
        }

        var destinationFloor = ReadFloor("Destination floor");
        if (destinationFloor is null)
        {
            return;
        }

        if (pickupFloor == destinationFloor)
        {
            Console.WriteLine("Pickup and destination floors must be different.");
            return;
        }

        var passengers = ReadPassengerCount();
        if (passengers is null)
        {
            return;
        }

        try
        {
            Console.WriteLine(
                $"Dispatching to floor {pickupFloor} for {passengers} passenger(s) travelling to floor {destinationFloor}...");

            await RunWithLiveDisplayAsync(
                () => _controller.RequestElevator(pickupFloor.Value, destinationFloor.Value, passengers.Value));

            _display.Render(_controller.Elevators);
            PrintMenu();
            Console.WriteLine($"Request for floor {pickupFloor} completed.");
        }
        catch (InvalidFloorException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (NoAvailableElevatorException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (CapacityExceededException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private int? ReadFloor(string label)
    {
        Console.Write($"{label} ({_settings.MinFloor}-{_settings.MaxFloor}): ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var floor))
        {
            Console.WriteLine("Invalid floor. Enter a number.");
            return null;
        }

        return floor;
    }

    private int? ReadPassengerCount()
    {
        Console.Write("Passengers waiting: ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var count) || count <= 0)
        {
            Console.WriteLine("Invalid passenger count. Enter a positive number.");
            return null;
        }

        return count;
    }

    private async Task RunWithLiveDisplayAsync(Func<Task> action)
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var displayTask = RefreshLoopAsync(cancellationTokenSource.Token);

        try
        {
            await action();
        }
        finally
        {
            cancellationTokenSource.Cancel();
            try
            {
                await displayTask;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    private async Task RefreshLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _display.Render(_controller.Elevators);
                await Task.Delay(150, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("Commands: [C]all elevator  [S]tatus  [Q]uit");
    }
}