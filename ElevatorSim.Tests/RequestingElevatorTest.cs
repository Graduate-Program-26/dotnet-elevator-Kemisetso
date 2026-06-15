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