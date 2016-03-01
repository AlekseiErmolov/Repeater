using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Repeater.ViewModel
{
    class ViewModelBase : INotifyPropertyChanged
    {
        //basic ViewModelBase
        internal void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //Extra Stuff, shows why a base ViewModel is useful
        private bool? _closeWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _closeWindowFlag; }
            set
            {
                _closeWindowFlag = value;
                NotifyPropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
            }));
        }
    }
}
