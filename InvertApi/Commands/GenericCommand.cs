namespace InvertApi.Commands;

class GenericCommand : BaseCommand
{
    private string _name;
    public override string Name
    {
        get
        {
            if (string.IsNullOrEmpty(_name))
                _name = FullCommand?.Split(' ')[0] ?? "generic";

            return _name;
        }
        set
        {
            _name = value;
        }
    }

    protected override List<string> Args => new List<string>();
    public override bool IsCommand(string raw)
    {
        return true;
    }

    protected override void ParseArgs()
    {
        var result = new Dictionary<string, string>();
        var splitted = FullCommand!.Split(" ", 2)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Trim().Replace(";", ""))
            .ToList();

        for (var i = 1; i < splitted.Count; i++)
        {
            result.Add($"arg_{i - 1}", splitted[i]);
        }

        parsedArgs = result;
    }

    public GenericCommand(string fullCommand) : base(fullCommand) { }
}

