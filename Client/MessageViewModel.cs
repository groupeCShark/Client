using CSharkLibrary;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CSharkClient
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public NetworkManager client;

        public string Username = "";
        public string DownloadDirectory = "";

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
            Username = ConfigSettings.InitFile();
            client = new NetworkManager(this);
            Login();
            PrintLoggedUsers();
        }

        public void ProcessInput(string TextInput)
        {
            Regex regex = new Regex(@"!(\w+) *([A-Za-z0-9_ ]*)");
            Match match = regex.Match(TextInput);
            if (match.Success)
            {
                switch (match.Groups[1].Value)
                {
                    case "username":
                        ConfigSettings.EditElement("username", match.Groups[2].Value);
                        Username = ConfigSettings.ReadElement("username");
                        Logout();
                        Login();
                        break;
                    default:
                        AddMessage("Server", "Command not found...");
                        break;
                }
            }
            else
            {
                SendMessage(TextInput);
            }
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

        public void AddMessage(string Username, string Text)
        {
            Messages.Add(new Message() { Username = Username, Text = Text });

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

        public void UploadFile(string Filename)
        {
            bool connected = client.UploadFile(Filename);
            if (!connected)
            {
                PrintNetworkErrorMessage("UploadFile");
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
