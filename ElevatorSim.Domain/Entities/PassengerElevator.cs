namespace ElevatorSim.Domain.Entities;

using ElevatorSim.Domain.Enums;

/// <summary>
/// A passenger elevator that moves one floor at a time.
/// </summary>
public class PassengerElevator : ElevatorBase
{
    private const int DefaultMaxCapacity = 10;
    private readonly int _travelDelayMilliseconds;

    public override int MaxCapacity => DefaultMaxCapacity;

    /// <summary>
    /// Creates a passenger elevator.
    /// </summary>
    /// <param name="id">The unique identifier for elevator.</param>
    /// <param name="travelDelayMilliseconds">delay between floor moves, for simulation pacing.</param>
    public PassengerElevator(int id, int travelDelayMilliseconds = 0) : base(id)
    {
        _travelDelayMilliseconds = travelDelayMilliseconds;
    }

    public override async Task MoveToFloor(int floor)
    {
        if (floor == CurrentFloor) { return; }

        State = ElevatorState.Moving;
        Direction = floor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

        while (CurrentFloor != floor)
        {
            CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
            if (_travelDelayMilliseconds > 0)
            {
                await Task.Delay(_travelDelayMilliseconds);
            }
        }

        Direction = ElevatorDirection.Stationary;
        State = ElevatorState.DoorsOpen;
    }
}
