namespace ElevatorSim.Tests;

using ElevatorSim.Domain.Entities;
using ElevatorSim.Domain.Enums;
using ElevatorSim.Domain.Exceptions;

public class PassengerElevatorPassengerTests
{
    [Fact]
    public void Constructor_SetsDefaultState()
    {
        var elevator = new PassengerElevator(id: 1);

        Assert.Equal(1, elevator.Id);
        Assert.Equal(1, elevator.CurrentFloor);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
        Assert.Equal(ElevatorState.Available, elevator.State);
        Assert.Equal(0, elevator.PassengerCount);
        Assert.Equal(10, elevator.MaxCapacity);
        Assert.True(elevator.IsAvailable);
    }

    [Fact]
    public void BoardingPassengers_IncreasesPassengerCount()
    {
        var elevator = new PassengerElevator(id: 1);

        elevator.BoardingPassengers(count: 3);

        Assert.Equal(3, elevator.PassengerCount);
    }

    [Fact]
    public void ExitingPassengers_DecreasesPassengerCount()
    {
        var elevator = new PassengerElevator(id: 1);
        elevator.BoardingPassengers(count: 7);

        elevator.ExitingPassengers(count: 4);

        Assert.Equal(3, elevator.PassengerCount);
    }

    [Fact]
    public void ExitingPassengers_DoesNotDropBelowZero()
    {
        var elevator = new PassengerElevator(id: 1);
        elevator.BoardingPassengers(count: 2);

        elevator.ExitingPassengers(count: 5);

        Assert.Equal(0, elevator.PassengerCount);
    }

    [Fact]
    public void BoardingPassengers_ThrowsWhenCapacityWouldBeExceeded()
    {
        var elevator = new PassengerElevator(id: 1);
        elevator.BoardingPassengers(elevator.MaxCapacity);

        var exception = Assert.Throws<CapacityExceededException>(
            () => elevator.BoardingPassengers(count: 1));

        Assert.Equal(elevator.MaxCapacity, exception.MaxCapacity);
    }
}