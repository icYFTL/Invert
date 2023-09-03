using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using InvertApi.Commands;
using InvertApi.Models.logic;

namespace InvertApi.Controllers;

public class ConfigLogic
{
    #region SpecialData
    public class MessageBoxSettings
    {
        public string Message { get; init; } = null!;
        public string Title { get; init; } = null!;

    }
    public class LogicResult
    {
        public MessageBoxSettings? MessageBoxSettings { get; init; }
    }
    #endregion
    private readonly string _oldConfigFile;
    private readonly string _newConfigPath;
    public List<BaseCommand> ParsedCommands;

    public bool IsParsed => ParsedCommands.Any();
    private string _targetNamespace => "Invert.Commands";
    
    public ConfigLogic()
    {
    }

    public async Task<GenericLogicResult> ParseAsync(string raw)
    {
        var data = raw
            .ToLower()
            .Split('\n')
            .Where(x => !String.IsNullOrEmpty(x))
            .Select(x => x.Trim())
            .ToList();

        var assembly = Assembly.GetExecutingAssembly();

        var classesInNamespace = assembly.GetTypes()
            .Where(t => string.Equals(t.Namespace, _targetNamespace, StringComparison.Ordinal) &&
            !t.Name.StartsWith("Base") &&
            !t.Name.StartsWith("Generic") &&
            !t.Name.Contains(">") &&
            !t.Name.Contains("Constants"))
            .ToList();

        var foundCommands = new List<BaseCommand>();

        foreach (var command in data)
        {
            var found = false;
            foreach (var @class in classesInNamespace)
            {
                var instance = (BaseCommand)Activator.CreateInstance(@class)!;
                if (instance.IsCommand(command))
                {
                    instance = (BaseCommand)Activator.CreateInstance(@class, new { command })!;
                    found = true;
                    foundCommands.Add(instance);
                    break;
                }
            }

            if (!found)
            {
                var instance = new GenericCommand(command);
                foundCommands.Add(instance);
            }
        }

        return new SuccessLogicResult
        {
            Result = foundCommands
        };
    }

    private async Task _fixAliassesAsync(AliasCommand alias)
    {
        var aliasRegex = new Regex($"{alias.ParsedArgs["name"]};?", RegexOptions.Compiled);
        foreach (var command in ParsedCommands)
        {
            foreach (var arg in command.ParsedArgs)
            {
                if (aliasRegex.Match(arg.Value).Success)
                {
                    // if (true)
                    // {
                        command.ParsedArgs[arg.Key] = "";
                        command.FullCommandFromArgs();
                        // _listLog!.Replaced(String.Format((string)MediaTypeNames.Application.Current.Resources.MergedDictionaries[0]["LogAliasRemoved"], arg.Value, (string)MediaTypeNames.Application.Current.Resources.MergedDictionaries[0]["LogAliasRemovedCh1"]));
                    // }
                    // else
                    // {
                    //     command.Add = false;
                    //     // _listLog!.Deprecated(String.Format((string)MediaTypeNames.Application.Current.Resources.MergedDictionaries[0]["LogAliasRemoved"], arg.Value, (string)MediaTypeNames.Application.Current.Resources.MergedDictionaries[0]["LogAliasRemovedCh2"]));
                    // }
                    break;
                }
            }
        }
    }

    public async Task<GenericLogicResult> FixAsync(int level, List<BaseCommand> commands)
    {
        ParsedCommands = commands;
        // foreach (var x in ParsedCommands)
        // {
        //     x.Fix(level);
        //     if (level == 3)
        //     {
        //         if (x is AliasCommand)
        //         {
        //             await _fixAliassesAsync((AliasCommand)x);
        //         }
        //     }
        // }
        ParsedCommands.AsParallel().ForAll(async (x) =>
        {
            x.Fix(level);
            if (level == 3)
            {
                if (x is AliasCommand)
                {
                    await _fixAliassesAsync((AliasCommand)x);
                }
            }
        });

        return new SuccessLogicResult
        {
            Result = ParsedCommands
        };
    }

    public async Task<GenericLogicResult> RemoveDuplicatesAsync(List<BaseCommand> commands)
    {
        ParsedCommands = commands;
        var uniqueObjects = new Dictionary<string, BaseCommand>();
        
        foreach (var command in ParsedCommands)
        {
            if (uniqueObjects.ContainsKey(command.FullCommand!))
            {
                command.Add = false;
                // _listLog.Duplicate(command.FullCommand!);
            }
            else
            {
                uniqueObjects[command.FullCommand!] = command;
            }
        }

        return new SuccessLogicResult
        {
            Result = ParsedCommands
        };
    }

    // public async Task<GenericLogicResult> SaveAsync(List<BaseCommand> commands)
    // {
    //     ParsedCommands = commands;
    //     
    //     var version = Assembly.GetExecutingAssembly()
    // .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
    // .Select(x => x.InformationalVersion)
    // .FirstOrDefault();
    //
    //     var newPath = Path.Join(_newConfigPath, $"Invert_{Guid.NewGuid()}.cfg"); // XD to lazy for now
    //     File.WriteAllText(newPath, String.Join(";\n", ParsedCommands
    //         .Where(x => !x.Deprecated && x.Add)
    //         .Select(x => x.FullCommand)));
    //
    //     var sb = new StringBuilder();
    //     sb.AppendLine();
    //     sb.AppendLine($"echo \"Converted from CS:GO by Invert v{version}\"");
    //     sb.AppendLine($"echo \"In case of problems write issue at https://github.com/icYFTL/Invert\"");
    //     sb.AppendLine($"echo \"bw, icY\"");
    //
    //     File.AppendAllText(newPath, sb.ToString());
    //
    //     return newPath;
    // }

}

