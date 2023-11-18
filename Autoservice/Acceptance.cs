using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Autoservice
{
    public partial class Acceptance : Form
    {
        public Acceptance()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public int id;
        public string Key;
        public string Position;

        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        private void Acceptance_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;

            if (Position == "Директор")
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else if (Position == "Администратор")
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else if (Position == "Мастер")
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }

            // Автоматическая загрузка данных по всем

                Connection.Open();

                string request = $"USE ДоброДляАвто SELECT M.ID_Акта AS [Номер акта], K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество AS [ФИО Клиента], " +
                                                         $"(SELECT N.Название + ' ' + J.Название FROM Модель AS J JOIN Марка AS N ON J.ID_Марка = N.ID_Марка WHERE ID_Модель = M.ID_Модель) AS Авто, " +
                                                         $"M.[Цвет модели], M.[Дата приема], M.[Дата сдачи], " +
                                                         $"P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество AS [ФИО Сотрудника], " +
                                                         $"(SELECT SUM(Стоимость) FROM [Перечень работы] AS F JOIN Услуги AS B ON F.ID_Услуги = B.ID_Услуги WHERE ID_Акта = M.ID_Акта) AS [Стоимость услуг], " +
                                                         $"(SELECT SUM(Цена) FROM [Перечень запчастей] AS Z JOIN Запчасти AS V ON Z.ID_Запчасти = V.ID_Запчасти WHERE ID_Акта = M.ID_Акта) AS [Стоимость запчастей] " +
                                 $"FROM [Приемо-сдаточный акт] AS M " +
                                 $"JOIN Клиенты AS K ON K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество = (SELECT K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество WHERE K.ID_Клиента = M.ID_Клиента) " +
                                 $"JOIN Сотрудник AS P ON P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество = (SELECT P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество WHERE P.ID_Сотрудника = M.ID_Сотрудника) ";

                SqlCommand command = new SqlCommand(request, Connection);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet DB = new DataSet();
                adapter.Fill(DB);
                dataGridView1.DataSource = DB.Tables[0];

                Connection.Close();

            // Автоматическая загрузка данных по всем

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;

            id = (int)dataGridView1.Rows[row].Cells[0].Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Назад

            Personality pers = new Personality();
            pers.Key = Key;
            this.Hide();
            pers.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Все данные об актах

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT M.ID_Акта AS [Номер акта], K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество AS [ФИО Клиента], " +
                                                         $"(SELECT N.Название + ' ' + J.Название FROM Модель AS J JOIN Марка AS N ON J.ID_Марка = N.ID_Марка WHERE ID_Модель = M.ID_Модель) AS Авто, " +
                                                         $"M.[Цвет модели], M.[Дата приема], M.[Дата сдачи], " +
                                                         $"P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество AS [ФИО Сотрудника], " +
                                                         $"(SELECT SUM(Стоимость) FROM [Перечень работы] AS F JOIN Услуги AS B ON F.ID_Услуги = B.ID_Услуги WHERE ID_Акта = M.ID_Акта) AS [Стоимость услуг], " +
                                                         $"(SELECT SUM(Цена) FROM [Перечень запчастей] AS Z JOIN Запчасти AS V ON Z.ID_Запчасти = V.ID_Запчасти WHERE ID_Акта = M.ID_Акта) AS [Стоимость запчастей] " +
                                 $"FROM [Приемо-сдаточный акт] AS M " +
                                 $"JOIN Клиенты AS K ON K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество = (SELECT K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество WHERE K.ID_Клиента = M.ID_Клиента) " +
                                 $"JOIN Сотрудник AS P ON P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество = (SELECT P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество WHERE P.ID_Сотрудника = M.ID_Сотрудника);";

            SqlCommand command = new SqlCommand(request, Connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet DB = new DataSet();
            adapter.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Определенный акт

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT M.ID_Акта AS [Номер акта], K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество AS [ФИО Клиента], " +
                                                         $"(SELECT N.Название + ' ' + J.Название FROM Модель AS J JOIN Марка AS N ON J.ID_Марка = N.ID_Марка WHERE ID_Модель = M.ID_Модель) AS Авто, " +
                                                         $"M.[Цвет модели], M.[Дата приема], M.[Дата сдачи], " +
                                                         $"P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество AS [ФИО Сотрудника], " +
                                                         $"(SELECT SUM(Стоимость) FROM [Перечень работы] AS F JOIN Услуги AS B ON F.ID_Услуги = B.ID_Услуги WHERE ID_Акта = M.ID_Акта) AS [Стоимость услуг], " +
                                                         $"(SELECT SUM(Цена) FROM [Перечень запчастей] AS Z JOIN Запчасти AS V ON Z.ID_Запчасти = V.ID_Запчасти WHERE ID_Акта = M.ID_Акта) AS [Стоимость запчастей] " +
                                 $"FROM [Приемо-сдаточный акт] AS M " +
                                 $"JOIN Клиенты AS K ON K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество = (SELECT K.Фамилия + ' ' + K.Имя + ' ' + K.Отчество WHERE K.ID_Клиента = M.ID_Клиента) " +
                                 $"JOIN Сотрудник AS P ON P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество = (SELECT P.Фамилия + ' ' + P.Имя + ' ' + P.Отчество WHERE P.ID_Сотрудника = M.ID_Сотрудника) " +
                                 $"WHERE ID_Акта = {textBox1.Text}; ";

            SqlCommand command = new SqlCommand(request, Connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet DB = new DataSet();
            adapter.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Добавить данные в БД

            AddAccept add = new AddAccept();
            add.Key = Key;
            this.Hide();
            add.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Подробный просмотр или обновить

            AboutAccept about = new AboutAccept();
            about.Key = Key;
            about.id = id;
            this.Hide();
            about.ShowDialog();
        }
    }
}
