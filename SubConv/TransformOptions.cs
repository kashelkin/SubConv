namespace SubConv;

public class TransformOptions
{
    public string Name { get; }
    public IReadOnlyList<string> Params { get; }

    public TransformOptions(string option)
    {
        var split = option.Split(':', 2);
        Name = split[0];
        Params = split.Length == 2 
            ? split[1].Split(';')
            : Array.Empty<string>();
    }
}