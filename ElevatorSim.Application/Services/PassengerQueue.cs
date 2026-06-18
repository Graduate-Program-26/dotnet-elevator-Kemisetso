namespace ElevatorSim.Application.Services;

using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Interfaces;

/// <summary>
/// Tracks remaining passengers and splits them across elevator capacity.
/// </summary>
public class PassengerQueue
{
    private int _remaining;

    /// <summary>
    /// Creates a queue for the given passenger count.
    /// </summary>
    public PassengerQueue(int passengerCount)
    {
        _remaining = passengerCount;
    }

    /// <summary>Gets whether passengers are still waiting to board.</summary>
    public bool HasPending => _remaining > 0;

    /// <summary>Gets the number of passengers not yet assigned to an elevator.</summary>
    public int Remaining => _remaining;

    /// <summary>
    /// Removes the next boarding batch based on elevator spare capacity.
    /// </summary>
    /// <returns>The number of passengers assigned to this trip.</returns>
    public int DequeueForElevator(IPassengerInteraction elevator, ElevatorType tripMode = ElevatorType.Normal)
    {
        var maxCapacity = elevator.GetMaxCapacity(tripMode);
        var batchSize = Math.Min(_remaining, maxCapacity - elevator.PassengerCount);
        _remaining -= batchSize;
        return batchSize;
    }
}