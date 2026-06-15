namespace ElevatorSim.Tests;

using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Exceptions;
using ElevatorSim.Domain.Interfaces;

public class ElevatorControllerTests
{
    [Fact]
    public async Task RequestElevator_ThrowsWhenFloorIsBelowMinimum()
    {
        var controller = CreateController(minFloor: 1, maxFloor: 10);

        var exception = await Assert.ThrowsAsync<InvalidFloorException>(
            () => controller.RequestElevator(floor: 0, passengerCount: 1));

        Assert.Equal(0, exception.RequestedFloor);
    }

    [Fact]
    public async Task RequestElevator_ThrowsWhenFloorIsAboveMaximum()
    {
        var controller = CreateController(minFloor: 1, maxFloor: 10);

        var exception = await Assert.ThrowsAsync<InvalidFloorException>(
            () => controller.RequestElevator(floor: 11, passengerCount: 1));

        Assert.Equal(11, exception.RequestedFloor);
    }

    [Fact]
    public async Task RequestElevator_MovesSelectedElevatorToRequestedFloor()
    {
        var elevator = new PassengerElevator(id: 1);
        var controller = CreateController(elevator);

        await controller.RequestElevator(floor: 5, passengerCount: 2);

        Assert.Equal(5, elevator.CurrentFloor);
        Assert.Equal(2, elevator.PassengerCount);
    }

    [Fact]
    public async Task RequestElevator_BoardsPassengersAfterElevatorArrives()
    {
        var elevator = new PassengerElevator(id: 1);
        var controller = CreateController(elevator);

        await controller.RequestElevator(floor: 4, passengerCount: 3);

        Assert.Equal(3, elevator.PassengerCount);
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

        await controller.RequestElevator(floor: 6, passengerCount: 16);

        Assert.Equal(10, firstElevator.PassengerCount);
        Assert.Equal(6, secondElevator.PassengerCount);
        Assert.Equal(6, firstElevator.CurrentFloor);
        Assert.Equal(6, secondElevator.CurrentFloor);
    }


    private static ElevatorController CreateController(params IElevator[] elevators)
    {
        return new ElevatorController(
            elevators,
            new NearestElevator(),
            minFloor: 1,
            maxFloor: 10);
    }

    private static ElevatorController CreateController(int minFloor, int maxFloor)
    {
        var elevator = new PassengerElevator(id: 1);

        return new ElevatorController(
            [elevator],
            new NearestElevator(),
            minFloor,
            maxFloor);
    }
}