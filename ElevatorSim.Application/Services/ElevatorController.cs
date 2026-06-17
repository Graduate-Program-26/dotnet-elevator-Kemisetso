namespace ElevatorSim.Application.Services;

using ElevatorSim.Application.Interfaces;
using ElevatorSim.Domain.Exceptions;
using ElevatorSim.Domain.Interfaces;

public class ElevatorController : IElevatorController
{
    private readonly IDispatch _dispatch;
    private readonly List<IElevator> _elevators;
    private readonly int _minFloor;
    private readonly int _maxFloor;

    public IReadOnlyList<IElevator> Elevators => _elevators.AsReadOnly();

    public ElevatorController(IEnumerable<IElevator> elevators, IDispatch dispatch, int minFloor, int maxFloor)
    {
        _elevators = elevators.ToList();
        _dispatch = dispatch;
        _minFloor = minFloor;
        _maxFloor = maxFloor;
    }

    public async Task RequestElevator(int pickupFloor, int destinationFloor, int passengerCount)
    {
        ValidateFloor(pickupFloor, nameof(pickupFloor));
        ValidateFloor(destinationFloor, nameof(destinationFloor));

        if (pickupFloor == destinationFloor)
        {
            throw new ArgumentException(
                "Pickup and destination floors must be different.",
                nameof(destinationFloor));
        }

        if (passengerCount <= 0)
        {
            throw new ArgumentException(
                "Passenger count must be positive.",
                nameof(passengerCount));
        }

        var remaining = passengerCount;

        while (remaining > 0)
        {
            var elevator = _dispatch.SelectElevator(_elevators, pickupFloor);
            if (elevator is null)
            {
                throw new NoAvailableElevatorException(remaining);
            }

            var canEnter = Math.Min(remaining, elevator.MaxCapacity - elevator.PassengerCount);

            await elevator.MoveToFloor(pickupFloor);
            elevator.DisembarkAtCurrentFloor();
            elevator.BoardingPassengers(canEnter, destinationFloor);
            remaining -= canEnter;

            await elevator.MoveToFloor(destinationFloor);
            elevator.DisembarkAtCurrentFloor();
        }
    }

    private void ValidateFloor(int floor, string paramName)
    {
        if (floor < _minFloor || floor > _maxFloor)
        {
            throw new InvalidFloorException(floor, _minFloor, _maxFloor);
        }
    }
}