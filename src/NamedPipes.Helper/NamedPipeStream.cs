using System.IO.Pipes;

namespace NamedPipes.Helper
{
    internal class NamedPipeStream : IDisposable
    {
        NamedPipeServerStream _pipeServer;
        Action<string> _onMessageRecieved;
        public NamedPipeStream(string pipeName, Action<string> onMessageRecieved,int maxNoOfInstance = 10)
        {
            _pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxNoOfInstance, PipeTransmissionMode.Byte,PipeOptions.Asynchronous);
            _onMessageRecieved = onMessageRecieved;
        }

        public void Dispose()
        {
            _pipeServer.Dispose();
        }

        public async Task ListenForConnectionsAsync(CancellationToken cancellationToken)
        {
            await _pipeServer.WaitForConnectionAsync(cancellationToken);
            StreamString ss = new StreamString(_pipeServer);
            var message = ss.ReadString();
            _onMessageRecieved?.Invoke(message);
        }
    }
}
