using System;
using HintModel = taskTimer.Model.Hint;

namespace taskTimer.ViewModel
{
    public class Hint : Base
    {
        public Hint(HintModel model)
        {
            this.Model = model;
        }

        public HintModel Model { get; set; }

        public string HintText
        {
            get { return Model.Text; }
            set {
                Model.Text = value;
                this.OnPropertyChanged("HintText");
            }
        }
    }
}