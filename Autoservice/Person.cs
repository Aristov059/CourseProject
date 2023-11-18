using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Autoservice
{
    public partial class Person : Form
    {

        public Person()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public string Position;
        public string Key;
        
        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        private void Person_Load(object sender, EventArgs e)
        {
            ComboLoad();

            dataGridView1.ReadOnly = true;

            if (Position == "Директор")
            {
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }
            else if (Position == "Администратор")
            {
                button4.Enabled = false;
                button5.Enabled = true;
                button6.Enabled = false;

                button4.BackColor = Color.Gray;
                button6.BackColor = Color.Gray;
            }

            textBox2.MaxLength = 20;
            textBox3.MaxLength = 20;
            textBox4.MaxLength = 20;
            textBox5.MaxLength = 4;
            textBox6.MaxLength = 6;
            textBox7.MaxLength = 12;
            textBox8.MaxLength = 80;
            textBox9.MaxLength = 20;
            textBox10.MaxLength = 20;

            // Автоматическая загрузка данных по всем

                Connection.Open();

                string request = $"USE ДоброДляАвто SELECT Фамилия, Имя, Отчество, [Серия паспорта], " +
                                 $"[Номер паспорта], [Номер телефона], Адрес, Логин, Пароль, p.Название " +
                                 $"FROM Сотрудник AS m " +
                                 $"JOIN Должность AS p ON m.ID_Должность = p.ID_Должность;";

                SqlCommand command = new SqlCommand(request, Connection);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet DB = new DataSet();
                adapter.Fill(DB);
                dataGridView1.DataSource = DB.Tables[0];

                Connection.Close();

            // Автоматическая загрузка данных по всем
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Вывести всех

            // Кнопка для обратного просмотра данных по всем

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT Фамилия, Имя, Отчество, [Серия паспорта], " +
                             $"[Номер паспорта], [Номер телефона], Адрес, Логин, Пароль, p.Название " +
                             $"FROM Сотрудник AS m " +
                             $"JOIN Должность AS p ON m.ID_Должность = p.ID_Должность;";

            SqlCommand command = new SqlCommand(request, Connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet DB = new DataSet();
            adapter.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Назад

            Personality pers = new Personality();
            pers.Key = Key;
            this.Hide();
            pers.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Найти определнного человека

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT Фамилия, Имя, Отчество, [Серия паспорта], " +
                             $"[Номер паспорта], [Номер телефона], Адрес, Логин, Пароль, p.Название " +
                             $"FROM Сотрудник AS m " +
                             $"JOIN Должность AS p ON m.ID_Должность = p.ID_Должность " +
                             $"WHERE Фамилия + ' ' + Имя + ' ' + Отчество = '{textBox1.Text}' " +
                             $"OR Фамилия + ' ' + Имя = '{textBox1.Text}' " +
                             $"OR Фамилия = '{textBox1.Text}';";

            SqlCommand command = new SqlCommand(request, Connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet DB = new DataSet();
            adapter.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Обновление данных в БД

            if (textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox4.Text.Length == 0 || textBox5.Text.Length == 0
                || textBox6.Text.Length == 0 || textBox7.Text.Length == 0 || textBox8.Text.Length == 0 || textBox9.Text.Length == 0
                || textBox10.Text.Length == 0 || comboBox1.Text.Length == 0)
            {
                MessageBox.Show("Заполните все данные!", "Ошибка", MessageBoxButtons.OK);
            }
            else
            {
                Connection.Open();

                string request = $"USE ДоброДляАвто UPDATE Сотрудник SET Фамилия = '{textBox2.Text}', " +
                                 $"Имя = '{textBox3.Text}', Отчество = '{textBox4.Text}', [Серия паспорта] = '{textBox5.Text}', " +
                                 $"[Номер паспорта] = '{textBox6.Text}', [Номер телефона] = '{textBox7.Text}', " +
                                 $"Адрес = '{textBox8.Text}', Пароль = '{textBox10.Text}', " +
                                 $"ID_Должность = (SELECT p.ID_Должность FROM Должность AS p WHERE p.Название = '{comboBox1.Text}') " +
                                 $"WHERE Логин = '{textBox9.Text}';";

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                SqlCommand cmd = new SqlCommand(request, Connection);
                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                Connection.Close();

                MessageBox.Show("Изменение внесены успешно!", "Выполненно", MessageBoxButtons.OK);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Добавление данных в БД

            if (textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox4.Text.Length == 0 || textBox5.Text.Length == 0
                || textBox6.Text.Length == 0 || textBox7.Text.Length == 0 || textBox8.Text.Length == 0 || textBox9.Text.Length == 0
                || textBox10.Text.Length == 0 || comboBox1.Text.Length == 0)
            {
                MessageBox.Show("Заполните все данные!", "Ошибка", MessageBoxButtons.OK);
            }
            else
            {
                Connection.Open();

                string requestCheck = $"USE ДоброДляАвто SELECT Логин, [Серия паспорта], [Номер паспорта], [Номер телефона] " +
                                      $"FROM Сотрудник " +
                                      $"WHERE Логин = '{textBox9.Text}' " +
                                      $"OR [Серия паспорта] = '{textBox5.Text}' AND [Номер паспорта] = '{textBox6.Text}' " +
                                      $"OR [Номер телефона] = '{textBox7.Text}';";


                SqlDataAdapter adapterCheck = new SqlDataAdapter();
                DataTable table1 = new DataTable();
                SqlCommand cmd1 = new SqlCommand(requestCheck, Connection);
                adapterCheck.SelectCommand = cmd1;
                adapterCheck.Fill(table1);

                if (table1.Rows.Count == 1)
                {
                    MessageBox.Show("Новые данные совпадают с добавленными!", "Ошибка", MessageBoxButtons.OK);

                    Connection.Close();
                }
                else if (table1.Rows.Count != 1)
                {
                    string requestAdd = $"USE ДоброДляАвто " +
                                        $"INSERT INTO Сотрудник " +
                                        $"(ID_Сотрудника, Фамилия, Имя, Отчество, [Серия паспорта], " +
                                        $"[Номер паспорта], [Номер телефона], Адрес, " +
                                        $"ID_Должность, Логин, Пароль) " +
                                        $"VALUES " +
                                        $"((SELECT MAX(ID_Сотрудника) + 1 FROM Сотрудник), " +
                                        $"'{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}', " +
                                        $"'{textBox5.Text}', '{textBox6.Text}', '{textBox7.Text}', " +
                                        $"'{textBox8.Text}', " +
                                        $"(SELECT ID_Должность FROM Должность WHERE Название = '{comboBox1.Text}'), " +
                                        $"'{textBox9.Text}', '{textBox10.Text}');";

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();

                    SqlCommand cmd = new SqlCommand(requestAdd, Connection);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(table);

                    MessageBox.Show("Сотрудник добавлен!", "Выполненно", MessageBoxButtons.OK);
                }

                Connection.Close();
            }
        }

        private void ComboLoad()
        {
            string position = "USE ДоброДляАвто SELECT * FROM Должность;";

            using (SqlCommand cmd = new SqlCommand(position, Connection))
            {
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBox1.DisplayMember = "Название";
                comboBox1.ValueMember = "ID_Должность";
                comboBox1.DataSource = table;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();

            int row = dataGridView1.CurrentRow.Index;

            for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
            {
                this.Controls["textBox" + (i + 2).ToString()].Text += dataGridView1.Rows[row].Cells[i].Value;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Удаление данных из БД

            Connection.Open();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Вы точно хотите удалить пользователя?", "Подтверждение", buttons);

            if(result == System.Windows.Forms.DialogResult.Yes)
            {
                string request = $"USE ДоброДляАвто DELETE FROM Сотрудник WHERE Логин = '{textBox9.Text}';";

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                SqlCommand cmd = new SqlCommand(request, Connection);
                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                MessageBox.Show("Сотрудник удален!", "Выполненно", MessageBoxButtons.OK);
            }

            Connection.Close();
        }
    }
}
