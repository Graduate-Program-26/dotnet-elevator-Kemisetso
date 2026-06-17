namespace ElevatorSim.Cons.Simulation;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Application.Interfaces;
using ElevatorSim.Cons.Display;
using ElevatorSim.Domain.Exceptions;

public sealed class ElevatorSimulationApp
{
    private const string MenuText = "Commands: [C]all elevator  [Q]uit";

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
        RenderScreen();
        await RunCommandLoopAsync();
    }

    private async Task RunCommandLoopAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write("  > ");
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
                case "Q":
                    Console.WriteLine();
                    ConsoleUi.WriteInfo("Goodbye.");
                    return;
                default:
                    RenderScreen("Unknown command. Use C or Q.");
                    break;
            }
        }
    }

    private async Task HandleCallElevatorAsync()
    {
        Console.WriteLine();
        var pickupFloor = ReadFloor("Pickup floor");
        if (pickupFloor is null)
        {
            RenderScreen();
            return;
        }

        var destinationFloor = ReadFloor("Destination floor");
        if (destinationFloor is null)
        {
            RenderScreen();
            return;
        }

        if (pickupFloor == destinationFloor)
        {
            RenderScreen("Pickup and destination floors must be different.");
            return;
        }

        var passengers = ReadPassengerCount();
        if (passengers is null)
        {
            RenderScreen();
            return;
        }

        var statusMessage =
            $"Dispatching to floor {pickupFloor} for {passengers} passenger(s) travelling to floor {destinationFloor}...";

        try
        {
            await RunWithLiveDisplayAsync(
                () => _controller.RequestElevator(pickupFloor.Value, destinationFloor.Value, passengers.Value),
                statusMessage);

            RenderScreen($"Elevator request for floor {pickupFloor} completed.");
        }
        catch (InvalidFloorException ex)
        {
            RenderScreen(ex.Message);
        }
        catch (NoAvailableElevatorException ex)
        {
            RenderScreen(ex.Message);
        }
        catch (CapacityExceededException ex)
        {
            RenderScreen(ex.Message);
        }
        catch (ArgumentException ex)
        {
            RenderScreen(ex.Message);
        }
    }

    private int? ReadFloor(string label)
    {
        Console.Write($"  {label} ({_settings.MinFloor}-{_settings.MaxFloor}): ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var floor))
        {
            ConsoleUi.WriteError("Invalid floor. Enter a number.");
            return null;
        }

        return floor;
    }

    private int? ReadPassengerCount()
    {
        Console.Write("  Passengers waiting: ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var count) || count <= 0)
        {
            ConsoleUi.WriteError("Invalid passenger count. Enter a positive number.");
            return null;
        }

        return count;
    }

    private async Task RunWithLiveDisplayAsync(Func<Task> action, string statusMessage)
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var displayTask = RefreshLoopAsync(cancellationTokenSource.Token, statusMessage);

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

    private async Task RefreshLoopAsync(CancellationToken cancellationToken, string statusMessage)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _display.Render(_controller.Elevators, statusMessage);
                await Task.Delay(150, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void RenderScreen(params string[] messages)
    {
        var footer = messages.Length > 0
            ? messages.Append(string.Empty).Append(MenuText).ToArray()
            : [MenuText];

        _display.Render(_controller.Elevators, footer);
    }
}
