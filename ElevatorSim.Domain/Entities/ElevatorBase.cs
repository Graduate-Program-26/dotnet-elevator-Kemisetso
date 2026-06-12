namespace ElevatorSim.Domain.Entities;

using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Exceptions;
using ElevatorSim.Domain.Interfaces;

public abstract class ElevatorBase : IElevator
{
    private int _passengerCount;
    public int Id { get; }
    public int CurrentFloor { get; set; }
    public ElevatorDirection Direction { get; set; } = ElevatorDirection.Stationary;

    public ElevatorState State { get; set; } = ElevatorState.Available;

    public int PassengerCount => _passengerCount;

    public abstract int MaxCapacity { get; }

    public bool IsAvailable => State == ElevatorState.Available || State == ElevatorState.DoorsOpen;

    public ElevatorBase(int id)
    {
        Id = id;
        CurrentFloor = 1;
    }

    public abstract Task MoveToFloor(int floor);

    public void BoardingPassengers(int count)
    {
        if (_passengerCount + count > MaxCapacity)
            throw new CapacityExceededException(MaxCapacity);

        _passengerCount += count;
    }

    public void ExitingPassengers(int count)
    {
        _passengerCount = Math.Max(0, _passengerCount - count);
    }
}