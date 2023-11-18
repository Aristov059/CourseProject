using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Autoservice
{
    public partial class Personality : Form
    {
        public Personality()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public string Key;
        public string FIO;
        public string Position; 

        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        private void Personality_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 20;
            textBox2.MaxLength = 20;
            textBox3.MaxLength = 20;
            textBox7.MaxLength = 35;

            string fullData = $"USE ДоброДляАвто SELECT Фамилия, Имя, Отчество, [Серия паспорта], " +
                              $"[Номер паспорта], p.Название, [Номер телефона], Адрес " +
                              $"FROM Сотрудник AS s " +
                              $"JOIN Должность AS p ON s.ID_Должность = p.ID_Должность " +
                              $"WHERE Логин = '{Key}'; ";

            using (SqlCommand cmd = new SqlCommand(fullData, Connection))
            {
                Connection.Open();
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Position = reader[5].ToString();
                    label1.Text = Position;
                }
                Connection.Close();
            }

            for (int i = 0; i <= 2; i++)
            {
                using (SqlCommand cmd = new SqlCommand(fullData, Connection))
                {
                    Connection.Open();
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        FIO += reader[i].ToString() + " ";
                    }
                    Connection.Close();
                }
            }

            label2.Text = FIO;

            for (int i = 1; i <= 8; i++)
            {
                using (SqlCommand cmd = new SqlCommand(fullData, Connection))
                {
                    Connection.Open();
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        this.Controls["textBox" + i.ToString()].Text = reader[i - 1].ToString();
                    }
                    Connection.Close();
                }
            }

            if (Position == "Мастер")
            {
                button4.Enabled = false;

                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;

                button4.BackColor = Color.Gray;
                textBox4.BackColor = Color.Gray;
                textBox5.BackColor = Color.Gray;
                textBox6.BackColor = Color.Gray;
            }

            textBox6.ReadOnly = true;
            textBox6.BackColor = Color.Gray;
        }

        bool ValidNumber(string Number)
        {
            string pattern = "^(\\+7|8)[0-9]{10}$";
            Match isMatch = Regex.Match(Number, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Приемо-сдаточный акт

            Acceptance accept = new Acceptance();
            accept.Key = this.Key;
            accept.Position = this.Position;
            this.Hide();
            accept.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Клиенты

            Client client = new Client();
            client.Key = this.Key;
            client.Position = this.Position;
            this.Hide();
            client.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Запчасти

            Spares spares = new Spares();
            spares.Key = this.Key;
            spares.Position = this.Position;
            this.Hide();
            spares.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Персонал

            Person person = new Person();
            person.Key = this.Key;
            person.Position = this.Position;
            this.Hide();
            person.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        { 
            // Выйти

            Autorization auto = new Autorization();
            this.Hide();
            auto.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Услуги

            Services service = new Services();
            service.Key = this.Key;
            service.Position = this.Position;
            this.Hide();
            service.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string Number = textBox7.Text;

            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0 ||
                textBox7.Text.Length == 0 || textBox8.Text.Length == 0)
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (ValidNumber(Number))
                {
                    Connection.Open();

                    string request = $"USE ДоброДляАвто UPDATE Сотрудник SET Фамилия = '{textBox1.Text}', " +
                                     $"Имя = '{textBox2.Text}', Отчество = '{textBox3.Text}', " +
                                     $"[Номер телефона] = '{textBox7.Text}', Адрес = '{textBox8.Text}' " +
                                     $"WHERE Логин = '{Key}';";

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();

                    SqlCommand cmd = new SqlCommand(request, Connection);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(table);

                    MessageBox.Show("Изменение внесены успешно!", "Выполненно", MessageBoxButtons.OK);

                    Connection.Close();
                }
                else
                {
                    MessageBox.Show("Неверный номер телефона!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }   
            }          
        }
    }
}
