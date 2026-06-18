namespace ElevatorSim.Domain.Entities;

using ElevatorSim.Domain.Enums;

/// <summary>
/// A freight elevator with higher capacity and slower travel.
/// </summary>
public class FreightElevator : ElevatorBase
{
    private const int DefaultMaxCapacity = 25;
    private const int TravelDelayMilliseconds = 500;

    public override int MaxCapacity => DefaultMaxCapacity;

    /// <summary>
    /// Creates a freight elevator.
    /// </summary>
    public FreightElevator(int id) : base(id)
    {
    }

    public override async Task MoveToFloor(int floor)
    {
        if (floor == CurrentFloor)
        {
            return;
        }

        State = ElevatorState.Moving;
        Direction = floor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

        while (CurrentFloor != floor)
        {
            CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
            await Task.Delay(TravelDelayMilliseconds);
        }

        Direction = ElevatorDirection.Stationary;
        State = ElevatorState.DoorsOpen;
    }
}
