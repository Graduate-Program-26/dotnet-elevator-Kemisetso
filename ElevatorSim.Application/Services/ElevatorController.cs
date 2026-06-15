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

    public async Task RequestElevator(int floor, int passengerCount)
    {
        if (floor < _minFloor || floor > _maxFloor)
        {
            throw new InvalidFloorException(floor, _minFloor, _maxFloor);

            var remaining = passengerCount;

            while (remaining > 0)
            {
                var elevator = _dispatch.SelectElevator(_elevators, floor);
                if (elevator is null)
                {
                    break;
                }

                var canEnter = Math.Min(remaining, elevator.MaxCapacity - elevator.PassengerCount);
                await elevator.MoveToFloor(floor);
                elevator.BoardingPassengers(canEnter);
                remaining -= canEnter;
            }
        }
    }
}