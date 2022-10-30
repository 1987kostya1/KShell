using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace KShell.Controls;

public partial class FileExplorer : UserControl
{
    public class FilesystemItem
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
    public class FilesystemFile:FilesystemItem
    {

    }
    public class FilesystemDirectory:FilesystemItem
    {
    }
    public ObservableCollection<ExplorerFile> FilesCollection { get; set; }

    public string CurrentFolder;
    public FileExplorer()
    {
        InitializeComponent();
        FilesCollection = new ObservableCollection<ExplorerFile>();
        CurrentFolder = $"C:\\Users{Environment.UserName}\\Desktop";
    }

    public void DisplayFolder(string folder)
    {
        CurrentFolder = folder;
        var files = Directory.GetFiles(CurrentFolder);
        var dirs = Directory.GetDirectories(CurrentFolder);
        FilesCollection.Clear();
        foreach (var file in files)
        {
            FilesCollection.Add(new ExplorerFile()
            {
                Filename = file
            });
        }

        foreach (var file in dirs)
        {
            FilesCollection.Add(new ExplorerFile()
            {
                Filename = file
            });
        }
        ListBoxFiles.ItemsSource = FilesCollection;
        foreach (var listBoxFile in ListBoxFiles.ItemsSource)
        {
            var obj = listBoxFile as ExplorerFile;
            obj.Redraw();
        }


    }
    
    private void ListBoxFiles_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var clicked = (sender as ListViewItem);
        if (clicked == null) return;
        var file = (clicked.Content) as ExplorerFile;
        if(file==null) return;
        if(Directory.Exists(file.Filename))
            DisplayFolder(file.Filename);
        if (File.Exists(file.Filename))
            Process.Start(file.Filename);


    }

    private void PrevFolderButton_OnClick(object sender, RoutedEventArgs e)
    {
        DisplayFolder(Path.GetDirectoryName(CurrentFolder));
    }
}