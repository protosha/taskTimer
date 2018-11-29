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
        private string _hintText;
        private TimeSpan _spanLeft;

        public MainWindow(string hintText, TimeSpan spanLeft)
        {
            InitializeComponent();
            _hintText = hintText;
            _spanLeft = spanLeft;
            DataContext = this;
            CurrentViewModel = new HintViewModel(new HintModel() {Text = _hintText});
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
            e.Cancel = true;
        }

        void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (CurrentViewModel is TimerViewModel) {
                return;
            }

            long currentTicks = Environment.TickCount * 10000L;
            long endTicks = currentTicks + _spanLeft.Ticks;
            var timerViewModel = new TimerViewModel(new TimerModel() {
                CurrentTicks = currentTicks,
                EndTicks = endTicks
            });
            CurrentViewModel = timerViewModel;

            var timer = new DispatcherTimer();
            var warnTimer = new DispatcherTimer();
            int warnSecondsThreshold = 5;
            Action beep = Console.Beep;
            warnTimer.Tick += (timerSender, timerEventArgs) => {
                beep.BeginInvoke((a) => { beep.EndInvoke(a); }, null);
            };
            warnTimer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (timerSender, timerEventArgs) => {
                currentTicks = Environment.TickCount * 10000L;
                if (new TimeSpan(timerViewModel.Model.EndTicks - currentTicks).Seconds <= warnSecondsThreshold) {
                    warnTimer.Start();
                }
                if (timerViewModel.Model.EndTicks - currentTicks <= 0) {
                    timerViewModel.CurrentTicks = timerViewModel.Model.EndTicks;
                    timer.Stop();
                    warnTimer.Stop();
                    return;
                }
                timerViewModel.CurrentTicks = currentTicks;
            };
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();


        }
    }
}
