using System;
using System.Windows;
using System.Windows.Controls;

namespace Invert.Utils;
public class ListLogUtil
{
    private readonly ListBox _listBox;
    public ListLogUtil(ref ListBox listBox)
    {
        _listBox = listBox;
    }

    public void Write(string message, DateTime dateTime, string title = "Info")
    {
        _listBox.Items.Add($"({dateTime}) [{title}] -> {message}");
    }

    public void Deprecated(string message, DateTime? dateTime = null)
    {
        if (!dateTime.HasValue)
        {
            dateTime = DateTime.Now;
        }
        Write(message, dateTime.Value, (string)Application.Current.Resources.MergedDictionaries[0]["LogDeprecatedTitle"]);
    }

    public void Replaced(string message, DateTime? dateTime = null)
    {
        if (!dateTime.HasValue)
        {
            dateTime = DateTime.Now;
        }
        Write(message, dateTime.Value, (string)Application.Current.Resources.MergedDictionaries[0]["LogReplacedTitle"]);
    }

    public void Duplicate(string message, DateTime? dateTime = null)
    {
        if (!dateTime.HasValue)
        {
            dateTime = DateTime.Now;
        }
        Write(message, dateTime.Value, (string)Application.Current.Resources.MergedDictionaries[0]["LogDuplicatedTitle"]);
    }
}

