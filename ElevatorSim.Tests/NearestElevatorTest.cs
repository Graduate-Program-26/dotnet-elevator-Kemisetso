namespace ElevatorSim.Tests;

using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Interfaces;

public class NearestElevatorTest
{
    [Fact]
    public void SelectElevator_ReturnsNearestAvailableElevator()
    {
        var strategy = new NearestElevator();
        var elevators = new List<IElevator>
        {
            new PassengerElevator(id: 1) { CurrentFloor = 1 },
            new PassengerElevator(id: 2) { CurrentFloor = 5 },
            new PassengerElevator(id: 3) { CurrentFloor = 9 }
        };

        var selectedElevator = strategy.SelectElevator(elevators, requestedFloor: 6);

        Assert.NotNull(selectedElevator);
        Assert.Equal(2, selectedElevator.Id);
    }

}