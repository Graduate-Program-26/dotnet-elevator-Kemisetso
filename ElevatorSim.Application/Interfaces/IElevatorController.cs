namespace ElevatorSim.Application.Interfaces;

using ElevatorSim.Domain.Interfaces;

public interface IElevatorController
{
    IReadOnlyList<IElevator> Elevators { get; }
    Task RequestElevator(int floor, int passengerCount);
}