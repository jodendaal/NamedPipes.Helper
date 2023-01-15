using System.IO.Pipes;

namespace NamedPipes.Helper
{
    internal class NamedPipeStream : IDisposable
    {
        NamedPipeServerStream _pipeServer;
        Action<string> _onMessageRecieved;
        public NamedPipeStream(string pipeName, Action<string> onMessageRecieved)
        {
            _pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 10);
            _onMessageRecieved = onMessageRecieved;
        }

        public void Dispose()
        {
            _pipeServer.Dispose();
        }

        public void ListenForConnections()
        {
            try
            {
                _pipeServer.WaitForConnectionAsync();
                StreamString ss = new StreamString(_pipeServer);
                var message = ss.ReadString();
                if (message != "DISPOSE_MESSAGE")
                {
                    _onMessageRecieved?.Invoke(message);
                }
            }
            catch (IOException)
            {
                Dispose();
            }
        }

    }
}
