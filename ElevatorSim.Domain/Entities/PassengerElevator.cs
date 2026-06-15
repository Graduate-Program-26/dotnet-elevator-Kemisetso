namespace ElevatorSim.Domain.Entities;

using ElevatorSim.Domain.Enums;

public class PassengerElevator : ElevatorBase
{
    private const int defaultMaxCap = 10;
    private const int TravelDelay = 500;

    public override int MaxCapacity => defaultMaxCap;

    public PassengerElevator(int id) : base(id) { }

    public override async Task MoveToFloor(int floor)
    {
        if (floor == CurrentFloor) { return; }

        State = ElevatorState.Moving;
        Direction = floor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

        while (CurrentFloor != floor)
        {
            CurrentFloor += Direction == ElevatorDirection.Up ? 1 : -1;
            // await Task.Delay(TravelDelay);
        }

        Direction = ElevatorDirection.Stationary;
        State = ElevatorState.DoorsOpen;


    }
}