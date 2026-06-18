namespace ElevatorSim.Domain.Enums;

/// <summary>
/// Travel mode for a single elevator request. Does not designate fleet roles.
/// </summary>
public enum ElevatorType
{
    Normal,
    Freight,
    HighSpeed
}