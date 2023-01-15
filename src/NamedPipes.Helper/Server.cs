namespace NamedPipes.Helper
{
    public class Server : IDisposable
    {
        PipeServer _pipeServer;
        public Server(string pipeName, Action<string> onMessageRecieved)
        {
            _pipeServer = new PipeServer(pipeName, onMessageRecieved);
        }

        public void Start(int noOfThreads = 1)
        {
            _pipeServer.Listen(noOfThreads);
        }

        public void Dispose()
        {
            _pipeServer.Dispose();
        }
    }
}
