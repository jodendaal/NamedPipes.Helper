using System.IO.Pipes;

namespace NamedPipes.Helper
{
    public class Client
    {
        public static void SendMessage(string pipeName, string message)
        {
            using (var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.None))
            {
                pipeClient.Connect();

                StreamString ss = new StreamString(pipeClient);
                ss.WriteString(message);

                pipeClient.Close();
            }
        }

        public static async Task SendMessageAsync(string pipeName, string message, CancellationToken? cancellationToken = null)
        {
            using (var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.None))
            {
                try
                {
                    await pipeClient.ConnectAsync(cancellationToken ?? CancellationToken.None);

                    StreamString ss = new StreamString(pipeClient);
                    ss.WriteString(message);
                }
                catch (IOException)
                {
                    pipeClient.Close();
                }
            }
        }
    }
}
