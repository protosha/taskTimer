using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace taskTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length < 2) {
                this.Shutdown(42);
                return;
            }

            string hintText = e.Args[0];
            string timeLeft = e.Args[1];
            TimeSpan spanLeft;
            bool isParsed = TimeSpan.TryParse(timeLeft, out spanLeft);

            if (!isParsed) {
                this.Shutdown(42);
                return;
            }

            MainWindow appWindow = new MainWindow(hintText, spanLeft);
            appWindow.Show();
        }
    }
}
