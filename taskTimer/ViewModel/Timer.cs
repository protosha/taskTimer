using System;
using TimerModel = taskTimer.Model.Timer;

namespace taskTimer.ViewModel
{
    public class Timer : Base
    {
        public Timer(TimerModel model)
        {
            this.Model = model;
        }

        public TimerModel Model { get; set; }

        public long CurrentTicks
        {
            get { return Model.CurrentTicks; }
            set {
                Model.CurrentTicks = value;
                this.OnPropertyChanged("CurrentTicks");
                this.OnPropertyChanged("TimeLeft");
            }
        }

        public string TimeLeft
        {
            get {
                TimeSpan timeRemaining = new TimeSpan(Model.EndTicks - Model.CurrentTicks);
                return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", timeRemaining.Hours, timeRemaining.Minutes,
                    timeRemaining.Seconds, timeRemaining.Milliseconds / 10);
            }
        }
    }
}