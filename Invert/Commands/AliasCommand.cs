using Invert.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Invert.Commands;

class AliasCommand : BaseCommand
{
    public override string Name => "alias";
    public override List<string> Args => new List<string>() { "name", "action" };
    protected override Regex CommandRegex => new Regex(@"^alias\s+(""[^""]+""|\S+)\s+(""[^""]+"");?$", RegexOptions.Compiled);

    public AliasCommand() : base()
    {

    }
    public AliasCommand(string fullCommand, ListLogUtil listLog) : base(fullCommand, listLog) { }

    public override void Fix()
    {
        base.Fix();
    }
}

