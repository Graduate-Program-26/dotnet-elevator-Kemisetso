namespace ElevatorSim.Application.Services;

using ElevatorSim.Domain.Exceptions;

/// <summary>
/// Validates floor numbers against the building's configured range.
/// </summary>
public class FloorManager
{
    private readonly int _minFloor;
    private readonly int _maxFloor;

    public FloorManager(int minFloor, int maxFloor)
    {
        _minFloor = minFloor;
        _maxFloor = maxFloor;
    }

    /// <summary>
    /// Validates a pickup request before dispatch.
    /// </summary>
    public void ValidateRequest(int pickupFloor, int destinationFloor, int passengerCount)
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
    }

    /// <summary>
    /// Throws when a floor is outside the valid range.
    /// </summary>
    public void ValidateFloor(int floor, string paramName)
    {
        if (floor < _minFloor || floor > _maxFloor)
        {
            throw new InvalidFloorException(floor, _minFloor, _maxFloor);
        }
    }
}