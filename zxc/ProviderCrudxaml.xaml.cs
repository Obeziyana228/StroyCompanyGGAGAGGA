using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для ProviderCrudxaml.xaml
    /// </summary>
    public partial class ProviderCrudxaml : Page
    {
        private string connectionString = "Server=sql-ser-larisa\\serv1215;Database=Строй Компания И закупка материалов;Integrated Security=True;";
        private DataTable ProviderTable;

        public ProviderCrudxaml()
        {
            InitializeComponent();
            LoadProvider();
        }

        private void LoadProvider()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Provider", connection);
                ProviderTable = new DataTable();
                adapter.Fill(ProviderTable);

                ProvidersListView.ItemsSource = ProviderTable.DefaultView;
            }
        }

        private void DobavitButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Provider (Name_Postavshik, reliability, phone) VALUES (@Name_Postavshik, @reliability, @phone)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name_Postavshik", Name_Postavshik.Text);
                command.Parameters.AddWithValue("@reliability", reliability.Text);
                command.Parameters.AddWithValue("@phone", phone.Text);

                connection.Open();
                command.ExecuteNonQuery();
            }
            LoadProvider();
        }//z gbljh

        private void UdaltButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProvidersListView.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ProvidersListView.SelectedItem;
                int id = (int)selectedRow["ID_Postavshik"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Provider WHERE ID_Postavshik = @ID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                LoadProvider();
            }
            else
            {
                MessageBox.Show("Выберите Поставщика для удаления.");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
