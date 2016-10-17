using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class NetworkManager
    {
        private Server.Service1Client server;
        private MessageViewModel messageViewModel;

        public NetworkManager(MessageViewModel messageViewModel) {
            this.messageViewModel = messageViewModel;
            this.server = new Server.Service1Client();
        }

        public List<string> Authentification(string Username) {
            try
            {
                List<string> pendingUsernames = new List<string>();
                Server.LogResult logResults = server.Auth(Username);
                pendingUsernames = new List<string>(logResults.UserList);
                return pendingUsernames;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ReceiveMessage(string Username, string Text) {
            Message message = new Message() { Username = Username, Text = Crypto.Decrypt(Text) };
            this.messageViewModel.Messages.Add(message);
        }

        public bool SendMessage(string Username, string Message) {
            try
            {
                server.Send(Username, Crypto.Encrypt(Message));
                ReceiveMessage("God", Crypto.Encrypt("Release the Kraken!"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
