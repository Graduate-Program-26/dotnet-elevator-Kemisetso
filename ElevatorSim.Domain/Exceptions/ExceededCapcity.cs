namespace ElevatorSim.Domain.Exceptions;

/// <summary>
/// Thrown when boarding would exceed the elevator's capacity.
/// </summary>
public class CapacityExceededException : Exception
{
    /// <summary>Gets the maximum capacity that was exceeded.</summary>
    public int MaxCapacity { get; }

    /// <summary>
    /// Creates an exception for a full elevator.
    /// </summary>
    public CapacityExceededException(int maxCapacity)
        : base($"Cannot board passengers — elevator is at maximum capacity of {maxCapacity}.")
    {
        MaxCapacity = maxCapacity;
    }
}
