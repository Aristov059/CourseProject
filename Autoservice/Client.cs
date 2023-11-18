using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Autoservice
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        public string Position;
        public string Key;
        public int id;

        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        private void Client_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;

            if (Position == "Директор")
            {
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
            }
            else if (Position == "Администратор")
            {
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
            }
            else if (Position == "Мастер")
            {
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = false;

                button7.BackColor = Color.Gray;
            }

            textBox2.MaxLength = 20;
            textBox3.MaxLength = 20;
            textBox4.MaxLength = 20;
            textBox5.MaxLength = 35;
            textBox6.MaxLength = 80;
            textBox7.MaxLength = 9;

            // Автоматическая загрузка данных по всем

            Connection.Open();

                string request = $"USE ДоброДляАвто SELECT ID_Клиента, Фамилия, Имя, Отчество, [Номер телефона], Адрес, [ГОС номер] " +
                                 $"FROM Клиенты; ";

                SqlCommand command = new SqlCommand(request, Connection);
                command.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet DB = new DataSet();
                adapter.Fill(DB);
                dataGridView1.DataSource = DB.Tables[0];

                Connection.Close();

            // Автоматическая загрузка данных по всем
        }

        bool ValidNumber(string Number)
        {
            string pattern = "^(\\+7|8)[0-9]{10}$";
            Match isMatch = Regex.Match(Number, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Вывести всех клиентов

            // Кнопка для обратного просмотра данных по всем

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT ID_Клиента, Фамилия, Имя, Отчество, [Номер телефона], Адрес, [ГОС номер] " +
                             $"FROM Клиенты; ";

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
            // Найти определенного клиента

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT ID_Клиента, Фамилия, Имя, Отчество, " +
                             $"[Номер телефона], Адрес, [ГОС номер] " +
                             $"FROM Клиенты " +
                             $"WHERE Фамилия + ' ' + Имя + ' ' + Отчество = '{textBox1.Text}' " +
                             $"OR Фамилия + ' ' + Имя = '{textBox1.Text}' " +
                             $"OR Фамилия = '{textBox1.Text}' " +
                             $"OR [ГОС номер] = '{textBox1.Text}';";

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
            // Обновление данных в БД

            if (textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox4.Text.Length == 0 
                || textBox5.Text.Length == 0 || textBox7.Text.Length == 0)
            {
                MessageBox.Show("Заполните все данные!", "Ошибка", MessageBoxButtons.OK);
            }
            else
            {
                string Number = textBox5.Text;

                if (ValidNumber(Number))
                {
                    Connection.Open();

                    string request = $"USE ДоброДляАвто UPDATE Клиенты SET Фамилия = '{textBox2.Text}', " +
                                     $"Имя = '{textBox3.Text}', Отчество = '{textBox4.Text}', " +
                                     $"[Номер телефона] = '{textBox5.Text}', Адрес = '{textBox6.Text}', " +
                                     $"[ГОС номер] = '{textBox7.Text}' " +
                                     $"WHERE ID_Клиента = '{id.ToString()}';";

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();

                    SqlCommand cmd = new SqlCommand(request, Connection);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(table); 

                    Connection.Close();

                    MessageBox.Show("Изменение внесены успешно!", "Выполненно", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Неверный номер телефона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

            int row = dataGridView1.CurrentRow.Index;

            id = (int)dataGridView1.Rows[row].Cells[0].Value;

            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                this.Controls["textBox" + (i + 1).ToString()].Text += dataGridView1.Rows[row].Cells[i].Value;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Добавление данных в БД

            if (textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox4.Text.Length == 0
                || textBox5.Text.Length == 0 || textBox7.Text.Length == 0)
            {
                MessageBox.Show("Заполните все данные!", "Ошибка", MessageBoxButtons.OK);
            }
            else
            {
                string Number = textBox5.Text;

                if (ValidNumber(Number))
                {
                    Connection.Open();

                    string requestCheck = $"USE ДоброДляАвто SELECT ID_Клиента, Фамилия, Имя, Отчество, [Номер телефона], Адрес, [ГОС номер] " +
                                          $"FROM Клиенты " +
                                          $"WHERE [Номер телефона] = '{textBox5.Text}' " +
                                          $"OR [ГОС номер] = '{textBox7.Text}';";

                    SqlDataAdapter adapterCheck = new SqlDataAdapter();
                    DataTable tableCheck = new DataTable();
                    SqlCommand cmdCheck = new SqlCommand(requestCheck, Connection);
                    adapterCheck.SelectCommand = cmdCheck;
                    adapterCheck.Fill(tableCheck);

                    if (tableCheck.Rows.Count == 1)
                    {
                        MessageBox.Show("Новые данные совпадают с добавленными!", "Ошибка", MessageBoxButtons.OK);

                        Connection.Close();
                    }
                    else if (tableCheck.Rows.Count != 1)
                    {
                        string requestAdd = $"USE ДоброДляАвто " +
                                            $"INSERT INTO Клиенты " +
                                            $"(ID_Клиента, Фамилия, Имя, Отчество, [Номер телефона], Адрес, [ГОС номер])" +
                                            $"VALUES " +
                                            $"((SELECT MAX(ID_Клиента) + 1 FROM Клиенты), '{textBox2.Text}', " +
                                            $"'{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', '{textBox6.Text}', " +
                                            $"'{textBox7.Text}');";

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        SqlCommand cmd = new SqlCommand(requestAdd, Connection);
                        adapter.SelectCommand = cmd;
                        adapter.Fill(table);

                        MessageBox.Show("Клиент добавлен!", "Выполненно", MessageBoxButtons.OK);

                        Connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный номер телефона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Удаление данных из БД

            Connection.Open();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Вы точно хотите удалить клиента?", "Подтверждение", buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                string request = $"USE ДоброДляАвто DELETE FROM Клиенты WHERE ID_Клиента = '{id}';";

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                SqlCommand cmd = new SqlCommand(request, Connection);
                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                MessageBox.Show("Клиент удален!", "Выполненно", MessageBoxButtons.OK);
            }

            Connection.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }
    }
}
