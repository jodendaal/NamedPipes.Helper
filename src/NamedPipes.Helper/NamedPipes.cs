using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NamedPipes.Helper
{


    public class NamedPipes : IDisposable
    {
        public Server _server;
        private readonly string _pipeName;

        public NamedPipes(string pipeName, Action<string> onMessageRecieved)
        {
            _server = new Server(pipeName, onMessageRecieved);
            _pipeName = pipeName;
        }

        public void Start(int noOfThreads = 1)
        {
            _server.Start(noOfThreads);
        }

        public void SendMessage(string message)
        {
            Client.SendMessage(_pipeName, message);
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
