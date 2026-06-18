namespace ElevatorSim.Domain.Entities;

using ElevatorSim.Domain.Enums;

/// <summary>
/// A passenger elevator that moves one floor at a time.
/// </summary>
public class PassengerElevator : ElevatorBase
{
    private const int DefaultMaxCapacity = 10;
    private const int FreightDelayDivisor = 3;
    private const int HighSpeedDelayMilliseconds = 100;

    private readonly int _travelDelayMilliseconds;

    public override int MaxCapacity => DefaultMaxCapacity;

    /// <summary>
    /// Creates a passenger elevator.
    /// </summary>
    /// <param name="id">The unique identifier for elevator.</param>
    /// <param name="travelDelayMilliseconds">Delay between floor moves, for simulation pacing.</param>
    public PassengerElevator(int id, int travelDelayMilliseconds = 0) : base(id)
    {
        _travelDelayMilliseconds = travelDelayMilliseconds;
    }

    /// <inheritdoc />
    public override async Task MoveToFloor(int floor, ElevatorType tripMode = ElevatorType.Normal)
    {
        if (floor == CurrentFloor)
        {
            return;
        }

        State = ElevatorState.Moving;
        Direction = floor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

        if (tripMode == ElevatorType.HighSpeed)
        {
            CurrentFloor = floor;
            if (_travelDelayMilliseconds > 0)
            {
                await Task.Delay(HighSpeedDelayMilliseconds);
            }
        }
        else
        {
            var delayPerFloor = tripMode == ElevatorType.Freight
                ? Math.Max(1, _travelDelayMilliseconds / FreightDelayDivisor)
                : _travelDelayMilliseconds;

            while (CurrentFloor != floor)
            {
                CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
                if (delayPerFloor > 0)
                {
                    await Task.Delay(delayPerFloor);
                }
            }
        }

        Direction = ElevatorDirection.Stationary;
        State = ElevatorState.DoorsOpen;
    }
}
