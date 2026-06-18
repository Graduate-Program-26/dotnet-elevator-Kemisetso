namespace ElevatorSim.Cons.Display;

/// <summary>
/// Helpers for writing styled text to the console.
/// </summary>
public static class ConsoleUI
{
    /// <summary>
    /// Writes a line using the color associated with the given message kind.
    /// </summary>
    /// <param name="message">The text to write.</param>
    /// <param name="kind">The visual style to apply.</param>
    /// <param name="indent">Number of leading spaces.</param>
    public static void WriteLine(string message, MessageKind kind = MessageKind.Plain, int indent = 0)
    {
        WriteLine(message, GetColor(kind), indent);
    }

    /// <summary>
    /// Writes a line in the specified console color.
    /// </summary>
    public static void WriteLine(string message, ConsoleColor color, int indent = 0)
    {
        var padding = indent > 0 ? new string(' ', indent) : string.Empty;
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"{padding}{message}");
        Console.ForegroundColor = previousColor;
    }

    /// <summary>
    /// Returns console color for a message kind.
    /// </summary>
    /// <param name="kind">message style.</param>
    /// <returns> matching foreground color.</returns>
    public static ConsoleColor GetColor(MessageKind kind) => kind switch
    {
        MessageKind.Info => ConsoleColor.Cyan,
        MessageKind.Success => ConsoleColor.Green,
        MessageKind.Warning => ConsoleColor.Yellow,
        MessageKind.Error => ConsoleColor.Red,
        MessageKind.Menu => ConsoleColor.DarkGray,
        _ => ConsoleColor.Gray
    };

    /// <summary>
    /// Returns console color for an elevator row style.
    /// </summary>
    /// <param name="style">row style.</param>
    /// <returns>matching foreground color.</returns>
    public static ConsoleColor GetElevatorColor(ElevatorRowStyle style) => style switch
    {
        ElevatorRowStyle.Moving => ConsoleColor.Yellow,
        ElevatorRowStyle.DoorsOpen => ConsoleColor.Green,
        ElevatorRowStyle.Full => ConsoleColor.Red,
        _ => ConsoleColor.White
    };
}

public enum ElevatorRowStyle
{
    Available,
    Moving,
    DoorsOpen,
    Full
}