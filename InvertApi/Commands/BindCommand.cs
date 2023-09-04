using System.Text.RegularExpressions;

namespace InvertApi.Commands;

class BindCommand : BaseCommand
{
    public override string Name => "bind";
    protected override List<string> Args => new List<string>() { "key", "action" };
    protected override Regex CommandRegex => new Regex(@"^bind\s+(""[^""]+""|\S+)\s+(""[^""]+"");?$", RegexOptions.Compiled);
    public BindCommand() : base()
    {

    }
    public BindCommand(string fullCommand) : base(fullCommand) { }
}

