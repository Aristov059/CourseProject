using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Autoservice
{
    public partial class Autorization : Form
    {
        public Autorization()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        public static string login;
        public static string password;

        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.MaxLength = 10;
            textBox2.MaxLength = 6;

            login = Convert.ToString(textBox1.Text);
            password = Convert.ToString(textBox2.Text);

            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Заполните данные!", "Ошибка", MessageBoxButtons.OK);
            }
            else
            {
                Connection.Open();

                string request = $"USE ДоброДляАвто SELECT Логин, Пароль " +
                                 $"FROM Сотрудник " +                                 
                                 $"WHERE Логин = '{login}' AND Пароль = '{password}'";

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable table = new DataTable();
                SqlCommand cmd = new SqlCommand(request, Connection);
                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                if (table.Rows.Count != 1)
                {
                   
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK);
                    
                }
                else if (table.Rows.Count == 1)
                {

                    Personality pers = new Personality();
                    pers.Key = login;
                    this.Hide();
                    pers.ShowDialog();
                }

                Connection.Close();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Обратитесь за помощью\n к администратору\n +79223145000", "Поддержка", MessageBoxButtons.OK);
        }

        private void Autorization_Load(object sender, EventArgs e)
        {

        }
    }
}
