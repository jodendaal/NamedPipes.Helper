namespace SingleInstanceWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public void ShowMessage(string message)
        {
            txtMessages.Text += $"{Environment.NewLine}{message}";
        }
    }
}