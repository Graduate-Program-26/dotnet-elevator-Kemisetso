namespace ElevatorSim.Tests;

using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Interfaces;

public class NearestElevatorStrategyTests
{
    [Fact]
    public void SelectElevator_ReturnsNearestAvailableElevator()
    {
        var strategy = new NearestElevatorStrategy();
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

    [Fact]
    public void SelectElevator_SkipsMovingElevator()
    {
        var strategy = new NearestElevatorStrategy();
        var elevators = new List<IElevator>
        {
            new PassengerElevator(id: 1)
            {
                CurrentFloor = 5,
                State = ElevatorState.Moving
            },
            new PassengerElevator(id: 2) { CurrentFloor = 2 }
        };

        var selectedElevator = strategy.SelectElevator(elevators, requestedFloor: 5);

        Assert.NotNull(selectedElevator);
        Assert.Equal(2, selectedElevator.Id);
    }
    [Fact]
    public void SelectElevator_SkipsFullElevator()
    {
        var strategy = new NearestElevatorStrategy();
        var fullElevator = new PassengerElevator(id: 1)
        {
            CurrentFloor = 5
        };
        fullElevator.BoardingPassengers(fullElevator.MaxCapacity);

        var elevators = new List<IElevator>
        {
            fullElevator,
            new PassengerElevator(id: 2) { CurrentFloor = 3 }
        };

        var selectedElevator = strategy.SelectElevator(elevators, requestedFloor: 5);

        Assert.NotNull(selectedElevator);
        Assert.Equal(2, selectedElevator.Id);
    }

    [Fact]
    public void SelectElevator_ReturnsNullWhenNoElevatorIsAvailable()
    {
        var strategy = new NearestElevatorStrategy();
        var fullElevator = new PassengerElevator(id: 1);
        fullElevator.BoardingPassengers(fullElevator.MaxCapacity);

        var elevators = new List<IElevator>
        {
            new PassengerElevator(id: 2) { State = ElevatorState.Moving },
            fullElevator
        };

        var selectedElevator = strategy.SelectElevator(elevators, requestedFloor: 4);

        Assert.Null(selectedElevator);
    }
}