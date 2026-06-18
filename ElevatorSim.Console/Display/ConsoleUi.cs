namespace ElevatorSim.Cons.Display;

public static class ConsoleUI
{
    public static void WriteLine(string message, MessageKind kind = MessageKind.Plain, int indent = 0)
    {
        WriteLine(message, GetColor(kind), indent);
    }

    public static void WriteLine(string message, ConsoleColor color, int indent = 0)
    {
        var padding = indent > 0 ? new string(' ', indent) : string.Empty;
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"{padding}{message}");
        Console.ForegroundColor = previousColor;
    }

    public static ConsoleColor GetColor(MessageKind kind) => kind switch
    {
        MessageKind.Info => ConsoleColor.Cyan,
        MessageKind.Success => ConsoleColor.Green,
        MessageKind.Warning => ConsoleColor.Yellow,
        MessageKind.Error => ConsoleColor.Red,
        MessageKind.Menu => ConsoleColor.DarkGray,
        _ => ConsoleColor.Gray
    };

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