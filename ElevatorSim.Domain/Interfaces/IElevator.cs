namespace ElevatorSim.Domain.Interfaces;

/// <summary>
/// Represents a single elevator in the building.
/// </summary>
public interface IElevator : IElevatorControl, IFloorEvents, IPassengerInteraction;
