namespace ElevatorSim.Tests;

using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Exceptions;

public class FreightElevatorTests
{
    [Fact]
    public void Constructor_SetsDefaultStateAndCapacity()
    {
        var elevator = new FreightElevator(id: 1);

        Assert.Equal(1, elevator.Id);
        Assert.Equal(1, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.Available, elevator.State);
        Assert.Equal(0, elevator.PassengerCount);
        Assert.Equal(25, elevator.MaxCapacity);
        Assert.True(elevator.IsAvailable);
    }

    [Fact]
    public async Task MoveToFloor_WhenTargetIsAboveCurrentFloor_MovesUpAndOpensDoors()
    {
        var elevator = new FreightElevator(id: 1);

        await elevator.MoveToFloor(floor: 2);

        Assert.Equal(2, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.DoorsOpen, elevator.State);
        Assert.True(elevator.IsAvailable);
    }

    [Fact]
    public async Task MoveToFloor_WhenTargetIsBelowCurrentFloor_MovesDownAndOpensDoors()
    {
        var elevator = new FreightElevator(id: 1)
        {
            CurrentFloor = 3
        };

        await elevator.MoveToFloor(floor: 2);

        Assert.Equal(2, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.DoorsOpen, elevator.State);
        Assert.True(elevator.IsAvailable);
    }

    [Fact]
    public async Task MoveToFloor_WhenTargetIsCurrentFloor_DoesNotChangeState()
    {
        var elevator = new FreightElevator(id: 1);

        await elevator.MoveToFloor(floor: elevator.CurrentFloor);

        Assert.Equal(1, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.Available, elevator.State);
    }

    [Fact]
    public void BoardingPassengers_ThrowsWhenCapacityWouldBeExceeded()
    {
        var elevator = new FreightElevator(id: 1);
        elevator.BoardingPassengers(elevator.MaxCapacity);

        var exception = Assert.Throws<CapacityExceededException>(
            () => elevator.BoardingPassengers(count: 1));

        Assert.Equal(25, exception.MaxCapacity);
    }
}