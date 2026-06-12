namespace ElevatorSim.Domain.Interfaces;

using ElevatorSim.Domain.Enums;

public interface IElevator
{
    int Id { get; }
    int CurrentFloor { get; }
    ElevatorDirection Direction { get; }
    ElevatorState State { get; }
    int PassengerCount { get; }
    int MaxCapacity { get; }
    bool IsAvailable { get; }
    Task MoveToFloor(int floor);
    void BoardingPassengers(int count);
    void ExitingPassengers(int count);
}