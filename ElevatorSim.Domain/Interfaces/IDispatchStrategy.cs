namespace ElevatorSim.Domain.Interfaces;

/// <summary>
/// Chooses which elevator should respond to a floor request.
/// </summary>
public interface IDispatchStrategy
{
    /// <summary>
    /// Picks the best elevator for a pickup at the given floor.
    /// </summary>
    /// <returns>The selected elevator, or null if none are suitable.</returns>
    IElevator? SelectElevator(IReadOnlyList<IElevator> elevators, int requestedFloor);
}