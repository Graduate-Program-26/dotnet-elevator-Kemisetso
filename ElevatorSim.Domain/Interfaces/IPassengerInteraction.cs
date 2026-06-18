namespace ElevatorSim.Domain.Interfaces;

using ElevatorSim.Domain.Enums;

/// <summary>
/// Passenger boarding and capacity operations for an elevator.
/// </summary>
public interface IPassengerInteraction
{
    int PassengerCount { get; }

    int MaxCapacity { get; }

    /// <summary>Gets the effective capacity for the given trip mode.</summary>
    int GetMaxCapacity(ElevatorType tripMode = ElevatorType.Normal);

    void BoardingPassengers(int count, ElevatorType tripMode = ElevatorType.Normal);

    void BoardingPassengers(int count, int destinationFloor, ElevatorType tripMode = ElevatorType.Normal);

    /// <summary>Disembarks passengers whose destination is the current floor.</summary>
    /// <returns>The number of passengers who left.</returns>
    int DisembarkAtCurrentFloor();

    void ExitingPassengers(int count);
}