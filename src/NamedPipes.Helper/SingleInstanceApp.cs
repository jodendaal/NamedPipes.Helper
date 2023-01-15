namespace NamedPipes.Helper
{
    internal class SingleInstanceApp : IDisposable
    {
        private static NamedPipes? _namedPipes;
        public SingleInstanceApp(string id, Action initialise, Action<string>? onMessageRecieved = null, string message = "")
        {
            if (onMessageRecieved != null)
            {
                _namedPipes = new NamedPipes(id, (message) =>
                {
                    onMessageRecieved?.Invoke(message);
                });
                _namedPipes.Start(1);
            }

            using (Mutex mutex = new Mutex(true, id, out var mutexCreated))
            {
                if (mutexCreated)
                {
                    initialise?.Invoke();
                }
                else
                {
                    _namedPipes?.SendMessage(message);
                }
            }
        }

        public void Dispose()
        {
            _namedPipes?.Dispose();
        }
    }
}