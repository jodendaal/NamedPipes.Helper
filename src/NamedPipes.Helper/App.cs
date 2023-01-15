namespace NamedPipes.Helper
{
    public class App
    {
        public static void SingleInstance(string id, Action initialise, Action<string>? onMessageRecieved = null, string message = "")
        {
            using (var app = new SingleInstanceApp(id, initialise, onMessageRecieved, message))
            {

            }
        }
    }
}