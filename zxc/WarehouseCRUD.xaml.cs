using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zxc
{
    /// <summary>
    /// Логика взаимодействия для WarehouseCRUD.xaml
    /// </summary>
    public partial class WarehouseCRUD : Page
    {

        private string connectionString = "Server=sql-ser-larisa\\serv1215;Database=Строй Компания И закупка материалов;Integrated Security=True;";
        private DataTable WarehouseTable;

        public WarehouseCRUD()
        {
            InitializeComponent();
            LoadWarehouse();
        }

        private void LoadWarehouse()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Warehouse", connection);
                WarehouseTable = new DataTable();
                adapter.Fill(WarehouseTable);
                WarehouseListView.ItemsSource = WarehouseTable.DefaultView;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Warehouse (ID_warehouse, ID_material, Quantity_of_material, Adress) VALUES (@ID_warehouse, @ID_material, @Quantity_of_material, @Adress)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_warehouse", ID_Sklad.Text);
                command.Parameters.AddWithValue("@ID_material", ID_Material.Text);
                command.Parameters.AddWithValue("@Quantity_of_material",Quantity_of_material.Text);
                command.Parameters.AddWithValue("@Adress", Adress.Text);

                connection.Open();
                command.ExecuteNonQuery();
            }
            LoadWarehouse();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (WarehouseListView.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)WarehouseListView.SelectedItem;
                int id = (int)selectedRow["ID_warehouse"]; 

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Warehouse WHERE ID_warehouse = @ID"; 
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                LoadWarehouse();
            }
            else
            {
                MessageBox.Show("Выберите Склад для удаления.");
            }
        }
    }
}
