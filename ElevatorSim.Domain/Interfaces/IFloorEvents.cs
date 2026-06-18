namespace ElevatorSim.Domain.Interfaces;

/// <summary>
/// Floor position information for display and dispatch clients.
/// </summary>
public interface IFloorEvents
{
    int CurrentFloor { get; }
}