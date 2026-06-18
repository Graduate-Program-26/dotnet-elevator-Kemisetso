namespace ElevatorSim.Application.Services;

using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Interfaces;

/// <summary>
/// Picks the nearest available elevator with spare capacity.
/// </summary>
public class NearestElevatorStrategy : IDispatchStrategy
{
    /// <inheritdoc />
    public IElevator? SelectElevator(
        IReadOnlyList<IElevator> elevators,
        int requestedFloor,
        ElevatorType tripMode = ElevatorType.Normal)
    {
        return elevators
            .Where(ele => ele.IsAvailable && ele.PassengerCount < ele.GetMaxCapacity(tripMode))
            .OrderBy(ele => Math.Abs(ele.CurrentFloor - requestedFloor))
            .FirstOrDefault();
    }
}