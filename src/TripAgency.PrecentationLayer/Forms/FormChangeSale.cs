using System;
using System.Linq;
using System.Windows.Forms;
using TripAgency.DataAccessLayer.Models;
using TripAgency.DataAccessLayer.Repository;
using TripAgency.PrecentationLayer.Tools;

namespace TripAgency.PrecentationLayer.Forms
{
    public partial class FormChangeSale : Form
    {
        private readonly FormMain _formMain;
        private readonly Sale _sale;
        private readonly IRepository<Sale> _repositorySale;
        private readonly IRepository<Client> _repositoryClient;
        private readonly IRepository<Trip> _repositoryTrip;
        private readonly IRepository<Employee> _repositoryEmployee;

        public FormChangeSale(FormMain formMain, Sale sale, IRepository<Sale> repositorySale,
            IRepository<Client> repositoryClient, IRepository<Trip> repositoryTrip, 
            IRepository<Employee> repositoryEmployee)
        {
            InitializeComponent();
            _formMain = formMain;
            _sale = sale;
            _repositorySale = repositorySale;
            _repositoryClient = repositoryClient;
            _repositoryTrip = repositoryTrip;
            _repositoryEmployee = repositoryEmployee;
        }

        private void FormChangeSale_Load(object sender, EventArgs e)
        {
            this.FillCombobox(comboBox1, _repositoryClient.GetAll().Select(x => x.Surname).ToArray());
            this.FillCombobox(comboBox2, _repositoryTrip.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox3, _repositoryEmployee.GetAll().Select(x => x.Surname).ToArray());
            dateTimePicker1.Value = _sale.Data;
            textBox1.Text = _sale.Count.ToString();
            var client = _repositoryClient.GetById(_sale.ClientId);
            comboBox1.SelectedItem = client.Surname;
            var trip = _repositoryTrip.GetById(_sale.TripId);
            comboBox2.SelectedItem = trip.Name;
            var employee = _repositoryEmployee.GetById(_sale.EmployeeId);
            comboBox3.SelectedItem = employee.Surname;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || comboBox1.SelectedIndex == -1 ||
                comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны ввести все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var client = _repositoryClient.GetModelByProperty(comboBox1.SelectedItem.ToString(), "Surname");
            var trip = _repositoryTrip.GetModelByProperty(comboBox2.SelectedItem.ToString(), "Name");
            var employee = _repositoryEmployee.GetModelByProperty(comboBox3.SelectedItem.ToString(), "Surname");
            var sale = new Sale
            {
                Id = _sale.Id,
                Data = dateTimePicker1.Value.Date,
                Count = Convert.ToInt32(textBox1.Text),
                ClientId = client.Id,
                TripId = trip.Id,
                EmployeeId = employee.Id
            };

            _repositorySale.Update(sale);
            _formMain.button3_Click(null, null);
            this.Close();
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
    }
}
