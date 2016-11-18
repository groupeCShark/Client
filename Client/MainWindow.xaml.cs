using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

            // Automaticllay scroll the ListView when new item is added
            messageViewModel.Messages.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null)
                {
                    Decorator border = VisualTreeHelper.GetChild(MessageListView, 0) as Decorator;
                    ScrollViewer scroller = border.Child as ScrollViewer;
                    scroller.ScrollToBottom();
                }
            };
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(TextInput.Text))
            {
                string textInputContent = TextInput.Text;
                messageViewModel.Messages.Add(new Message() { Username = "Me", Text = TextInput.Text });
                messageViewModel.SendMessage(textInputContent);
                TextInput.Text = "";
            }
        }

        private void SendAction(object sender, RoutedEventArgs e)
        {
            SendMessage();
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

        void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }
    }
}
