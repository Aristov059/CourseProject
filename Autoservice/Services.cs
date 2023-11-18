using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Autoservice
{
    public partial class Services : Form
    {
        public Services()
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

        private void Services_Load(object sender, EventArgs e)
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
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;

                textBox2.Enabled = false;
                textBox3.Enabled = false;

                button4.BackColor = Color.Gray;
                button5.BackColor = Color.Gray;
                button6.BackColor = Color.Gray;
                button7.BackColor = Color.Gray;
            }

            // Автоматическая загрузка данных по всем

                Connection.Open();

                string request = $"USE ДоброДляАвто SELECT * FROM Услуги;";

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
            textBox2.Clear();
            textBox3.Clear();

            int row = dataGridView1.CurrentRow.Index;

            id = (int)dataGridView1.Rows[row].Cells[0].Value;

            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                this.Controls["textBox" + (i + 1).ToString()].Text += dataGridView1.Rows[row].Cells[i].Value;
            }
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
            // Вывести все услуги

            // Кнопка для обратного просмотра данных по всем

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT * FROM Услуги;";

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
            // Найти определенную услугу

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT ID_Услуги, Название, Стоимость FROM Услуги " +
                             $"WHERE Название = '{textBox1.Text}';";

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

            Connection.Open();

            string request = $"USE ДоброДляАвто UPDATE Услуги SET Название = '{textBox2.Text}', " +
                             $"Стоимость = '{textBox3.Text}' " +
                             $"WHERE ID_Услуги = '{id}'";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            SqlCommand cmd = new SqlCommand(request, Connection);
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            Connection.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Добавление данных в БД

            if (textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
            {
                MessageBox.Show("Заполните все данные!", "Ошибка", MessageBoxButtons.OK);
            }
            else
            {
                Connection.Open();

                string requestCheck = $"USE ДоброДляАвто SELECT * FROM Услуги " +
                                      $"WHERE ID_Услуги = '{id}' OR Название = '{textBox2.Text}';";

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

                    string request = $"USE ДоброДляАвто " +
                                     $"INSERT INTO Услуги " +
                                     $"(ID_Услуги, Название, Стоимость) " +
                                     $"VALUES " +
                                     $"((SELECT MAX(ID_Услуги) + 1 FROM Услуги), '{textBox2.Text}', '{textBox3.Text}');";

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable table = new DataTable();

                    SqlCommand cmd = new SqlCommand(request, Connection);
                    adapter.SelectCommand = cmd;
                    adapter.Fill(table);

                    MessageBox.Show("Услуга добавлена!", "Выполненно", MessageBoxButtons.OK);

                    Connection.Close();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Удаление данных из БД

            Connection.Open();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Вы точно хотите удалить данные по услуге?", "Подтверждение", buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                string request = $"USE ДоброДляАвто DELETE FROM Услуги WHERE ID_Услуги = '{id}';";

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();

                SqlCommand cmd = new SqlCommand(request, Connection);
                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                MessageBox.Show("Услуга удалена!", "Выполненно", MessageBoxButtons.OK);
            }

            Connection.Close();
        }
    }
}
