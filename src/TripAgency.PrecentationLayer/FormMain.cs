using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using TripAgency.DataAccessLayer.Models;
using TripAgency.PrecentationLayer.Tools;
using TripAgency.PrecentationLayer.Forms;
using Word = Microsoft.Office.Interop.Word;
using TripAgency.DataAccessLayer.Repository;
using Excel = Microsoft.Office.Interop.Excel;
using TripAgency.DataAccessLayer.Repository.EntityFramework;

namespace TripAgency.PrecentationLayer
{
    public partial class FormMain : Form
    {
        public CurrentTable CurrentTable = CurrentTable.Employee;
        private readonly IRepository<Employee> _repositoryEmployee;
        private readonly IRepository<Client> _repositoryClient;
        private readonly IRepository<Trip> _repositoryTrip;
        private readonly IRepository<Sale> _repositorySale;
        private readonly List<Panel> _addPanels;
        private readonly List<Button> _tablesButtons;

        public FormMain()
        {
            InitializeComponent();
            var databaseContext = new DatabaseContext();

            #region Init repositoties

            _repositoryEmployee = new BaseRepository<Employee>(databaseContext);
            _repositoryClient = new BaseRepository<Client>(databaseContext);
            _repositoryTrip = new BaseRepository<Trip>(databaseContext);
            _repositorySale = new RepositorySale(databaseContext);

            #endregion

            _addPanels = new List<Panel> { panel5, panel6, panel7, panel8 };
            _tablesButtons = new List<Button> { button1, button2, button3, button4 };
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            UpdateCombobox();
            button1_Click(null, null);
        }

        private void PickOutButtonCurrentTable(CurrentTable currentTable)
        {
            var pickOut = Color.FromArgb(52, 74, 100);
            var normal = Color.FromArgb(52, 74, 121);
            var index = (int)currentTable;
            _tablesButtons[index].BackColor = pickOut;
            _tablesButtons.Except(new[] { _tablesButtons[index] }).ToList().ForEach(x =>
            {
                x.BackColor = normal;
            });
        }

        private void UpdateCombobox()
        {
            this.FillCombobox(comboBox1, _repositoryClient.GetAll().Select(x => x.Surname).ToArray());
            this.FillCombobox(comboBox2, _repositoryTrip.GetAll().Select(x => x.Name).ToArray());
            this.FillCombobox(comboBox3, _repositoryEmployee.GetAll().Select(x => x.Surname).ToArray());
        }

        #region Print

        public void button1_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Client;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel5, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Фамилия", "Имя", "Отчество", "Паспорт", "Телефон"
            );
            foreach (var item in _repositoryClient.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Surname,
                    item.Name,
                    item.Patronymic,
                    item.Passport,
                    item.Phone
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
            //ToggleFiltrationMenu(false);
        }

        public void button2_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Employee;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel6, _addPanels);
            this.GenerateColumns(
                listView1,
                120,
                "Код", "Фамилия", "Имя", "Отчество", "Телефон"
            );
            foreach (var item in _repositoryEmployee.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Surname,
                    item.Name,
                    item.Patronymic,
                    item.Phone
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
            //ToggleFiltrationMenu(false);
        }

        public void button3_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Sale;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel7, _addPanels);
            this.GenerateColumns(
                listView1,
                100,
                "Код", "Дата", "Цена", "Клиент", "Тур", "Сотрудник"
            );
            if (_repositorySale is not RepositorySale repositorySale) return;
            foreach (var item in repositorySale.GetAllIncludeForeignKey())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Data.ToShortDateString(),
                    item.Count.ToString(),
                    item.Client.Surname,
                    item.Trip.Name,
                    item.Employee.Surname
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
            //ToggleFiltrationMenu(false);
        }

        public void button4_Click(object sender, EventArgs e)
        {
            CurrentTable = CurrentTable.Trip;
            PickOutButtonCurrentTable(CurrentTable);
            this.OpenAddPanel(panel8, _addPanels);
            this.GenerateColumns(
                listView1,
                100,
                "Код", "Название", "Цена", "Дата отправки", "Кол-во ночей", "Кол-во детей", "Страна", "Тип тура", "Питание"
            );
            foreach (var item in _repositoryTrip.GetAll())
            {
                var newItem = new ListViewItem(new[]
                {
                    item.Id.ToString(),
                    item.Name,
                    item.Price.ToString("F"),
                    item.DepartureDate.ToShortDateString(),
                    item.CountNights.ToString(),
                    item.CountKids.ToString(),
                    item.Country,
                    item.TypeTrip,
                    item.Nutrition
                });
                listView1.Items.Add(newItem);
            }

            UpdateCombobox();
            //ToggleFiltrationMenu(false);
        }

        #endregion

        #region Add

        private void button5_Click(object sender, EventArgs e)
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
                Surname = textBox1.Text,
                Name = textBox2.Text,
                Patronymic = textBox3.Text,
                Phone = maskedTextBox1.Text,
                Passport = maskedTextBox2.Text
            };

            _repositoryClient.Insert(client);
            button1_Click(null, null);
            button6_Click(null, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) || maskedTextBox4.Text.Trim(' ').Length < 18)
            {
                MessageBox.Show("Вы должны ввести все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var employee = new Employee
            {
                Surname = textBox6.Text,
                Name = textBox5.Text,
                Patronymic = textBox4.Text,
                Phone = maskedTextBox4.Text
            };

            _repositoryEmployee.Insert(employee);
            button2_Click(null, null);
            button7_Click(null, null);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox8.Text) || comboBox1.SelectedIndex == -1 ||
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
                Data = dateTimePicker1.Value.Date,
                Count = Convert.ToInt32(textBox8.Text),
                ClientId = client.Id,
                TripId = trip.Id,
                EmployeeId = employee.Id
            };

            _repositorySale.Insert(sale);
            button3_Click(null, null);
            button9_Click(null, null);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox9.Text) ||
                string.IsNullOrWhiteSpace(textBox11.Text) || string.IsNullOrWhiteSpace(textBox10.Text) ||
                comboBox4.SelectedIndex == -1 || comboBox5.SelectedIndex == -1 || comboBox6.SelectedIndex == -1)
            {
                MessageBox.Show("Вы должны ввести все данные!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var trip = new Trip
            {
                Name = textBox7.Text,
                Price = Convert.ToDecimal(textBox9.Text),
                DepartureDate = dateTimePicker2.Value.Date,
                CountNights = Convert.ToInt32(textBox11.Text),
                CountKids = Convert.ToInt32(textBox10.Text),
                Country = comboBox6.SelectedItem.ToString(),
                TypeTrip = comboBox5.SelectedItem.ToString(),
                Nutrition = comboBox4.SelectedItem.ToString()
            };

            _repositoryTrip.Insert(trip);
            button4_Click(null, null);
            button11_Click(null, null);
        }

        #region Clear

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            maskedTextBox1.Text = string.Empty;
            maskedTextBox2.Text = string.Empty;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox6.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox4.Text = string.Empty;
            maskedTextBox4.Text = string.Empty;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.Date;
            textBox8.Text = string.Empty;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox7.Text = string.Empty;
            textBox9.Text = string.Empty;
            dateTimePicker2.Value = DateTime.Now.Date;
            textBox11.Text = string.Empty;
            textBox10.Text = string.Empty;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
        }

        #endregion

        #endregion

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            switch (CurrentTable)
            {
                case CurrentTable.Client:
                    var client = _repositoryClient.GetById(id);
                    _repositoryClient.Delete(client);
                    button1_Click(this, null);
                    break;
                case CurrentTable.Employee:
                    var employee = _repositoryEmployee.GetById(id);
                    _repositoryEmployee.Delete(employee);
                    button2_Click(this, null);
                    break;
                case CurrentTable.Sale:
                    var sale = _repositorySale.GetById(id);
                    _repositorySale.Delete(sale);
                    button3_Click(this, null);
                    break;
                case CurrentTable.Trip:
                    var trip = _repositoryTrip.GetById(id);
                    _repositoryTrip.Delete(trip);
                    button4_Click(this, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            switch (CurrentTable)
            {
                case CurrentTable.Client:
                    var client = _repositoryClient.GetById(id);
                    var formChangeClient = new FormChangeClient(this, client, _repositoryClient);
                    formChangeClient.ShowDialog();
                    break;
                case CurrentTable.Employee:
                    var employee = _repositoryEmployee.GetById(id);
                    var formChangeEmployee = new FormChangeEmployee(this, employee, _repositoryEmployee);
                    formChangeEmployee.ShowDialog();
                    break;
                case CurrentTable.Sale:
                    var sale = _repositorySale.GetById(id);
                    var formChangeSale = new FormChangeSale(this, sale, _repositorySale, _repositoryClient, _repositoryTrip, _repositoryEmployee);
                    formChangeSale.ShowDialog();
                    break;
                case CurrentTable.Trip:
                    var trip = _repositoryTrip.GetById(id);
                    var formChangeTrip = new FormChangeTrip(this, trip, _repositoryTrip);
                    formChangeTrip.ShowDialog();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Word and Excel

        private void PrintIntoExcel(CurrentTable currentTable, params string[] namesColumns)
        {
            var application = new Excel.Application();
            var worksheet = (Excel.Worksheet)application.Workbooks.Add(Type.Missing).ActiveSheet;
            const int indexFirstLetter = 65;
            var nextLetter = Convert.ToChar(indexFirstLetter + namesColumns.Length - 1);
            var excelCells = worksheet.get_Range("A1", $"{nextLetter}1").Cells;
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.Interior.Color = Color.Gold;
            excelCells.Merge(Type.Missing);
            var nameTable = currentTable switch
            {
                CurrentTable.Client => "Клиенты",
                CurrentTable.Employee => "Сотрудники",
                CurrentTable.Sale => "Продажи",
                CurrentTable.Trip => "Туры",
                _ => throw new ArgumentOutOfRangeException()
            };

            worksheet.Cells[1, 1] = $"Табличны данные \"{nameTable}\"";
            for (var i = 0; i < namesColumns.Length; i++)
            {
                worksheet.Cells[2, i + 1] = namesColumns[i];
                worksheet.Cells[2, i + 1].HorizontalAlignment = Excel.Constants.xlCenter;
                worksheet.Columns[i + 1].ColumnWidth = 35;
            }

            switch (currentTable)
            {
                case CurrentTable.Client:
                    var clients = _repositoryClient.GetAll();
                    var listClients = clients.ToList();
                    for (var i = 0; i < listClients.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listClients[i].Surname;
                        application.Cells[i + 3, 2] = listClients[i].Name;
                        application.Cells[i + 3, 3] = listClients[i].Patronymic;
                        application.Cells[i + 3, 4] = listClients[i].Phone;
                        application.Cells[i + 3, 5] = listClients[i].Passport;
                    }
                    break;
                case CurrentTable.Employee:
                    var employees = _repositoryEmployee.GetAll();
                    var listEmployees = employees.ToList();
                    for (var i = 0; i < listEmployees.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listEmployees[i].Surname;
                        application.Cells[i + 3, 2] = listEmployees[i].Name;
                        application.Cells[i + 3, 3] = listEmployees[i].Patronymic;
                        application.Cells[i + 3, 4] = listEmployees[i].Phone;
                    }
                    break;
                case CurrentTable.Sale:
                    if (_repositorySale is not RepositorySale upcastRepository) return;
                    var sales = upcastRepository.GetAllIncludeForeignKey();
                    var listSales = sales.ToList();
                    for (var i = 0; i < listSales.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listSales[i].Data.ToShortDateString();
                        application.Cells[i + 3, 2] = listSales[i].Count.ToString();
                        application.Cells[i + 3, 3] = listSales[i].Client.Surname;
                        application.Cells[i + 3, 4] = listSales[i].Trip.Name;
                        application.Cells[i + 3, 5] = listSales[i].Employee.Surname;
                    }
                    break;
                case CurrentTable.Trip:
                    var trips = _repositoryTrip.GetAll();
                    var listTrips = trips.ToList();
                    for (var i = 0; i < listTrips.Count; i++)
                    {
                        application.Cells[i + 3, 1] = listTrips[i].Name;
                        application.Cells[i + 3, 2] = listTrips[i].Price.ToString("F");
                        application.Cells[i + 3, 3] = listTrips[i].DepartureDate.ToShortDateString();
                        application.Cells[i + 3, 4] = listTrips[i].CountNights;
                        application.Cells[i + 3, 5] = listTrips[i].CountKids;
                        application.Cells[i + 3, 6] = listTrips[i].Country;
                        application.Cells[i + 3, 7] = listTrips[i].TypeTrip;
                        application.Cells[i + 3, 8] = listTrips[i].Nutrition;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            application.Visible = true;
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Client, "Фамилия", "Имя", "Отчество", "Телефон", "Паспорт");
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Employee, "Фамилия", "Имя", "Отчество", "Телефон");
        }

        private void продажиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Sale, "Дата", "Количество", "Клиент", "Тур", "Сотрудник");
        }

        private void турыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintIntoExcel(CurrentTable.Trip, "Название", "Цена", "Дата отправления",
                "Кол-во ночей", "Кол-во детей", "Страна", "Тип тура", "Питание");
        }

        private void отчётПоПродажеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTable != CurrentTable.Sale)
            {
                MessageBox.Show("Выберите таблицу продаж!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Выберите строку таблицы!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var saleId = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            var sale = _repositorySale.GetById(saleId);
            var client = _repositoryClient.GetById(sale.ClientId);
            var trip = _repositoryTrip.GetById(sale.TripId);
            var employee = _repositoryEmployee.GetById(sale.EmployeeId);
            var application = new Word.Application
            {
                Visible = false
            };
            var path = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\ReportSale.docx");
            var document = application.Documents.Open(path);
            ReplaceData("{date}", DateTime.Now.ToShortDateString(), document);
            ReplaceData("{dateSale}", sale.Data.ToShortDateString(), document);
            ReplaceData("{count}", sale.Count.ToString(), document);
            ReplaceData("{client}", client.Surname + ' ' + client.Name + ' ' + client.Patronymic, document);
            ReplaceData("{employee}", employee.Surname + ' ' + employee.Name + ' ' + employee.Patronymic, document);
            ReplaceData("{trip}", trip.Name, document);
            ReplaceData("{dateDeparture}", trip.DepartureDate.ToShortDateString(), document);
            ReplaceData("{country}", trip.Country, document);
            document.SaveAs(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Documents\\ReportSaleResult.docx"));
            application.Visible = true;
        }

        private void ReplaceData(string target, string data, Word.Document documentMy)
        {
            var content = documentMy.Content;
            content.Find.ClearFormatting();
            content.Find.Execute(FindText: target, ReplaceWith: data);
        }

        #endregion

        #region Validation enter

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

        #endregion
    }
}
