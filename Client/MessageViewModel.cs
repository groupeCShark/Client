using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public NetworkManager client;
        public string Username = "Htime";
        public string Recipient;

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set {
                _messages = value;
                NotifyPropertyChanged("_messages");
            }
        }

        private void PrintNetworkErrorMessage(string Message)
        {
            Messages.Add(new Message() { Username = "Server", Text = "Network issue: Unreachable server (" + Message + ")" });
        }

        public bool Authentification() {
            List<string> pendingUsernames = client.Authentification(Username);
            if (pendingUsernames != null)
            {
                Messages.Add(new Message() { Username = "Server", Text = "Please select a user to start session with (!connect <username>):\n" + string.Join(", ", pendingUsernames.ToArray()) });
                return true;
            }
            return false;
        }

        public void StartSessionWith(string Recipient)
        {
            Messages.Clear();
            this.Recipient = Recipient;
        }

        public void SendMessage(string Text) {
            bool connected = client.SendMessage(Username, Text);
            if (!connected) {
                PrintNetworkErrorMessage("SendMessage");
            }
        }

        public MessageViewModel() {
            client = new NetworkManager(this);
            bool connected = Authentification();
            if (!connected) {
                PrintNetworkErrorMessage("MessageViewModel's constructor");
            }
            
            //_messages.Add(new Message() { Username = "Server", Text = "Hello from the server" });
            //_messages.Add(new Message() { Username = "Server", Text = "Htime, Appo, Oliver" });
            //_messages.Add(new Message() { Username = "Me", Text = "!connect Appo" });
            //_messages.Add(new Message() { Username = "Me", Text = "Hi there!" });
            //_messages.Add(new Message() { Username = "Appo", Text = "Hey! Nice to see you again :-)" });
            //_messages.Add(new Message() { Username = "Crypto", Text = Crypto.Encrypt("Hello World!") });
            //_messages.Add(new Message() { Username = "Crypto", Text = Crypto.Decrypt(Crypto.Encrypt("Hello World!")) });
        }

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
