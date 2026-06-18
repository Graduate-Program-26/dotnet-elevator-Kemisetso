namespace ElevatorSim.Domain.Enums;

/// <summary>
/// The current operational state of an elevator.
/// </summary>
public enum ElevatorState
{
    Available,

    Moving,
    DoorsOpen
}