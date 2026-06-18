namespace ElevatorSim.Domain.Interfaces;

/// <summary>
/// Chooses which elevator should respond to a floor request.
/// </summary>
public interface IDispatchStrategy
{
    /// <summary>
    /// Picks the best elevator for a pickup at the given floor.
    /// </summary>
    /// <param name="elevators">The elevators available to dispatch.</param>
    /// <param name="requestedFloor">The floor where passengers are waiting.</param>
    /// <returns>The selected elevator, or null if none are suitable.</returns>
    IElevator? SelectElevator(IReadOnlyList<IElevator> elevators, int requestedFloor);
}
