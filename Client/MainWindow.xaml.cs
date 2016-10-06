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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Message> Messages = new ObservableCollection<Message>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Messages;

            Messages.Add(new Message() { Username = "Server", Text = "Hello from the server" });
            Messages.Add(new Message() { Username = "Me", Text = "Hi there!" });
            Messages.Add(new Message() { Username = "Bob", Text = "Hey! Nice to see you again :-)" });
        }

        private void SendAction(object sender, RoutedEventArgs e)
        {
            Messages.Add(new Message() { Username = "Me", Text = TextInput.Text });
            TextInput.Text = "";
        }

        public class Message
        {
            public string Username { get; set; }
            public string Text { get; set; }
            public string Display
            {
                get
                {
                    return Username + ":\n" + Text;
                }
            }
        }
    }
}
