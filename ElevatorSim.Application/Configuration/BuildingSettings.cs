namespace ElevatorSim.Application.Configuration;

public sealed class BuildingSettings
{
    public const int DefaultMinFloor = 1;
    public const int DefaultMaxFloor = 20;
    public const int DefaultElevatorCount = 3;

    public int MinFloor { get; init; } = DefaultMinFloor;
    public int MaxFloor { get; init; } = DefaultMaxFloor;
    public int ElevatorCount { get; init; } = DefaultElevatorCount;
}