using System.Diagnostics;
using System.Windows.Controls;

namespace KShell;

public partial class ProcessButton : Button
{
    public Process Process;
    public ProcessButton()
    {
        InitializeComponent();
    }

    protected override void OnClick()
    {
        base.OnClick();
        var handle = Process.MainWindowHandle;
        var placement = MainWindow.GetPlacement(handle);
        if (placement.showCmd is MainWindow.ShowWindowCommands.Minimized or MainWindow.ShowWindowCommands.Hide)
        {
            MainWindow.ShowWindow(handle, (int)MainWindow.ShowWindowCommands.Normal);
        }
        else if (placement.showCmd is MainWindow.ShowWindowCommands.Normal or MainWindow.ShowWindowCommands.Maximized)
        {
            if (true)//MainWindow.GetForegroundWindow() == handle)
            {
                MainWindow.ShowWindow(handle, (int)MainWindow.ShowWindowCommands.Minimized);
            }
            else MainWindow.SetForegroundWindow(handle);

        }
    }

    public void UpdateText()
    {
        var title = Process.GetProcessById(Process.Id).MainWindowTitle;
        if (title != "")
        {
            MainText.Text = title;
        }
        else MainText.Text = Process.ProcessName;
    }
}