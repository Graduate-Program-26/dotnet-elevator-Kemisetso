namespace ElevatorSim.Application.Services;

using ElevatorSim.Domain.Interfaces;

public class NearestElevator : IDispatch
{
    public IElevator? SelectElevator(IReadOnlyList<IElevator> elevators, int requestedFloor)
    {
        return elevators
            .Where(ele => ele.IsAvailable && ele.PassengerCount < ele.MaxCapacity)
            .OrderBy(ele => Math.Abs(ele.CurrentFloor - requestedFloor))
            .FirstOrDefault();
    }
}