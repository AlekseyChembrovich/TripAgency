using System;
using System.Windows.Forms;
using TripAgency.DataAccessLayer.Models;
using TripAgency.DataAccessLayer.Repository;

namespace TripAgency.PrecentationLayer.Forms
{
    public partial class FormChangeTrip : Form
    {
        private readonly FormMain _formMain;
        private readonly Trip _trip;
        private readonly IRepository<Trip> _repositoryTrip;

        public FormChangeTrip(FormMain formMain, Trip trip, IRepository<Trip> repositoryTrip)
        {
            InitializeComponent();
            _formMain = formMain;
            _trip = trip;
            _repositoryTrip = repositoryTrip;
        }

        private void FormChangeClient_Load(object sender, EventArgs e)
        {
            textBox1.Text = _trip.Name;
            textBox2.Text = _trip.Price.ToString("F");
            dateTimePicker1.Value = _trip.DepartureDate;
            textBox3.Text = _trip.CountNights.ToString();
            textBox4.Text = _trip.CountKids.ToString();
            comboBox1.SelectedItem = _trip.Country;
            comboBox2.SelectedItem = _trip.TypeTrip;
            comboBox3.SelectedItem = _trip.Nutrition;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны ввести все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var trip = new Trip
            {
                Id = _trip.Id,
                Name = textBox1.Text,
                Price = Convert.ToDecimal(textBox2.Text),
                DepartureDate = dateTimePicker1.Value.Date,
                CountNights = Convert.ToInt32(textBox3.Text),
                CountKids = Convert.ToInt32(textBox4.Text),
                Country = comboBox1.SelectedItem.ToString(),
                TypeTrip = comboBox2.SelectedItem.ToString(),
                Nutrition = comboBox3.SelectedItem.ToString()
            };

            _repositoryTrip.Update(trip);
            _formMain.button4_Click(null, null);
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

        private void EnterOnlyDigit(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        private void EnterFractionalNumber(object sender, KeyPressEventArgs e)
        {
            var textBox = sender as TextBox;
            var isContain = textBox?.Text.Contains(',') ?? false;
            if (isContain && e.KeyChar is ',')
            {
                e.Handled = true;
                return;
            }

            if (char.IsDigit(e.KeyChar) || e.KeyChar is (char)Keys.Back or (char)Keys.Delete or ',')
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }
    }
}
