using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Autoservice
{
    public partial class AddAccept : Form
    {
        public AddAccept()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public int id_client;
        public int id_person;
        public string NameProd;
        public string Key;

        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        private void AddAccept_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;

            ComboLoadName();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Назад

            Acceptance acc = new Acceptance();
            acc.Key = this.Key;
            this.Hide();
            acc.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // Добавить

            Connection.Open();

            string request = $"USE ДоброДляАвто INSERT INTO [Приемо-сдаточный акт] (ID_Акта, ID_Клиента, ID_Модель, [Цвет модели], [Дата приема], [Дата сдачи], ID_Сотрудника) VALUES " +
                             $"((SELECT MAX(ID_Акта) + 1 FROM [Приемо-сдаточный акт]), {id_client}, (SELECT ID_Модель FROM Модель WHERE ID_Марка = (SELECT ID_Марка FROM Марка WHERE Название = '{comboBox1.Text}') AND Название = '{comboBox2.Text}'), " +
                             $"'{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', {id_person});";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            SqlCommand cmd = new SqlCommand(request, Connection);
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            string requestUp = $"USE ДоброДляАвто SELECT M.ID_Акта AS [Номер акта], K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество AS [ФИО Клиента], " +
                                                         $"(SELECT N.Название + ' ' + J.Название FROM Модель AS J JOIN Марка AS N ON J.ID_Марка = N.ID_Марка WHERE ID_Модель = M.ID_Модель) AS Авто, " +
                                                         $"M.[Цвет модели], M.[Дата приема], M.[Дата сдачи], " +
                                                         $"P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество AS [ФИО Сотрудника], " +
                                                         $"(SELECT SUM(Стоимость) FROM [Перечень работы] AS F JOIN Услуги AS B ON F.ID_Услуги = B.ID_Услуги WHERE ID_Акта = M.ID_Акта) AS [Стоимость услуг], " +
                                                         $"(SELECT SUM(Цена) FROM [Перечень запчастей] AS Z JOIN Запчасти AS V ON Z.ID_Запчасти = V.ID_Запчасти WHERE ID_Акта = M.ID_Акта) AS [Стоимость запчастей] " +
                                 $"FROM [Приемо-сдаточный акт] AS M " +
                                 $"JOIN Клиенты AS K ON K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество = (SELECT K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество WHERE K.ID_Клиента = M.ID_Клиента) " +
                                 $"JOIN Сотрудник AS P ON P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество = (SELECT P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество WHERE P.ID_Сотрудника = M.ID_Сотрудника) " +
                                 $"WHERE ID_Акта = (SELECT MAX(ID_Акта) FROM [Приемо-сдаточный акт]); ";

            SqlCommand commandUp = new SqlCommand(requestUp, Connection);
            commandUp.ExecuteNonQuery();
            SqlDataAdapter adapterUp = new SqlDataAdapter(commandUp);
            DataSet DB = new DataSet();
            adapterUp.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Выбор клиента

            AddClient AC = new AddClient();
            AC.ShowDialog();
            id_client = AC.id;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Выбор сотрудника

            AddPerson AP = new AddPerson();
            AP.ShowDialog();
            id_person = AP.id;
        }

        private void ComboLoadName()
        {
            string position = "USE ДоброДляАвто SELECT DISTINCT Название FROM Марка;";

            using (SqlCommand cmd = new SqlCommand(position, Connection))
            {
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBox1.DisplayMember = "Название";
                comboBox1.ValueMember = "ID_Марка";
                comboBox1.DataSource = table;
            }
        }

        private void ComboLoadModel()
        {
            string position = $"USE ДоброДляАвто SELECT DISTINCT Название FROM Модель AS O WHERE O.ID_Марка = (SELECT ID_Марка FROM Марка WHERE Название = '{comboBox1.Text}');";

            using (SqlCommand cmd = new SqlCommand(position, Connection))
            {
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBox2.DisplayMember = "Название";
                comboBox2.ValueMember = "ID_Марка";
                comboBox2.DataSource = table;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboLoadModel();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
