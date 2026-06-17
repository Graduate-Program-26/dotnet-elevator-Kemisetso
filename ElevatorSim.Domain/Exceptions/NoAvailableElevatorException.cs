namespace ElevatorSim.Domain.Exceptions;

public class NoAvailableElevatorException : Exception
{
    public int UnassignedPassengerCount { get; }

    public NoAvailableElevatorException(int unassignedPassengerCount)
        : base($"No elevator available — {unassignedPassengerCount} passenger(s) could not be assigned.")
    {
        UnassignedPassengerCount = unassignedPassengerCount;
    }
}