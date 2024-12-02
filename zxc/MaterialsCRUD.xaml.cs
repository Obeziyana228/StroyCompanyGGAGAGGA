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
using zxc.ADO;
using System.Windows.Media.Media3D;

namespace zxc
{
    /// <summary>
    /// Логика взаимодействия для MaterialsCRUD.xaml
    /// </summary>
    public partial class MaterialsCRUD : Page
    {

        private string connectionString = "Server=DESKTOP-LQ2LR6H\\MSSQLSERVER01;Database=Строй Компания И закупка материалов;Integrated Security=True;";
        private DataTable materialsTable;

        public MaterialsCRUD()
        {
            InitializeComponent();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Materials", connection);
                materialsTable = new DataTable();
                adapter.Fill(materialsTable);
                MaterialsListView.ItemsSource = materialsTable.DefaultView;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Materials (Name_Maerials, Сategory, purchase_price, sale_price) VALUES (@Name, @Category, @purchase_price, @SalePrice)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", NameTextBox.Text);
                command.Parameters.AddWithValue("@Category", CategoryTextBox.Text);
                command.Parameters.AddWithValue("@purchase_price", decimal.Parse(PriceTextBox.Text));
                command.Parameters.AddWithValue("@SalePrice", decimal.Parse(SalePriceTextBox.Text));

                connection.Open();
                command.ExecuteNonQuery();
            }
            LoadMaterials();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsListView.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)MaterialsListView.SelectedItem;

                // Используйте правильное имя столбца
                int id = (int)selectedRow["ID_Materials"]; // Измените на ID_Materials, если это правильное имя

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Убедитесь, что здесь также используется правильное имя столбца
                    string query = "DELETE FROM Materials WHERE ID_Materials = @ID"; // Измените на ID_Materials
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                LoadMaterials();
            }
            else
            {
                MessageBox.Show("Выберите материал для удаления.");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}

