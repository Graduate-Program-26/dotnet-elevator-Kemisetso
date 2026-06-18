namespace ElevatorSim.Tests;

using ElevatorSim.Application.Services;
using ElevatorSim.Domain.Entities;

public class PassengerQueueTests
{
    [Fact]
    public void DequeueForElevator_AssignsUpToElevatorSpareCapacity()
    {
        var queue = new PassengerQueue(passengerCount: 7);
        var elevator = new PassengerElevator(id: 1);

        var batchSize = queue.DequeueForElevator(elevator);

        Assert.Equal(7, batchSize);
        Assert.Equal(0, queue.Remaining);
        Assert.False(queue.HasPending);
    }

    [Fact]
    public void DequeueForElevator_RespectsPartiallyFullElevator()
    {
        var queue = new PassengerQueue(passengerCount: 8);
        var elevator = new PassengerElevator(id: 1);
        elevator.BoardingPassengers(count: 6);

        var batchSize = queue.DequeueForElevator(elevator);

        Assert.Equal(4, batchSize);
        Assert.Equal(4, queue.Remaining);
        Assert.True(queue.HasPending);
    }

    [Fact]
    public void DequeueForElevator_DrainsOverflowAcrossMultipleBatches()
    {
        var queue = new PassengerQueue(passengerCount: 25);
        var firstElevator = new PassengerElevator(id: 1);
        var secondElevator = new PassengerElevator(id: 2);

        var firstBatch = queue.DequeueForElevator(firstElevator);
        var secondBatch = queue.DequeueForElevator(secondElevator);
        var thirdBatch = queue.DequeueForElevator(firstElevator);

        Assert.Equal(10, firstBatch);
        Assert.Equal(10, secondBatch);
        Assert.Equal(5, thirdBatch);
        Assert.Equal(0, queue.Remaining);
        Assert.False(queue.HasPending);
    }

    [Fact]
    public void DequeueForElevator_ReturnsZeroWhenElevatorIsFull()
    {
        var queue = new PassengerQueue(passengerCount: 5);
        var elevator = new PassengerElevator(id: 1);
        elevator.BoardingPassengers(elevator.MaxCapacity);

        var batchSize = queue.DequeueForElevator(elevator);

        Assert.Equal(0, batchSize);
        Assert.Equal(5, queue.Remaining);
        Assert.True(queue.HasPending);
    }
}