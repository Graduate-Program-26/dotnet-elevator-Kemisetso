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
    public void BoardingPassengers_ThrowsWhenCapacityWouldBeExceeded()
    {
        var elevator = new PassengerElevator(id: 1);
        elevator.BoardingPassengers(elevator.MaxCapacity);

        var exception = Assert.Throws<CapacityExceededException>(
            () => elevator.BoardingPassengers(count: 1));

        Assert.Equal(elevator.MaxCapacity, exception.MaxCapacity);
    }

    [Fact]
    public void GetMaxCapacity_Returns25ForFreightTripMode()
    {
        var elevator = new PassengerElevator(id: 1);

        Assert.Equal(10, elevator.GetMaxCapacity());
        Assert.Equal(10, elevator.GetMaxCapacity(ElevatorType.Normal));
        Assert.Equal(10, elevator.GetMaxCapacity(ElevatorType.HighSpeed));
        Assert.Equal(25, elevator.GetMaxCapacity(ElevatorType.Freight));
    }

    [Fact]
    public void BoardingPassengers_FreightTripMode_AllowsUpTo25Passengers()
    {
        var elevator = new PassengerElevator(id: 1);

        elevator.BoardingPassengers(count: 23, tripMode: ElevatorType.Freight);

        Assert.Equal(23, elevator.PassengerCount);
    }

    [Fact]
    public void DisembarkAtCurrentFloor_RemovesPassengersWithMatchingDestination()
    {
        var elevator = new PassengerElevator(id: 1)
        {
            CurrentFloor = 5
        };
        elevator.BoardingPassengers(count: 3, destinationFloor: 5);

        var exited = elevator.DisembarkAtCurrentFloor();

        Assert.Equal(3, exited);
        Assert.Equal(0, elevator.PassengerCount);
    }
}