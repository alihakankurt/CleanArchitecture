namespace Core.Application;

public sealed class Unit
{
    public static Unit Value { get; }
    public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(Value)!;

    static Unit()
    {
        Value = new Unit();
    }

    private Unit()
    {
    }
}
