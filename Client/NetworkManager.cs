using System;
using System.Collections.Generic;
using System.ServiceModel;
using CSharkLibrary;

namespace CSharkClient
{
    public class CSharkClientImpl : ICSharkClient
    {
        private MessageViewModel messageViewModel;

        public CSharkClientImpl(MessageViewModel messageViewModel)
        {
            this.messageViewModel = messageViewModel;
        }

        public void ReceiveMessage(string username, string text)
        {
            Message message = new Message() { Username = username, Text = Crypto.Decrypt(text) };
            this.messageViewModel.Messages.Add(message);
        }
    }

    public class NetworkManager
    {
        private ICSharkService server;
        private MessageViewModel messageViewModel;

        public NetworkManager(MessageViewModel messageViewModel) {
            this.messageViewModel = messageViewModel;
            var channelFactory = new DuplexChannelFactory<ICSharkService>(new CSharkClientImpl(messageViewModel), "CSharkServiceEndpoint");
            server = channelFactory.CreateChannel();
        }

        public User[] GetLoggedUsers()
        {
            try
            {
                return server.LoggedUsers;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool Login(string Username)
        {
            try
            {
                server.Login(Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Logout()
        {
            try
            {
                server.Logout();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendMessage(string Username, string Message) {
            try
            {
                server.SendMessage(Crypto.Encrypt(Message));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
