namespace ElevatorSim.Domain.Interfaces;

/// <summary>
/// Passenger boarding and capacity operations for an elevator.
/// </summary>
public interface IPassengerInteraction
{
    int PassengerCount { get; }

    int MaxCapacity { get; }
    void BoardingPassengers(int count);

    void BoardingPassengers(int count, int destinationFloor);

    /// <summary>Disembarks passengers whose destination is the current floor.</summary>
    /// <returns>The number of passengers who left.</returns>
    int DisembarkAtCurrentFloor();

    void ExitingPassengers(int count);
}
