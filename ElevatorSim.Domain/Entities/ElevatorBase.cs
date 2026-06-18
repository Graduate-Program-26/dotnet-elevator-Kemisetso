namespace ElevatorSim.Domain.Entities;

using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Exceptions;
using ElevatorSim.Domain.Interfaces;

/// <summary>
/// Base implementation of <see cref="IElevator"/> with shared passenger tracking.
/// </summary>
public abstract class ElevatorBase : IElevator
{
    private readonly Dictionary<int, int> _passengersByDestination = new();
    private int _passengerCount;

    public int Id { get; }

    public int CurrentFloor { get; set; }

    public ElevatorDirection Direction { get; set; } = ElevatorDirection.Stationary;

    public ElevatorState State { get; set; } = ElevatorState.Available;

    public int PassengerCount => _passengerCount;

    public abstract int MaxCapacity { get; }

    /// <inheritdoc />
    public virtual int GetMaxCapacity(ElevatorType tripMode = ElevatorType.Normal) => MaxCapacity;

    public bool IsAvailable => State == ElevatorState.Available || State == ElevatorState.DoorsOpen;

    /// <summary>
    /// Creates an elevator at floor 1.
    /// </summary>
    public ElevatorBase(int id)
    {
        Id = id;
        CurrentFloor = 1;
    }

    public abstract Task MoveToFloor(int floor, ElevatorType tripMode = ElevatorType.Normal);

    public void BoardingPassengers(int count, ElevatorType tripMode = ElevatorType.Normal)
    {
        var maxCapacity = GetMaxCapacity(tripMode);
        if (_passengerCount + count > maxCapacity)
        {
            throw new CapacityExceededException(maxCapacity);
        }

        _passengerCount += count;
    }

    /// <inheritdoc />
    /// <exception cref="CapacityExceededException">Thrown when boarding would exceed effective capacity.</exception>
    public void BoardingPassengers(int count, int destinationFloor, ElevatorType tripMode = ElevatorType.Normal)
    {
        var maxCapacity = GetMaxCapacity(tripMode);
        if (_passengerCount + count > maxCapacity)
        {
            throw new CapacityExceededException(maxCapacity);
        }

        _passengerCount += count;
        _passengersByDestination[destinationFloor] =
            _passengersByDestination.GetValueOrDefault(destinationFloor) + count;
    }

    public int DisembarkAtCurrentFloor()
    {
        if (!_passengersByDestination.Remove(CurrentFloor, out var count))
        {
            return 0;
        }

        _passengerCount -= count;
        return count;
    }

    public void ExitingPassengers(int count)
    {
        _passengerCount = Math.Max(0, _passengerCount - count);
    }
}