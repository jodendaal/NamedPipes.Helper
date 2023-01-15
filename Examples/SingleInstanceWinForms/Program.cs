using NamedPipes.Helper;

namespace SingleInstanceWinForms
{
    internal static class Program
    {
        static Form1? _form;
        
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            App.SingleInstance(nameof(SingleInstanceWinForms), () =>
            {
                _form = new Form1();
                Application.Run(_form);

            },(mesage)=> {
               _form?.Invoke(()=> _form.ShowMessage(mesage));
            },"Another instance started");
        }
    }
}