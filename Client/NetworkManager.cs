using System;
using System.ServiceModel;
using CSharkLibrary;
using System.IO;
using System.Diagnostics;

namespace CSharkClient
{
    public class CSharkClientImpl : ICSharkClient
    {
        private MessageViewModel messageViewModel;

        public CSharkClientImpl(MessageViewModel messageViewModel)
        {
            this.messageViewModel = messageViewModel;
        }

        public void DownloadFile(CSharkFile file)
        {
            string downloadDirectory = System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Download";
            System.IO.Directory.CreateDirectory(downloadDirectory);
            string filePath = downloadDirectory + Path.DirectorySeparatorChar + file.Filename;

            using (FileStream stream = File.Create(filePath))
            {
                file.FileByteStream.CopyTo(stream);
            }
            Message message = new Message() { Username = file.Username, Text =  file.Filename};
            this.messageViewModel.Messages.Add(message);
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

        public bool UploadFile(string Filename)
        {
            try
            {
                using (FileStream stream = File.Open(Filename, FileMode.Open, FileAccess.Read))
                {
                    CSharkFile file = new CSharkFile() { Filename = Path.GetFileName(Filename), FileByteStream = stream};
                    server.UploadFile(file);
                    return true;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
