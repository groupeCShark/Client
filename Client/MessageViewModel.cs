using CSharkLibrary;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CSharkClient
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public NetworkManager client;
        public string Username = "Htime";

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set {
                _messages = value;
                NotifyPropertyChanged("_messages");
            }
        }

        public MessageViewModel()
        {
            client = new NetworkManager(this);
            Login();
            PrintLoggedUsers();
        }

        private void PrintNetworkErrorMessage(string Message)
        {
            Messages.Add(new Message() { Username = "Server", Text = "Network issue: Unreachable server (" + Message + ")" });
        }

        private void PrintLoggedUsers()
        {
            User[] loggedUsers = client.GetLoggedUsers();
            if (loggedUsers == null)
            {
                PrintNetworkErrorMessage("PrintLoggedUsers");
            }
            else
            {
                string usersList = string.Join<User>(", ", loggedUsers);
                Messages.Add(new Message() { Username = "Server", Text = "Users: " + usersList });
            }
        }

        public void Login() {
            bool connected = client.Login(Username);
            if (!connected)
            {
                PrintNetworkErrorMessage("Login");
            }
            else
            {
                Messages.Add(new Message() { Username = "Server", Text = "Connected!" });
            }
        }

        public void Logout()
        {
            bool connected = client.Logout();
            if (!connected)
            {
                PrintNetworkErrorMessage("Logout");
            }
            else
            {
                Messages.Add(new Message() { Username = "Server", Text = "Deconnection..." });
            }
        }

        public void SendMessage(string Text) {
            bool connected = client.SendMessage(Username, Text);
            if (!connected)
            {
                PrintNetworkErrorMessage("SendMessage");
            }
        }

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
