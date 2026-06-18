namespace ElevatorSim.Domain.Exceptions;

/// <summary>
/// Thrown when a requested floor is outside the building's valid range.
/// </summary>
public class InvalidFloorException : Exception
{
    /// <summary>Gets the floor that was requested.</summary>
    public int RequestedFloor { get; }

    /// <summary>
    /// Creates an exception for an out-of-range floor.
    /// </summary>
    public InvalidFloorException(int requestedFloor, int minFloor, int maxFloor)
        : base($"Floor {requestedFloor} is invalid. Valid range is {minFloor}–{maxFloor}.")
    {
        RequestedFloor = requestedFloor;
    }
}