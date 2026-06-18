namespace ElevatorSim.Tests;

using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Exceptions;

public class FloorManagerTests
{
    private readonly FloorManager _floorManager = new(minFloor: 1, maxFloor: 20);

    [Theory]
    [InlineData(1)]
    [InlineData(20)]
    public void ValidateFloor_AcceptsFloorWithinRange(int floor)
    {
        var exception = Record.Exception(() => _floorManager.ValidateFloor(floor, nameof(floor)));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateFloor_ThrowsWhenBelowMinimum()
    {
        var exception = Assert.Throws<InvalidFloorException>(
            () => _floorManager.ValidateFloor(floor: 0, paramName: nameof(floor)));

        Assert.Equal(0, exception.RequestedFloor);
    }

    [Fact]
    public void ValidateFloor_ThrowsWhenAboveMaximum()
    {
        var exception = Assert.Throws<InvalidFloorException>(
            () => _floorManager.ValidateFloor(floor: 21, paramName: nameof(floor)));

        Assert.Equal(21, exception.RequestedFloor);
    }

    [Fact]
    public void ValidateRequest_AcceptsValidRequest()
    {
        var exception = Record.Exception(
            () => _floorManager.ValidateRequest(pickupFloor: 3, destinationFloor: 12, passengerCount: 4));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateRequest_ThrowsWhenPickupEqualsDestination()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => _floorManager.ValidateRequest(pickupFloor: 5, destinationFloor: 5, passengerCount: 2));

        Assert.Equal(nameof(destinationFloor), exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidateRequest_ThrowsWhenPassengerCountIsNotPositive(int passengerCount)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => _floorManager.ValidateRequest(pickupFloor: 1, destinationFloor: 5, passengerCount));

        Assert.Equal(nameof(passengerCount), exception.ParamName);
    }

    [Fact]
    public void ValidateRequest_ThrowsWhenPickupFloorIsInvalid()
    {
        Assert.Throws<InvalidFloorException>(
            () => _floorManager.ValidateRequest(pickupFloor: 0, destinationFloor: 5, passengerCount: 1));
    }

    [Fact]
    public void ValidateRequest_ThrowsWhenDestinationFloorIsInvalid()
    {
        Assert.Throws<InvalidFloorException>(
            () => _floorManager.ValidateRequest(pickupFloor: 1, destinationFloor: 25, passengerCount: 1));
    }
}
