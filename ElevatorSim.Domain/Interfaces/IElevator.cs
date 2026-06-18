namespace ElevatorSim.Domain.Interfaces;

using ElevatorSim.Domain.Enums;

/// <summary>
/// Represents a single elevator in the building.
/// </summary>
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

    void BoardingPassengers(int count, int destinationFloor);

    int DisembarkAtCurrentFloor();

    void ExitingPassengers(int count);
}
