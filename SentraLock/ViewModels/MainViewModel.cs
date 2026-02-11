using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SentraLock.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }

            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public ICommand ShowMenuViewCommand { get; }
        public ICommand CloseApplicationCommand { get; }

        public MainViewModel()
        {
            ShowMenuViewCommand = new ViewModelCommand(ExecuteShowMenuViewCommand);
            CloseApplicationCommand = new ViewModelCommand(ExecuteCloseApplicationCommand);
            ExecuteShowMenuViewCommand(null);
        }

        private void ExecuteCloseApplicationCommand(object obj)
        {
            Application.Current.Shutdown();
        }

        private void ExecuteShowMenuViewCommand(object obj)
        {
            CurrentChildView = new MenuViewModel();
        }
    }
}
