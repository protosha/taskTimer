using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BaseViewModel = taskTimer.ViewModel.Base;
using HintViewModel = taskTimer.ViewModel.Hint;
using HintModel = taskTimer.Model.Hint;
using TimerViewModel = taskTimer.ViewModel.Timer;
using TimerModel = taskTimer.Model.Timer;

namespace taskTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            CurrentViewModel = new HintViewModel(new HintModel() {Text = "This is hint!"});
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return this._currentViewModel; }
            set {
                _currentViewModel = value;
                this.RaisePropertyChanged("CurrentViewModel");
            }
        }

        void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            //e.Cancel = true;
        }

        void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (CurrentViewModel is TimerViewModel) {
                return;
            }

            long currentTicks = Environment.TickCount * 10000L;
            long endTicks = currentTicks + new TimeSpan(0, 0, 5).Ticks;
            var timerViewModel = new TimerViewModel(new TimerModel() {
                CurrentTicks = currentTicks,
                EndTicks = endTicks
            });
            CurrentViewModel = timerViewModel;

            var timer = new DispatcherTimer();
            timer.Tick += (timerSender, timerEventArgs) => {
                timerViewModel.CurrentTicks = Environment.TickCount * 10000L;
                if (timerViewModel.Model.EndTicks - timerViewModel.CurrentTicks <= 0) {
                    timer.Stop();
                }
            };
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }
    }
}
