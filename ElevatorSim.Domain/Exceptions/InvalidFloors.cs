namespace ElevatorSim.Domain.Exceptions;

public class InvalidFloorException : Exception
{
    public int RequestedFloor { get; }

    public InvalidFloorException(int requestedFloor, int minFloor, int maxFloor)
        : base($"Floor {requestedFloor} is invalid. Valid range is {minFloor}–{maxFloor}.")
    {
        RequestedFloor = requestedFloor;
    }
}