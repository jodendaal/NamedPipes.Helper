using NamedPipes.Helper;

namespace NamedPipesExample
{
    public partial class Form1 : Form
    {
        Server? _server;
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            await Client.SendMessageAsync(txtPipeName.Text,txtMessage.Text);
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (_server == null)
            {
                _server = new Server(txtPipeName.Text, (mesage) =>
                {
                    this.Invoke(() =>
                    {
                        txtRecievedMessages.Text += mesage;
                    });
                });

                _server.Start(2);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _server?.Dispose();
            base.OnFormClosing(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _server?.Dispose();
            _server = null;
        }
    }
}