using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SentraLock.ViewModels
{
    class EncryptViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand ShowMenuViewModelCommand { get; }
        public ICommand BrowseFileCommand { get; }
        public ICommand EncryptFileCommand { get; }

        private string _fileName = "Select a file...";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if(_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }

        private string _messageLabelText;
        public string MessageLabelText
        {
            get { return _messageLabelText; }
            set
            {
                if(_messageLabelText != value)
                {
                    _messageLabelText = value;
                    OnPropertyChanged(nameof(MessageLabelText));
                }
            }
        }

        public SecureString SecurePassword { private get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EncryptViewModel()
        {
            ShowMenuViewModelCommand = new ViewModelCommand(ExecuteShowMenuViewModelCommand);
            BrowseFileCommand = new ViewModelCommand(ExecuteBrowseFileCommand);
            EncryptFileCommand = new ViewModelCommand(ExecuteEncryptFileCommand);
        }

        private void ExecuteEncryptFileCommand(object obj)
        {
            string originalFilePath = FileName;
            string encryptedFilePath = FileName + ".aes";
            string passText = SecurePassword.ToString();
            const int SaltSize = 16;
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            const int Iterations = 100000;
            const int KeySize = 32;

            byte[] hashedPassword = Rfc2898DeriveBytes.Pbkdf2(passText, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            try
            {
                EncryptFile(originalFilePath, encryptedFilePath, hashedPassword, salt);
                MessageLabelText = $"File encrypted successfully to: {encryptedFilePath}";
            }

            catch (Exception ex)
            {
                MessageLabelText = $"Encryption failed: {ex.Message}";
            }
        }

        private static void EncryptFile(string inputFile, string outputFile, byte[] key, byte[] salt)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV();

                using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
                {
                    fsOutput.Write(salt, 0, salt.Length);
                    fsOutput.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream cs = new CryptoStream(fsOutput, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
                        {
                            fsInput.CopyTo(cs);
                        }
                    }
                }
            }
        }

        private void ExecuteBrowseFileCommand(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt|Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        }

        private void ExecuteShowMenuViewModelCommand(object parameter)
        {
            if (App.Current.MainWindow.DataContext is MainViewModel mainWindowViewModel)
            {
                mainWindowViewModel.CurrentChildView = new MenuViewModel();
            }
        }
    }
}
