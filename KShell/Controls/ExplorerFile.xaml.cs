using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace KShell.Controls;

public partial class ExplorerFile : UserControl
{
    public string Filename;
    public ExplorerFile()
    {
        InitializeComponent();
        
    }

    public void Redraw()
    {
        FilenameText.Text = Path.GetFileName(Filename);
        IconObject.Source = IconManager.FindIconForFilename(Filename,false);
    }

    
}