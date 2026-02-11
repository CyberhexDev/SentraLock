using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace SentraLock.Views
{
    /// <summary>
    /// Interaction logic for EncryptView.xaml
    /// </summary>
    public partial class EncryptView : UserControl
    {
        public EncryptView()
        {
            InitializeComponent();
        }

        public static void DecryptFile(string inputFile, string outputFile, byte[] key)
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
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(this.DataContext != null)
            {
                ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}
