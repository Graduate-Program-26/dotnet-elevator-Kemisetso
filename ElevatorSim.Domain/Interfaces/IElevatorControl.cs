namespace ElevatorSim.Domain.Interfaces;

using ElevatorSim.Domain.Enums;

/// <summary>
/// Movement and availability operations for an elevator.
/// </summary>
public interface IElevatorControl
{
    int Id { get; }
    ElevatorDirection Direction { get; }
    ElevatorState State { get; }

    /// <summary>Gets whether the elevator can accept a new dispatch.</summary>
    bool IsAvailable { get; }

    /// <summary>Moves the elevator to the requested floor.</summary>
    Task MoveToFloor(int floor, ElevatorType tripMode = ElevatorType.Normal);
}