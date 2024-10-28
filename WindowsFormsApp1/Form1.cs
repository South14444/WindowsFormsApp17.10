using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration.Provider;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DbConnection connection;
        private SqlDataAdapter adapter;
        private DataTable table;
        private DataGridView dataGridView;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private async void ConnectToDataBaseAsync(object sender, EventArgs e)
        {
            try
            {
                var factory = DbProviderFactories.GetFactory(comboBox1.SelectedItem.ToString());
                connection = factory.CreateConnection();
                connection.ConnectionString = comboBox2.SelectedItem.ToString();
                await connection.OpenAsync();
                Text = connection.State.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DisconnectToDataBase(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                Text = connection.State.ToString();
            }
            else { MessageBox.Show("Не удалось закрыть подключение\nПодключение было уже закрыто"); }
        }
        private async Task UpdateDataAsync()
        {
            try
            {
                int rowsAffected = adapter.Update(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }
        private async void GetProviders(object sender, EventArgs e)
        {
            var factoryCollection = DbProviderFactories.GetFactoryClasses();
            comboBox1.Items.Clear();
            foreach (System.Data.DataRow row in factoryCollection.Rows)
            {
                comboBox1.Items.Add($"{row[2]}");
            }
            var strings = ConfigurationManager.ConnectionStrings;
            for (int i = 0; i < strings.Count; i++)
            {
                comboBox2.Items.Add(strings[i].ConnectionString);
            }
        }
        private async void UpdateRowsButtonAsync(object sender, EventArgs e)
        {
            DateTime t1 = DateTime.Now;
            await UpdateDataAsync();
            Text = "Время выполнения запроса: "+(DateTime.Now - t1).Milliseconds.ToString();
        }

        private async void SelectCofeAsync(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = "select * from CofeInfo";
            command.Connection = (SqlConnection)connection;
            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                if(reader.HasRows)
                {
                    DataTable table = new DataTable();
                    for(int i=0;i<reader.FieldCount;i++)
                    {
                        table.Columns.Add(reader.GetName(i));
                    }
                    while(reader.Read())
                    {
                        var row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.GetValue(i).ToString();
                        }
                        table.Rows.Add(row);
                    }
                    table.AcceptChanges();
                    dataGridView1.DataSource = table;
                }
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            table.Rows[e.RowIndex].AcceptChanges();
        }
    }
}
