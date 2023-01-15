using System.Threading;

namespace NamedPipes.Helper
{
    internal class PipeServer : IDisposable
    {
        SynchronizedCollection<NamedPipeStream> _pipes = new SynchronizedCollection<NamedPipeStream>();
        string _pipeName;
        Action<string> _onMessageRecieved;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public PipeServer(string pipeName, Action<string> onMessageRecieved)
        {
            _pipeName = pipeName;
            _onMessageRecieved = onMessageRecieved;
        }

        public void Listen(int noOfThreads)
        {
            for (var i = 0; i < noOfThreads; i++)
            {
                Task.Factory.StartNew( async () =>
                {
                    using (var pipe = new NamedPipeStream(_pipeName, _onMessageRecieved,noOfThreads))
                    {
                        _pipes.Add(pipe);
                        await pipe.ListenForConnectionsAsync(_cancellationTokenSource.Token);
                        _pipes.Remove(pipe);
                    }
                   
                    Listen(1);
                    
                });
            }
        }


        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
          
            var pipes = _pipes.ToList();
            pipes.ForEach(pipe =>
            {
                _pipes.Remove(pipe);
                pipe.Dispose();
            });

            _cancellationTokenSource.Dispose();
        }
    }
}
