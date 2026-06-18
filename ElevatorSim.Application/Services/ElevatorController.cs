namespace ElevatorSim.Application.Services;

using ElevatorSim.Application.Interfaces;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Exceptions;
using ElevatorSim.Domain.Interfaces;

/// <summary>
/// Dispatches elevator requests and coordinates pickup-to-dropoff journeys.
/// </summary>
public class ElevatorController : IElevatorController
{
    private readonly IDispatchStrategy _dispatch;
    private readonly List<IElevator> _elevators;
    private readonly FloorManager _floorManager;

    /// <inheritdoc />
    public IReadOnlyList<IElevator> Elevators => _elevators.AsReadOnly();

    /// <summary>
    /// Creates a controller for the given elevators and floor validation.
    /// </summary>
    /// <param name="elevators">The elevator fleet to manage.</param>
    /// <param name="dispatch">The strategy used to select an elevator.</param>
    /// <param name="floorManager">Validates floor numbers for requests.</param>
    public ElevatorController(
        IEnumerable<IElevator> elevators,
        IDispatchStrategy dispatch,
        FloorManager floorManager)
    {
        _elevators = elevators.ToList();
        _dispatch = dispatch;
        _floorManager = floorManager;
    }

    /// <inheritdoc />
    public async Task RequestElevator(
        int pickupFloor,
        int destinationFloor,
        int passengerCount,
        ElevatorType tripMode = ElevatorType.Normal)
    {
        _floorManager.ValidateRequest(pickupFloor, destinationFloor, passengerCount);

        var queue = new PassengerQueue(passengerCount);

        while (queue.HasPending)
        {
            var elevator = _dispatch.SelectElevator(_elevators, pickupFloor);
            if (elevator is null)
            {
                throw new NoAvailableElevatorException(queue.Remaining);
            }

            var canEnter = queue.DequeueForElevator(elevator);

            await elevator.MoveToFloor(pickupFloor, tripMode);
            elevator.DisembarkAtCurrentFloor();
            elevator.BoardingPassengers(canEnter, destinationFloor);

            await elevator.MoveToFloor(destinationFloor, tripMode);
            elevator.DisembarkAtCurrentFloor();
        }
    }
}