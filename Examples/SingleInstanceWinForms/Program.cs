using NamedPipes.Helper;

namespace SingleInstanceWinForms
{
    //testmessage://helloworld
    internal static class Program
    {
        static Form1? _form;
        
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            App.SingleInstance(nameof(SingleInstanceWinForms), () =>
            {
                _form = new Form1();
                Application.Run(_form);

            },(mesage)=> {
               _form?.Invoke(()=> _form.ShowMessage(mesage));
            }, args.Length> 0 ? $"Another instance started with args : {string.Join(" ", args)}" : "Another instance started with no args");
        }
    }
}