using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Autoservice
{
    public partial class AddPerson : Form
    {
        public AddPerson()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public int id;
        public string Key;

        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        private void AddPerson_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;

            // Автоматическая загрузка данных по всем

            Connection.Open();

            string request = $"USE ДоброДляАвто SELECT * FROM Сотрудник;";

            SqlCommand command = new SqlCommand(request, Connection);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet DB = new DataSet();
            adapter.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();

            // Автоматическая загрузка данных по всем
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;

            id = (int)dataGridView1.Rows[row].Cells[0].Value;
        }
    }
}
