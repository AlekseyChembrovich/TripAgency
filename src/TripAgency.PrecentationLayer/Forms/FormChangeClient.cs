using System;
using System.Windows.Forms;
using TripAgency.DataAccessLayer.Models;
using TripAgency.DataAccessLayer.Repository;

namespace TripAgency.PrecentationLayer.Forms
{
    public partial class FormChangeClient : Form
    {
        private readonly FormMain _formMain;
        private readonly Client _client;
        private readonly IRepository<Client> _repositoryClient;

        public FormChangeClient(FormMain formMain, Client client, IRepository<Client> repositoryClient)
        {
            InitializeComponent();
            _formMain = formMain;
            _client = client;
            _repositoryClient = repositoryClient;
        }

        private void FormChangeClient_Load(object sender, EventArgs e)
        {
            textBox1.Text = _client.Surname;
            textBox2.Text = _client.Name;
            textBox3.Text = _client.Patronymic;
            maskedTextBox1.Text = _client.Phone;
            maskedTextBox2.Text = _client.Passport;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || maskedTextBox1.Text.Trim(' ').Length < 18 ||
                maskedTextBox2.Text.Trim(' ').Length < 14)
            {
                MessageBox.Show("Вы должны ввести все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var client = new Client
            {
                Id = _client.Id,
                Surname = textBox1.Text,
                Name = textBox2.Text,
                Patronymic = textBox3.Text,
                Phone = maskedTextBox1.Text,
                Passport = maskedTextBox2.Text
            };

            _repositoryClient.Update(client);
            _formMain.button1_Click(null, null);
            this.Close();
        }

        private void EnterOnlyLetter(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }
    }
}
