namespace ElevatorSim.Domain.Exceptions;

public class CapacityExceededException : Exception
{
    public int MaxCapacity { get; }

    public CapacityExceededException(int maxCapacity)
        : base($"Cannot board passengers — elevator is at maximum capacity of {maxCapacity}.")
    {
        MaxCapacity = maxCapacity;
    }
}