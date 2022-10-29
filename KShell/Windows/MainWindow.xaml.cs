using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KShell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool updateProcList;
        private Dictionary<Process, bool> procsToUpdate = new();
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            TimeStr.Text = DateTime.Now.ToString("h:mm:ss tt");
            foreach (UIElement element in ProcGrid.Children)
            {
                ((ProcessButton)element).UpdateText();
            }   
            
        }



        private Dictionary<int, ShowWindowCommands> cachedWindowStates = new Dictionary<int, ShowWindowCommands>();
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            foreach (var proc in Process.GetProcesses())
            {
                procsToUpdate.Add(proc,true);
                
                
            }
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Children, (sender, e) =>
            {
                var element = (AutomationElement)sender;
                var name = element.Current.Name;
                procsToUpdate.Add(Process.GetProcessById(element.Current.ProcessId),true);
                Application.Current.Dispatcher.Invoke(RefreshProcesses,element.Current.NativeWindowHandle);
                
                Automation.AddAutomationEventHandler(WindowPattern.WindowClosedEvent, element, TreeScope.Element, (s, e2) =>
                {
                    procsToUpdate.Add(Process.GetProcessById(element.Current.ProcessId),false);
                    Application.Current.Dispatcher.Invoke(RefreshProcesses,element.Current.NativeWindowHandle);

                });
                
            });
            
            RefreshProcesses(-1);
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
            
            
            
        }
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true, CharSet= CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);


        void RefreshProcesses(int hwnd)
        {
            if (true)
            {
                var arr = procsToUpdate.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    var process = arr[i].Key;
                    var add = arr[i].Value;

                    if (add)
                    {
                        if (process.MainWindowHandle == IntPtr.Zero) continue;
                        if (process.ProcessName == "ApplicationFrameHost") continue; //XXX remove this workaround
                        if (process.ProcessName == "SystemSettings") continue; //XXX remove this workaround
                        if (process.ProcessName == "TextInputHost") continue; //XXX remove this workaround

                        if (hwnd == 0) continue;
                        var button = new ProcessButton();

                        button.Process = process;
                        ProcGrid.Children.Add(button);
                    }
                    else
                    {
                        Console.WriteLine("HAS TO REMOVE WINDOW");

                        for (int j = 0; j < ProcGrid.Children.Count; j++)
                        {
                            var element = (ProcessButton)ProcGrid.Children[j];
                            if (element.Process.Id == process.Id)
                            {
                                j = 0;
                                ProcGrid.Children.Remove(element);
                            }
                        }
                    }
                    procsToUpdate.Remove(process);
                }
            }
        }

        [DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return placement;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowPlacement(
            IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public ShowWindowCommands showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public enum ShowWindowCommands : int
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
        }


        private void TasksButton_OnClick(object sender, RoutedEventArgs e)
        {
            var menu = App.startMenu;
            if (menu.IsVisible)
            {
                menu.Hide();
            }
            else
            {
                menu.Top =menu.Height-70;
                menu.Left = this.Left;
                menu.Show();
            }
        }
    }
}