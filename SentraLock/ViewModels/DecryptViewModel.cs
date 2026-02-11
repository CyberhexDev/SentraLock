using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SentraLock.ViewModels
{
    class DecryptViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand ShowMenuViewModelCommand { get; }
        public ICommand BrowseFileCommand { get; }
        public ICommand DecryptFileCommand { get; }

        private string _fileName = "Select a file...";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
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
                if (_messageLabelText != value)
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

        public DecryptViewModel()
        {
            ShowMenuViewModelCommand = new ViewModelCommand(ExecuteShowMenuViewModelCommand);
            BrowseFileCommand = new ViewModelCommand(ExecuteBrowseFileCommand);
            DecryptFileCommand = new ViewModelCommand(ExecuteDecryptFileCommand);
        }

        private void ExecuteDecryptFileCommand(object obj)
        {
            string encryptedFilePath = FileName; // string encryptedFilePath = "plaintText.aes";
            string filePathToRemove = ".aes";
            string decryptedFilePath = encryptedFilePath.Replace(filePathToRemove, ""); // string decryptedFilePath = "plainText.txt";
            string passText = SecurePassword.ToString();

            try
            {
                DecryptFile(encryptedFilePath, decryptedFilePath, passText);
                MessageLabelText = $"File decrypted successfully to: {decryptedFilePath}";
            }

            catch (Exception ex)
            {
                MessageLabelText = $"Encryption failed: {ex.Message}";
            }
        }

        private static void DecryptFile(string inputFile, string outputFile, string password)
        {
            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
            {
                int saltSize = 16;
                int ivSize = 16;

                byte[] salt = new byte[saltSize];
                byte[] iv = new byte[ivSize];

                fsInput.Read(salt, 0, saltSize);
                fsInput.Read(iv, 0, ivSize);

                using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
                {
                    byte[] key = deriveBytes.GetBytes(32);

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = key;
                        aesAlg.IV = iv;

                        using (CryptoStream csDecrypt = new CryptoStream(fsInput, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
                            {
                                csDecrypt.CopyTo(fsOutput);
                            }
                        }
                    }
                }
            }
        }

        /*private static void DecryptFile(string inputFile, string outputFile, byte[] key)
        {
            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                byte[] iv = new byte[aesAlg.IV.Length];
                fsInput.ReadExactly(iv);
                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (CryptoStream csDecrypt = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                {
                    csDecrypt.CopyTo(fsOutput);
                }
            }
        }*/

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
