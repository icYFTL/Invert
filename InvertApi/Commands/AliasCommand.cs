using System.Text.RegularExpressions;
using InvertApi.Models.logic;

namespace InvertApi.Commands;

class AliasCommand : BaseCommand
{
    public override string Name => "alias";
    protected override List<string> Args => new List<string>() { "name", "action" };
    protected override Regex CommandRegex => new Regex(@"^alias\s+(""[^""]+""|\S+)\s+(""[^""]+"");?$", RegexOptions.Compiled);

    public AliasCommand() : base()
    {

    }
    public AliasCommand(string fullCommand) : base(fullCommand) { }
}

