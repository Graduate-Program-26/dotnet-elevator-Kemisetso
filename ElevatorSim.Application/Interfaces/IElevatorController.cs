namespace ElevatorSim.Application.Interfaces;

using ElevatorSim.Domain.Interfaces;

/// <summary>
/// Coordinates elevator requests across the building fleet.
/// </summary>
public interface IElevatorController
{
    /// <summary>Gets the elevators managed by this controller.</summary>
    IReadOnlyList<IElevator> Elevators { get; }

    /// <summary>
    /// Dispatches an elevator to pick up passengers and take them to their destination.
    /// </summary>
    Task RequestElevator(int pickupFloor, int destinationFloor, int passengerCount);
}