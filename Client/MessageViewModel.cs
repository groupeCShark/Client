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

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set {
                _messages = value;
                NotifyPropertyChanged("_messages");
            }
        }

        public void Authentification() {
            if (client.IsConnected) {
                string username = "Htime";
                List<string> pendingUsernames = client.Authentification(username);
                if (pendingUsernames.Any())
                {
                    Messages.Add(new Message() { Username = "Server", Text = "Please select a user to start session with (!connect <username>):\n" + string.Join(", ", pendingUsernames.ToArray()) });
                }
                else
                {
                    Messages.Add(new Message() { Username = "Server", Text = "No user is connected..." });
                }
            }
        }

        public void StartSessionWith(string username) {
            if (client.IsConnected) {
                client.StartSessionWith(username);
                _messages.Clear();
            }
        }

        public void SendMessage(string text) {
            if (client.IsConnected) {
                client.SendMessage(text);
            }
        }

        public MessageViewModel() {
            client = new NetworkManager(this);
            if (!client.IsConnected) {
                Messages.Add(new Message() { Username = "Server", Text = "Network issue: Unreachable server..." });
            } else {
                this.Authentification();
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
