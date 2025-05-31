namespace cw11;

public class ConflictEX : Exception

{
    public ConflictEX()
    {
    }

    public ConflictEX(string? message) : base(message)
    {
    }

    public ConflictEX(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}