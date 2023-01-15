namespace NamedPipes.Helper
{
    internal class PipeServer : IDisposable
    {
        SynchronizedCollection<NamedPipeStream> _pipes = new SynchronizedCollection<NamedPipeStream>();
        string _pipeName;
        Action<string> _onMessageRecieved;
        private bool _isDisposed = false;
        public PipeServer(string pipeName, Action<string> onMessageRecieved)
        {
            _pipeName = pipeName;
            _onMessageRecieved = onMessageRecieved;
        }

        public void Listen(int noOfThreads)
        {
            for (var i = 0; i < noOfThreads; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    using (var pipe = new NamedPipeStream(_pipeName, _onMessageRecieved))
                    {
                        _pipes.Add(pipe);
                        pipe.ListenForConnections();
                        _pipes.Remove(pipe);
                    }
                    if (!_isDisposed)
                    {
                        Listen(1);
                    }
                });
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            var pipes = _pipes.ToList();
            pipes.ForEach(pipe =>
            {
                SendDisposedMessage();
                _pipes.Remove(pipe);
                pipe.Dispose();
            });
        }

        //This is here because the Pipe remains open listening until it receives another message.
        //This is problematic becuase if you call dispose then Listen again for the same pipe,
        //the previous pipes are still listening and will error for the first N (NoOfThreads)  messages received.
        //This is message will not raise call onMessageRecieved action, so consumers should not be affected.
        //Couldn't find a better way to be able to handle this in the same process. ?
        private void SendDisposedMessage()
        {
            Client.SendMessage(_pipeName, "DISPOSE_MESSAGE");
        }
    }
}
