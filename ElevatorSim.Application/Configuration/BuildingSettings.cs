namespace ElevatorSim.Application.Configuration;

/// <summary>
/// Configuration for the simulated building.
/// </summary>
public sealed class BuildingSettings
{
    public const int DefaultMinFloor = 1;

    public const int DefaultMaxFloor = 20;

    public const int DefaultElevatorCount = 3;

    public const int DefaultTravelDelayMilliseconds = 3000;

    public int MinFloor { get; init; } = DefaultMinFloor;

    public int MaxFloor { get; init; } = DefaultMaxFloor;

    public int ElevatorCount { get; init; } = DefaultElevatorCount;

    public int TravelDelayMilliseconds { get; init; } = DefaultTravelDelayMilliseconds;
}