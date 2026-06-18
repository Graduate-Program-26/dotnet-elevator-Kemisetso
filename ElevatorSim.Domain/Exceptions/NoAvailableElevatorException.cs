namespace ElevatorSim.Domain.Exceptions;

/// <summary>
/// Thrown when no elevator can take the remaining passengers.
/// </summary>
public class NoAvailableElevatorException : Exception
{
    /// <summary>Gets the number of passengers that could not be assigned.</summary>
    public int UnassignedPassengerCount { get; }

    /// <summary>
    /// Creates an exception for passengers left without an elevator.
    /// </summary>
    /// <param name="unassignedPassengerCount">How many passengers still need a ride.</param>
    public NoAvailableElevatorException(int unassignedPassengerCount)
        : base($"No elevator available — {unassignedPassengerCount} passenger(s) could not be assigned.")
    {
        UnassignedPassengerCount = unassignedPassengerCount;
    }
}
