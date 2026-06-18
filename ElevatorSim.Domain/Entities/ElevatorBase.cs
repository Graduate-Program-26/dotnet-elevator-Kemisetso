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
    public bool IsAvailable => State == ElevatorState.Available || State == ElevatorState.DoorsOpen;

    /// <summary>
    /// Creates an elevator at floor 1.
    /// </summary>
    /// <param name="id">The unique identifier for this elevator.</param>
    public ElevatorBase(int id)
    {
        Id = id;
        CurrentFloor = 1;
    }

    public abstract Task MoveToFloor(int floor);

    /// <inheritdoc />
    /// <exception cref="CapacityExceededException">Thrown when boarding would exceed <see cref="MaxCapacity"/>.</exception>
    public void BoardingPassengers(int count)
    {
        if (_passengerCount + count > MaxCapacity)
        {
            throw new CapacityExceededException(MaxCapacity);
        }

        _passengerCount += count;
    }

    /// <inheritdoc />
    /// <exception cref="CapacityExceededException">Thrown when boarding would exceed <see cref="MaxCapacity"/>.</exception>
    public void BoardingPassengers(int count, int destinationFloor)
    {
        if (_passengerCount + count > MaxCapacity)
        {
            throw new CapacityExceededException(MaxCapacity);
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
