using Invert.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Invert.Commands;

public class BaseCommand
{
    public virtual string Name { get; set; } = null!;
    public virtual List<string> Args { get; } = null!;
    public string? FullCommand { get; set; }

    protected Dictionary<string, string> parsedArgs { get; set; }
    public Dictionary<string, string> ParsedArgs => parsedArgs;
    protected virtual Regex? CommandRegex { get; }
    protected readonly ListLogUtil? ListLog;
    public bool GridViewAdd { get; set; } = true;
    public bool Deprecated { get; set; } = false;
    public string GridViewArgs
    {
        get
        {
            return string.Join(" ", ParsedArgs.Select(x => x.Value));
        }
        set
        {
            FullCommand = $"{Name} {value}";
            ParseArgs();
        }
    }

    public virtual void Fix()
    {
        if (FullCommand is null)
        {
            throw new NullReferenceException("FullCommand was null");
        }

        if (ParsedArgs is null)
        {
            throw new NullReferenceException("ParsedArgs was null");
        }

        foreach (var command in Constants.ReplaceableArgsRegexes)
        {
            if (command.Item1.Match(FullCommand).Success)
            {
                var result = command.Item1.Replace(FullCommand, command.Item2);
                ListLog!.Replaced(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogReplaced"], FullCommand, result));
                FullCommand = result;
                parsedArgs.Clear();
                ParseArgs();
                break;
            }
        }

        foreach (var command in Constants.ReplaceableCommands)
        {
            if (command.Item1 == Name)
            {
                ListLog!.Replaced(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogReplaced"], Name, command.Item2));
                Name = command.Item2;
                FullCommandFromArgs();
                break;
            }
        }

        foreach (var arg in ParsedArgs)
        {
            string newValue = arg.Value;

            foreach (var replaceArg in Constants.ReplaceableArgs)
            {
                if (arg.Value.Replace("\"", "").Replace(";", "").Trim() == replaceArg.Item1)
                {
                    ListLog!.Replaced(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogReplaced"], arg.Value, replaceArg.Item2));
                    newValue = $"\"{replaceArg.Item2}\"";
                    break;
                }
            }
            ParsedArgs[arg.Key] = newValue;
        }

        if (!Deprecated)
        {
            foreach (var deprecatedCommand in Constants.DeprecatedCommands)
            {
                if (Name == deprecatedCommand)
                {
                    ListLog!.Deprecated(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogDeprecatedCommand"], FullCommand));
                    GridViewAdd = false;
                    Deprecated = true;
                }
            }
            foreach (var deprecatedArg in Constants.DeprecatedArgs)
            {
                foreach (var arg in ParsedArgs)
                {
                    if (arg.Value.Replace(" ", "").Replace("\"", "").Contains(deprecatedArg))
                    {
                        ListLog!.Deprecated(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogDeprecated"], deprecatedArg, FullCommand));
                        GridViewAdd = false;
                        Deprecated = true;
                    }
                }
            }
            foreach (var deprecatedRegex in Constants.DeprecatedArgsRegexes)
            {
                if (deprecatedRegex.IsMatch(FullCommand))
                {
                    ListLog!.Deprecated(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogDeprecatedCommand"], FullCommand));
                    GridViewAdd = false;
                    Deprecated = true;
                }
            }
        }

        FullCommandFromArgs();
    }

    public void FullCommandFromArgs()
    {
        FullCommand = $"{Name} {String.Join(" ", ParsedArgs.Values)}";
    }


    public BaseCommand()
    {
        parsedArgs = new Dictionary<string, string>();
    }

    public BaseCommand(string fullCommand, ListLogUtil listLog)
    {
        FullCommand = fullCommand;
        ListLog = listLog;
        parsedArgs = new Dictionary<string, string>();
        ParseArgs();
    }

    public virtual bool IsCommand(string raw)
    {
        var splitted = raw.Split(' ');
        return splitted[0].ToLower().Trim() == Name.ToLower();
    }

    protected virtual void ParseArgs()
    {
        if (parsedArgs.Any())
        {
            return;
        }

        if (CommandRegex is null)
        {
            try
            {
                var splitted = FullCommand!.Split(" ", 2)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Trim().Replace(";", ""))
                    .ToList();

                for (var i = 0; i < splitted.Count - 1; i++)
                {
                    parsedArgs.Add(Args[i].ToLower(), splitted[i + 1]);
                }
            }
            catch
            {
            }
            return;
        }

        var reResult = CommandRegex!.Match(FullCommand!);
        if (reResult.Success)
        {
            for (var i = 1; i < reResult.Groups.Count; ++i)
            {
                parsedArgs.Add(Args[i - 1], reResult.Groups[i].Value);
            }
        }
    }


}

