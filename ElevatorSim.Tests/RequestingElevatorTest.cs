namespace ElevatorSim.Tests;

using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Exceptions;
using ElevatorSim.Domain.Interfaces;

public class ElevatorControllerTests
{
    [Fact]
    public async Task RequestElevator_ThrowsWhenFloorIsBelowMinimum()
    {
        var controller = CreateController(minFloor: 1, maxFloor: 10);

        var exception = await Assert.ThrowsAsync<InvalidFloorException>(
            () => controller.RequestElevator(pickupFloor: 0, destinationFloor: 5, passengerCount: 1));

        Assert.Equal(0, exception.RequestedFloor);
    }

    [Fact]
    public async Task RequestElevator_ThrowsWhenFloorIsAboveMaximum()
    {
        var controller = CreateController(minFloor: 1, maxFloor: 10);

        var exception = await Assert.ThrowsAsync<InvalidFloorException>(
            () => controller.RequestElevator(pickupFloor: 11, destinationFloor: 5, passengerCount: 1));

        Assert.Equal(11, exception.RequestedFloor);
    }

    [Fact]
    public async Task RequestElevator_ThrowsWhenPassengerCountIsNotPositive()
    {
        var controller = CreateController(new PassengerElevator(id: 1));

        await Assert.ThrowsAsync<ArgumentException>(
            () => controller.RequestElevator(pickupFloor: 1, destinationFloor: 5, passengerCount: 0));
    }

    [Fact]
    public async Task RequestElevator_ThrowsWhenPickupAndDestinationAreTheSame()
    {
        var controller = CreateController(new PassengerElevator(id: 1));

        await Assert.ThrowsAsync<ArgumentException>(
            () => controller.RequestElevator(pickupFloor: 5, destinationFloor: 5, passengerCount: 2));
    }

    [Fact]
    public async Task RequestElevator_ThrowsWhenNoElevatorIsAvailable()
    {
        var firstElevator = new PassengerElevator(id: 1);
        firstElevator.BoardingPassengers(firstElevator.MaxCapacity);
        var secondElevator = new PassengerElevator(id: 2);
        secondElevator.BoardingPassengers(secondElevator.MaxCapacity);
        var controller = CreateController(firstElevator, secondElevator);

        var exception = await Assert.ThrowsAsync<NoAvailableElevatorException>(
            () => controller.RequestElevator(pickupFloor: 5, destinationFloor: 1, passengerCount: 1));

        Assert.Equal(1, exception.UnassignedPassengerCount);
    }

    [Fact]
    public async Task RequestElevator_MovesSelectedElevatorToDestinationAndDisembarks()
    {
        var elevator = new PassengerElevator(id: 1);
        var controller = CreateController(elevator);

        await controller.RequestElevator(pickupFloor: 1, destinationFloor: 5, passengerCount: 2);

        Assert.Equal(5, elevator.CurrentFloor);
        Assert.Equal(0, elevator.PassengerCount);
    }

    [Fact]
    public async Task RequestElevator_DisbarksPassengersAtDestination()
    {
        var elevator = new PassengerElevator(id: 1);
        var controller = CreateController(elevator);

        await controller.RequestElevator(pickupFloor: 1, destinationFloor: 4, passengerCount: 3);

        Assert.Equal(4, elevator.CurrentFloor);
        Assert.Equal(0, elevator.PassengerCount);
    }

    [Fact]
    public async Task RequestElevator_DispatchesAdditionalElevatorWhenPassengerCountExceedsCapacity()
    {
        var firstElevator = new PassengerElevator(id: 1)
        {
            CurrentFloor = 5
        };
        var secondElevator = new PassengerElevator(id: 2)
        {
            CurrentFloor = 1
        };
        var controller = CreateController(firstElevator, secondElevator);

        await controller.RequestElevator(pickupFloor: 6, destinationFloor: 1, passengerCount: 16);

        Assert.Equal(0, firstElevator.PassengerCount);
        Assert.Equal(0, secondElevator.PassengerCount);
        Assert.Equal(1, firstElevator.CurrentFloor);
        Assert.Equal(1, secondElevator.CurrentFloor);
    }

    [Fact]
    public async Task RequestElevator_HighSpeedTripMode_JumpsToDestination()
    {
        var elevator = new PassengerElevator(id: 1);
        var controller = CreateController(elevator);

        await controller.RequestElevator(
            pickupFloor: 1,
            destinationFloor: 10,
            passengerCount: 1,
            tripMode: ElevatorType.HighSpeed);

        Assert.Equal(10, elevator.CurrentFloor);
    }

    private static ElevatorController CreateController(params IElevator[] elevators)
    {
        return new ElevatorController(
            elevators,
            new NearestElevatorStrategy(),
            new FloorManager(minFloor: 1, maxFloor: 10));
    }

    private static ElevatorController CreateController(int minFloor, int maxFloor)
    {
        var elevator = new PassengerElevator(id: 1);

        return new ElevatorController(
            [elevator],
            new NearestElevatorStrategy(),
            new FloorManager(minFloor, maxFloor));
    }
}