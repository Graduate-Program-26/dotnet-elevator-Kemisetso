namespace ElevatorSim.Tests;

using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Enums;

public class PassengerElevatorMovementTests
{
    [Fact]
    public async Task MoveToFloor_WhenTargetIsAboveCurrentFloor_MovesUpAndOpensDoors()
    {
        var elevator = new PassengerElevator(id: 1);

        await elevator.MoveToFloor(floor: 2);

        Assert.Equal(2, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.DoorsOpen, elevator.State);
        Assert.True(elevator.IsAvailable);
    }

    [Fact]
    public async Task MoveToFloor_WhenTargetIsBelowCurrentFloor_MovesDownAndOpensDoors()
    {
        var elevator = new PassengerElevator(id: 1)
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
        var elevator = new PassengerElevator(id: 1);

        await elevator.MoveToFloor(floor: elevator.CurrentFloor);

        Assert.Equal(1, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.Available, elevator.State);
    }
}