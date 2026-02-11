using SentraLock.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SentraLock.ViewModels
{
    class MenuViewModel : ViewModelBase
    {
        public ICommand ShowEncryptViewCommand { get; }
        public ICommand ShowDecryptViewCommand { get; }

        public MenuViewModel()
        {
            ShowEncryptViewCommand = new ViewModelCommand(ExecuteShowEncryptViewCommand);
            ShowDecryptViewCommand = new ViewModelCommand(ExecuteShowDecryptViewCommand);
        }

        private void ExecuteShowDecryptViewCommand(object parameter)
        {
            if (App.Current.MainWindow.DataContext is MainViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CurrentChildView = new DecryptViewModel();
            }
        }

        private void ExecuteShowEncryptViewCommand(object parameter)
        {
            if(App.Current.MainWindow.DataContext is MainViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CurrentChildView = new EncryptViewModel();
            }
        }
    }
}
