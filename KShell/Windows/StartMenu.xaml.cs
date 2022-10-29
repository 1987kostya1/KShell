using System.Diagnostics;
using System.Windows;
using KShell.Windows;

namespace KShell;

public partial class StartMenu : Window
{
    public StartMenu()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Process.Start(ProcToExecute.Text);
    }


    private void RegeditBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Process.Start("regedit.exe");
    }

    private void PaintBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Process.Start("mspaint.exe");
    }

    private void KillShellBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }

    private void ExplorerBtn_OnClick(object sender, RoutedEventArgs e)
    {
        new FileExplorer().Show();
    }
}