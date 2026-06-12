namespace ElevatorSim.Domain.Interfaces;


public interface IDispatch
{
    IElevator? SelectElevator(IReadOnlyList<IElevator> elevators, int requestedFloor);
}