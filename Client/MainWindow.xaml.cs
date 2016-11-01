using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CSharkClient
{
    public partial class MainWindow : Window
    {
        private MessageViewModel messageViewModel;

        public MainWindow()
        {
            InitializeComponent();
            messageViewModel = new MessageViewModel();
            this.DataContext = messageViewModel;
        }

        private void SendAction(object sender, RoutedEventArgs e)
        {
            string textInputContent = TextInput.Text;
            messageViewModel.Messages.Add(new Message() { Username = "Me", Text = TextInput.Text });
            messageViewModel.SendMessage(textInputContent);
            TextInput.Text = "";
        }

        private void FileAction(object sender, RoutedEventArgs e)
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All Files|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                string filename = fileDialog.FileName;
                messageViewModel.Messages.Add(new Message() { Username = "Me", Text = filename });
                messageViewModel.UploadFile(filename);
            }
        }

        void ClosingWindow(object sender, EventArgs e)
        {
            messageViewModel.Logout();
        }
    }
}
