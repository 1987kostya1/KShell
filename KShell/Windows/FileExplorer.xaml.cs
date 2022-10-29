using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace KShell.Windows;

public class FilesystemFile
{
    public string FileThumbnail { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
}
public class FilesystemDirectory
{
    public string FileThumbnail { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
}

public partial class FileExplorer : Window
{
    public FileExplorer()
    {
        InitializeComponent();
        foreach (var VARIABLE in Directory.GetDirectories("C:\\"))
        {
            
        }
    }
    public ObservableCollection<FilesystemFile> FilesCollection { get; set; }

    private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}