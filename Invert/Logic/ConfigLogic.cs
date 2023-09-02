using Invert.Commands;
using Invert.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CheckBox = System.Windows.Controls.CheckBox;
using ListBox = System.Windows.Controls.ListBox;
using Application = System.Windows.Application;
using System.Text;

namespace Invert.Logic;

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
    private readonly CacheStorage _cacheStorage;
    private readonly ListLogUtil _listLog;
    public List<BaseCommand> ParsedCommands;
    private readonly CheckBox _aliasRemoveChb;

    public bool IsParsed => ParsedCommands.Any();
    private string _targetNamespace => "Invert.Commands";
    public ConfigLogic(ref ListBox listBox, ref CheckBox aliasRemoveChb)
    {
        _cacheStorage = new CacheStorage();
        _listLog = new ListLogUtil(ref listBox);

        _oldConfigFile = _cacheStorage.Get("oldConfigPath")!;
        _newConfigPath = _cacheStorage.Get("newConfigPath")!;
        _aliasRemoveChb = aliasRemoveChb;
    }

    public void Parse()
    {
        var data = File
            .ReadAllText(_oldConfigFile)
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
                    instance = (BaseCommand)Activator.CreateInstance(@class, new object[] { command, _listLog })!;
                    found = true;
                    foundCommands.Add(instance);
                    break;
                }
            }

            if (!found)
            {
                var instance = new GenericCommand(command, _listLog);
                foundCommands.Add(instance);
            }
        }

        ParsedCommands = foundCommands;
    }

    private void _fixAliasses(AliasCommand alias)
    {
        var aliasRegex = new Regex($"{alias.ParsedArgs["name"]};?", RegexOptions.Compiled);
        foreach (var command in ParsedCommands)
        {
            foreach (var arg in command.ParsedArgs)
            {
                if (aliasRegex.Match(arg.Value).Success)
                {
                    if (_aliasRemoveChb.IsChecked!.Value)
                    {
                        command.ParsedArgs[arg.Key] = "";
                        command.FullCommandFromArgs();
                        _listLog!.Replaced(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogAliasRemoved"], arg.Value, (string)Application.Current.Resources.MergedDictionaries[0]["LogAliasRemovedCh1"]));
                    }
                    else
                    {
                        command.GridViewAdd = false;
                        _listLog!.Deprecated(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["LogAliasRemoved"], arg.Value, (string)Application.Current.Resources.MergedDictionaries[0]["LogAliasRemovedCh2"]));
                    }
                    break;
                }
            }
        }
    }

    public void Fix()
    {
        if (!IsParsed)
        {
            return;
        }
        foreach (var command in ParsedCommands)
        {
            command.Fix();
            if (command is AliasCommand)
            {
                _fixAliasses((AliasCommand)command);
            }
        }
    }

    public void RemoveDuplicates()
    {
        Dictionary<string, BaseCommand> uniqueObjects = new Dictionary<string, BaseCommand>();

        foreach (var command in ParsedCommands)
        {
            if (uniqueObjects.ContainsKey(command.FullCommand!))
            {
                command.GridViewAdd = false;
                _listLog.Duplicate(command.FullCommand!);
            }
            else
            {
                uniqueObjects[command.FullCommand!] = command;
            }
        }
    }

    public string Save()
    {
        var version = Assembly.GetExecutingAssembly()
    .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
    .Select(x => x.InformationalVersion)
    .FirstOrDefault();

        var newPath = Path.Join(_newConfigPath, $"Invert_{Guid.NewGuid()}.cfg"); // XD to lazy for now
        File.WriteAllText(newPath, String.Join(";\n", ParsedCommands
            .Where(x => !x.Deprecated && x.GridViewAdd)
            .Select(x => x.FullCommand)));

        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine($"echo \"Converted from CS:GO by Invert v{version}\"");
        sb.AppendLine($"echo \"In case of problems write issue at https://github.com/icYFTL/Invert\"");
        sb.AppendLine($"echo \"bw, icY\"");

        File.AppendAllText(newPath, sb.ToString());

        return newPath;
    }

}

