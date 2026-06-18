namespace ElevatorSim.Cons.Display;

/// <summary>
/// The visual style for a console message.
/// </summary>
public enum MessageKind
{
    Plain,
    Info,
    Success,
    Warning,
    Error,
    Menu
}

/// <summary>
/// A line of text shown in the console footer, with an optional style.
/// </summary>
/// <param name="Text">The message to display.</param>
/// <param name="Kind">How the message should be styled.</param>
public readonly record struct FooterLine(string Text, MessageKind Kind = MessageKind.Plain)
{
    public static FooterLine Plain(string text) => new(text, MessageKind.Plain);

    public static FooterLine Info(string text) => new(text, MessageKind.Info);

    public static FooterLine Success(string text) => new(text, MessageKind.Success);

    public static FooterLine Error(string text) => new(text, MessageKind.Error);

    public static FooterLine Warning(string text) => new(text, MessageKind.Warning);

    public static FooterLine Menu(string text) => new(text, MessageKind.Menu);
}