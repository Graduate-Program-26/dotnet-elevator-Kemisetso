namespace ElevatorSim.Tests;

using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Exceptions;

public class HighSpeedElevatorTests
{
    [Fact]
    public void Constructor_SetsDefaultStateAndCapacity()
    {
        var elevator = new HighSpeedElevator(id: 1);

        Assert.Equal(1, elevator.Id);
        Assert.Equal(1, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.Available, elevator.State);
        Assert.Equal(0, elevator.PassengerCount);
        Assert.Equal(8, elevator.MaxCapacity);
        Assert.True(elevator.IsAvailable);
    }

    [Fact]
    public async Task MoveToFloor_SkipsIntermediateFloors()
    {
        var elevator = new HighSpeedElevator(id: 1);

        await elevator.MoveToFloor(floor: 15);

        Assert.Equal(15, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.DoorsOpen, elevator.State);
    }

    [Fact]
    public async Task MoveToFloor_WhenTargetIsBelowCurrentFloor_JumpsDirectlyAndOpensDoors()
    {
        var elevator = new HighSpeedElevator(id: 1)
        {
            CurrentFloor = 12
        };

        await elevator.MoveToFloor(floor: 3);

        Assert.Equal(3, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.DoorsOpen, elevator.State);
    }

    [Fact]
    public async Task MoveToFloor_WhenTargetIsCurrentFloor_DoesNotChangeState()
    {
        var elevator = new HighSpeedElevator(id: 1);

        await elevator.MoveToFloor(floor: elevator.CurrentFloor);

        Assert.Equal(1, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.Available, elevator.State);
    }

    [Fact]
    public void BoardingPassengers_ThrowsWhenCapacityWouldBeExceeded()
    {
        var elevator = new HighSpeedElevator(id: 1);
        elevator.BoardingPassengers(elevator.MaxCapacity);

        var exception = Assert.Throws<CapacityExceededException>(
            () => elevator.BoardingPassengers(count: 1));

        Assert.Equal(8, exception.MaxCapacity);
    }
}