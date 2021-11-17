using System;
using System.Windows.Forms;
using TripAgency.DataAccessLayer.Models;
using TripAgency.DataAccessLayer.Repository;

namespace TripAgency.PrecentationLayer.Forms
{
    public partial class FormChangeEmployee : Form
    {
        private readonly FormMain _formMain;
        private readonly Employee _employee;
        private readonly IRepository<Employee> _repositoryEmployee;

        public FormChangeEmployee(FormMain formMain, Employee employee, IRepository<Employee> repositoryEmployee)
        {
            InitializeComponent();
            _formMain = formMain;
            _employee = employee;
            _repositoryEmployee = repositoryEmployee;
        }

        private void FormChangeEmployee_Load(object sender, EventArgs e)
        {
            textBox1.Text = _employee.Surname;
            textBox2.Text = _employee.Name;
            textBox3.Text = _employee.Patronymic;
            maskedTextBox1.Text = _employee.Phone;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || maskedTextBox1.Text.Trim(' ').Length < 18)
            {
                MessageBox.Show("Вы должны ввести все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var employee = new Employee
            {
                Id = _employee.Id,
                Surname = textBox1.Text,
                Name = textBox2.Text,
                Patronymic = textBox3.Text,
                Phone = maskedTextBox1.Text,
            };

            _repositoryEmployee.Update(employee);
            _formMain.button2_Click(null, null);
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
