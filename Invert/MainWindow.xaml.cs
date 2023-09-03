using Invert.Utils;
using System;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using TextBox = System.Windows.Controls.TextBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Globalization;
using System.Linq;
using System.IO;
using MessageBox = System.Windows.MessageBox;
using Invert.Logic;
using System.Reflection;

namespace Invert;

public partial class MainWindow : Window
{
    private readonly CacheStorage _cacheStorage;
    private ConfigLogic? _configLogic;
    public MainWindow()
    {
        _cacheStorage = new CacheStorage();
        InitializeComponent();
        var version = Assembly.GetExecutingAssembly()
    .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
    .Select(x => x.InformationalVersion)
    .FirstOrDefault();

        VersionLbl.Content = $"Version: {version}";
    }

    private void OldConfigTbx_LostFocus(object sender, RoutedEventArgs e)
    {
        var tbx = (TextBox)sender;
        if (!File.Exists(tbx.Text))
        {
            tbx.Text = String.Empty;
            _configLogic = null;
            UndefinedCommandsGv.ItemsSource = null;
            LogLb.Items.Clear();
            return;
        }

        if (!_cacheStorage.Exists("oldConfigPath"))
            _cacheStorage.Add("oldConfigPath", tbx.Text);
        else
            _cacheStorage.Update("oldConfigPath", tbx.Text);

        _configLogic = null;
        UndefinedCommandsGv.ItemsSource = null;
        LogLb.Items.Clear();
        Parse();
    }


    private void NewConfigTbx_LostFocus(object sender, RoutedEventArgs e)
    {
        var tbx = (TextBox)sender;
        if (!Directory.Exists(tbx.Text))
        {
            tbx.Text = String.Empty;
            _configLogic = null;
            UndefinedCommandsGv.ItemsSource = null;
            LogLb.Items.Clear();
            return;
        }

        if (!_cacheStorage.Exists("newConfigPath"))
            _cacheStorage.Add("newConfigPath", tbx.Text);
        else
            _cacheStorage.Update("newConfigPath", tbx.Text);

        _configLogic = null;
        UndefinedCommandsGv.ItemsSource = null;
        LogLb.Items.Clear();
        Parse();
    }

    private void OldConfigLoadBtn_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog();

        openFileDialog.Title = (string)Application.Current.Resources.MergedDictionaries[0]["OpenFileDialogTitle"];
        openFileDialog.Filter = (string)Application.Current.Resources.MergedDictionaries[0]["OpenFileDialogFilter"];
        openFileDialog.Multiselect = false;
        openFileDialog.CheckFileExists = true;

        var result = openFileDialog.ShowDialog();

        if (result is true)
        {
            OldConfigTbx.Text = openFileDialog.FileName;
            if (!_cacheStorage.Exists("oldConfigPath"))
                _cacheStorage.Add("oldConfigPath", openFileDialog.FileName);
            else
                _cacheStorage.Update("oldConfigPath", openFileDialog.FileName);
            Parse();
        }
    }

    private void NewConfigLoadBtn_Click(object sender, RoutedEventArgs e)
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = (string)Application.Current.Resources.MergedDictionaries[0]["OpenDirectoryDialogTitle"];
        folderBrowserDialog.ShowDialog();

        if (!String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
        {
            NewConfigTbx.Text = folderBrowserDialog.SelectedPath;
            if (!_cacheStorage.Exists("newConfigPath"))
                _cacheStorage.Add("newConfigPath", folderBrowserDialog.SelectedPath);
            else
                _cacheStorage.Update("newConfigPath", folderBrowserDialog.SelectedPath);

            Parse();
        }
    }

    private void ChangeLanguageClick(string newLanguage)
    {
        var language = App.Languages.FirstOrDefault(x => x.Name == newLanguage);
        if (language is null)
        {
            throw new ArgumentException("Not supported language");
        }

        App.Language = language;
    }

    private void MI_SetRussianLanguage(object sender, RoutedEventArgs e)
    {
        ChangeLanguageClick("ru-RU");
    }

    private void MI_SetEnglishLanguage(object sender, RoutedEventArgs e)
    {
        ChangeLanguageClick("en-US");
    }

    private void LanguageChanged(Object sender, EventArgs e)
    {
        CultureInfo currLang = App.Language;

    }

    private void Parse()
    {
        if (!_cacheStorage.Exists("oldConfigPath"))
        {
            // MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorOldPathNotSelectedBody"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorPathNotSelectedTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!_cacheStorage.Exists("newConfigPath"))
        {
            //MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorNewPathNotSelectedBody"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorPathNotSelectedTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _configLogic = new ConfigLogic(ref LogLb, ref AliasRemoveChb);
        _configLogic.Parse();

        UndefinedCommandsGv.ItemsSource = _configLogic.ParsedCommands;

        // Tabs.SelectedIndex = 1;
    }

    private void FixBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_configLogic is null)
        {
            MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorRunParseFirst"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorGenericTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (!_configLogic.IsParsed)
        {
            MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorRunParseFirst"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorGenericTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _configLogic.Fix();
        UndefinedCommandsGv.ItemsSource = null;
        UndefinedCommandsGv.ItemsSource = _configLogic.ParsedCommands;
    }

    private void RemoveDuplicatesBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_configLogic is null)
        {
            MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorRunParseFirst"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorGenericTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (!_configLogic.IsParsed)
        {
            MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorRunParseFirst"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorGenericTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _configLogic.RemoveDuplicates();
        UndefinedCommandsGv.ItemsSource = null;
        UndefinedCommandsGv.ItemsSource = _configLogic.ParsedCommands;
    }

    private void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_configLogic is null)
        {
            MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorRunParseFirst"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorGenericTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (!_configLogic.IsParsed)
        {
            MessageBox.Show((string)Application.Current.Resources.MergedDictionaries[0]["ErrorRunParseFirst"], (string)Application.Current.Resources.MergedDictionaries[0]["ErrorGenericTitle"], MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var resultPath = _configLogic!.Save();
        MessageBox.Show(String.Format((string)Application.Current.Resources.MergedDictionaries[0]["FileSaved"], resultPath), (string)Application.Current.Resources.MergedDictionaries[0]["SuccessGenericTitle"], MessageBoxButton.OK, MessageBoxImage.Information);
    }

}
