namespace ElevatorSim.Cons.Simulation;

using ElevatorSim.Application.Configuration;
using ElevatorSim.Application.Interfaces;
using ElevatorSim.Cons.Display;
using ElevatorSim.Domain.Exceptions;

/// <summary>
/// Runs the interactive elevator simulation from the console.
/// </summary>
public sealed class ElevatorSimulationApp
{
    private static readonly FooterLine MenuLine = FooterLine.Menu("Commands: [C]all elevator  [Q]uit");

    private readonly IElevatorController _controller;
    private readonly BuildingSettings _settings;
    private readonly ConsoleStatusDisplay _display;

    /// <summary>
    /// Creates the simulation app with its dependencies.
    /// </summary>
    /// <param name="controller">The elevator controller to dispatch requests through.</param>
    /// <param name="settings">Building configuration for floor validation and display.</param>
    /// <param name="display">The console display to render status updates.</param>
    public ElevatorSimulationApp(
        IElevatorController controller,
        BuildingSettings settings,
        ConsoleStatusDisplay display)
    {
        _controller = controller;
        _settings = settings;
        _display = display;
    }

    /// <summary>
    /// Starts the simulation and processes user commands until quit.
    /// </summary>
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
                    ConsoleUI.WriteLine("Goodbye.", MessageKind.Info, indent: 2);
                    return;
                default:
                    RenderScreen(FooterLine.Warning("Unknown command. Use C or Q."));
                    break;
            }
        }
    }

    private async Task HandleCallElevatorAsync()
    {
        Console.WriteLine();
        var pickupFloor = ReadFloor("Pickup floor", out var pickupError);
        if (pickupError is not null)
        {
            RenderScreen(FooterLine.Error(pickupError));
            return;
        }

        var destinationFloor = ReadFloor("Destination floor", out var destinationError);
        if (destinationError is not null)
        {
            RenderScreen(FooterLine.Error(destinationError));
            return;
        }

        if (pickupFloor == destinationFloor)
        {
            RenderScreen(FooterLine.Error("Pickup and destination floors must be different."));
            return;
        }

        var passengers = ReadPassengerCount(out var passengerError);
        if (passengerError is not null)
        {
            RenderScreen(FooterLine.Error(passengerError));
            return;
        }

        var statusMessage = FooterLine.Info(
            $"Dispatching to floor {pickupFloor} for {passengers} passenger(s) travelling to floor {destinationFloor}...");

        try
        {
            await RunWithLiveDisplayAsync(
                () => _controller.RequestElevator(pickupFloor, destinationFloor, passengers),
                statusMessage);

            RenderScreen(FooterLine.Success($"Elevator request for floor {pickupFloor} completed."));
        }
        catch (InvalidFloorException ex)
        {
            RenderScreen(FooterLine.Error(ex.Message));
        }
        catch (NoAvailableElevatorException ex)
        {
            RenderScreen(FooterLine.Error(ex.Message));
        }
        catch (CapacityExceededException ex)
        {
            RenderScreen(FooterLine.Error(ex.Message));
        }
        catch (ArgumentException ex)
        {
            RenderScreen(FooterLine.Error(ex.Message));
        }
    }

    private int ReadFloor(string label, out string? errorMessage)
    {
        Console.Write($"  {label} ({_settings.MinFloor}-{_settings.MaxFloor}): ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var floor))
        {
            errorMessage = "Invalid floor. Enter a number.";
            return 0;
        }

        errorMessage = null;
        return floor;
    }

    private int ReadPassengerCount(out string? errorMessage)
    {
        Console.Write("  Passengers waiting: ");
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var count) || count <= 0)
        {
            errorMessage = "Invalid passenger count. Enter a positive number.";
            return 0;
        }

        errorMessage = null;
        return count;
    }

    private async Task RunWithLiveDisplayAsync(Func<Task> action, FooterLine statusMessage)
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

    private async Task RefreshLoopAsync(CancellationToken cancellationToken, FooterLine statusMessage)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _display.Render(_controller.Elevators, statusMessage, FooterLine.Plain(string.Empty), MenuLine);
                await Task.Delay(150, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void RenderScreen(params FooterLine[] messages)
    {
        var footer = messages.Length > 0
            ? messages.Append(FooterLine.Plain(string.Empty)).Append(MenuLine).ToArray()
            : [MenuLine];

        _display.Render(_controller.Elevators, footer);
    }
}
