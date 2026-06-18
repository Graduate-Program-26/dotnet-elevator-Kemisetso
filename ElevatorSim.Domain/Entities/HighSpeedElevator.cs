namespace ElevatorSim.Domain.Entities;

using ElevatorSim.Domain.Enums;

/// <summary>
/// A high-speed elevator that skips intermediate floors during travel.
/// </summary>
public class HighSpeedElevator : ElevatorBase
{
    private const int DefaultMaxCapacity = 8;
    private const int TravelDelayMilliseconds = 100;

    public override int MaxCapacity => DefaultMaxCapacity;

    public HighSpeedElevator(int id) : base(id)
    {
    }

    public override async Task MoveToFloor(int floor, ElevatorType tripMode = ElevatorType.Normal)
    {
        if (floor == CurrentFloor)
        {
            return;
        }

        State = ElevatorState.Moving;
        Direction = floor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
        CurrentFloor = floor;
        await Task.Delay(TravelDelayMilliseconds);

        Direction = ElevatorDirection.Stationary;
        State = ElevatorState.DoorsOpen;
    }
}