using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KShell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static StartMenu startMenu;
        public App()
        {
            startMenu = new StartMenu();
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                var window = new MainWindow();
                
                window.Show();
                window.WindowState = WindowState.Normal;
                window.Left = screen.WorkingArea.Left;
                window.Top = screen.WorkingArea.Top;
                window.Width = screen.WorkingArea.Width;
                window.Height = screen.WorkingArea.Height;
                window.WindowState = WindowState.Maximized;
            }

        }
        
    }
}