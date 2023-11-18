using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Autoservice
{
    public partial class AddServices : Form
    {
        public AddServices()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        }

        public int id_act;

        public static string Con = @"Data Source=DESKTOP-VLO3SHC\MYSQL;Initial Catalog=ДоброДляАвто;Integrated Security=True";
        SqlConnection Connection = new SqlConnection(Con);

        private void AddServices_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;

            ComboLoadName();

            Connection.Open();

            string requestUpdate = $"USE ДоброДляАвто SELECT A.ID_Акта, O.Название FROM [Перечень работы] AS A " +
                                   $"JOIN Услуги AS O ON O.Название = (SELECT O.Название WHERE O.ID_Услуги = A.ID_Услуги) " +
                                   $"WHERE ID_Акта = '{id_act}';";

            SqlCommand commandUpdate = new SqlCommand(requestUpdate, Connection);
            commandUpdate.ExecuteNonQuery();
            SqlDataAdapter adapterUpdate = new SqlDataAdapter(commandUpdate);
            DataSet DB = new DataSet();
            adapterUpdate.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Назад

            AboutAccept acc = new AboutAccept();
            this.Hide();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Добавить данные в БД

            Connection.Open();

            string request = $"USE ДоброДляАвто INSERT INTO [Перечень работы] " +
                             $"(ID_Акта, ID_Услуги) " +
                             $"VALUES " +
                             $"('{id_act}', " +
                             $"(SELECT ID_Услуги FROM Услуги WHERE Название = '{comboBox1.Text}'));";

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            SqlCommand cmd = new SqlCommand(request, Connection);
            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            string requestUpdate = $"USE ДоброДляАвто SELECT A.ID_Акта, O.Название FROM [Перечень работы] AS A " +
                                   $"JOIN Услуги AS O ON O.Название = (SELECT O.Название WHERE O.ID_Услуги = A.ID_Услуги) " +
                                   $"WHERE ID_Акта = '{id_act}';";

            SqlCommand commandUpdate = new SqlCommand(requestUpdate, Connection);
            commandUpdate.ExecuteNonQuery();
            SqlDataAdapter adapterUpdate = new SqlDataAdapter(commandUpdate);
            DataSet DB = new DataSet();
            adapterUpdate.Fill(DB);
            dataGridView1.DataSource = DB.Tables[0];

            Connection.Close();
        }

        private void ComboLoadName()
        {
            string position = "USE ДоброДляАвто SELECT DISTINCT Название FROM Услуги;";

            using (SqlCommand cmd = new SqlCommand(position, Connection))
            {
                cmd.CommandType = CommandType.Text;
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                comboBox1.DisplayMember = "Название";
                comboBox1.ValueMember = "ID_Услуги";
                comboBox1.DataSource = table;
            }
        }
    }
}
