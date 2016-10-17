using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class NetworkManager
    {
        private Server.Service1Client server;
        private MessageViewModel messageViewModel;

        public bool IsConnected
        {
            get
            {
                return server != null;
            }
        }

        public NetworkManager(MessageViewModel messageViewModel) {
            this.messageViewModel = messageViewModel;
            this.server = new Server.Service1Client();
        }

        public List<string> Authentification(string Username) {
            List<string> pendingUsernames = new List<string>();
            Server.LogRes logResults = server.auth(Username);
            pendingUsernames = new List<string>(logResults.UserList);
            //pendingUsernames.Add("Appo");
            //pendingUsernames.Add("Oliver");
            return pendingUsernames;
        }

        public bool StartSessionWith(string username) {
            return server.startSession(username);
        }

        public void ReceiveMessage(string Username, string Text) {
            Message message = new Message() { Username = Username, Text = Crypto.Decrypt(Text) };
            this.messageViewModel.Messages.Add(message);
        }

        public bool SendMessage(string message) {
            // Test
            ReceiveMessage("God", Crypto.Encrypt("Release the Kraken!"));
            return server.send(Crypto.Encrypt(message));
        }
    }
}
