using Invert.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Invert.Commands;

class BindCommand : BaseCommand
{
    public override string Name => "bind";
    public override List<string> Args => new List<string>() { "key", "action" };
    protected override Regex CommandRegex => new Regex(@"^bind\s+(""[^""]+""|\S+)\s+(""[^""]+"");?$", RegexOptions.Compiled);

    public BindCommand() : base()
    {

    }
    public BindCommand(string fullCommand, ListLogUtil listLog) : base(fullCommand, listLog) { }

    public override void Fix()
    {
        base.Fix();
    }
}

