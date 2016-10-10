using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class MessageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set {
                _messages = value;
                NotifyPropertyChanged("_messages");
            }
        }


        public MessageViewModel() {
            _messages.Add(new Message() { Username = "Server", Text = "Hello from the server" });
            _messages.Add(new Message() { Username = "Me", Text = "Hi there!" });
            _messages.Add(new Message() { Username = "Bob", Text = "Hey! Nice to see you again :-)" });
        }

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
