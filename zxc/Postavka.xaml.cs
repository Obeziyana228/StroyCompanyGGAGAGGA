using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace zxc
{
    /// <summary>
    /// Логика взаимодействия для Reestr.xaml
    /// </summary>
    public partial class Reestr : Page
    {
        private string connectionString = "Server = DESKTOP-LQ2LR6H\\MSSQLSERVER01;Database=Строй Компания И закупка материалов;Integrated Security=True;";
        public Reestr()
        {
            InitializeComponent();
            Quantity_of_materials.IsEnabled = false;
            LoadData();
        }
        private int selectedMaterialId = -1; // Для выбранного материала
        private int selectedWarehouseId = -1; // Для выбранного склада
        private int selectedProviderId = -1; // Для выбранного поставщика
        private void ProvidersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProvidersListView.SelectedItem != null)
            {
                var selectedProvider = ProvidersListView.SelectedItem as Provider;
                selectedProviderId = selectedProvider.ID_Postavshik;
            }
            else
            {
                selectedProviderId = -1; // Если ничего не выбрано, сбрасываем ID
                MessageBox.Show("Поставщик не выбран.");
            }
        }

        private void LoadProviders()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Provider", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Provider> providers = new List<Provider>();
                        while (reader.Read())
                        {
                            providers.Add(new Provider
                            {
                                ID_Postavshik = Convert.ToInt32(reader["ID_Postavshik"]),
                                Name_Postavshik = reader["Name_Postavshik"].ToString(),
                                reliability = reader["reliability"].ToString(),
                                phone = reader["phone"].ToString()
                            });
                        }
                        ProvidersListView.ItemsSource = providers; // Обновляем источник 
                    }
                }
            }
        }

        private void LoadSupplyOrders()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Supply_order", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<SupplyOrder> supplyOrders = new List<SupplyOrder>();
                        while (reader.Read())
                        {
                            supplyOrders.Add(new SupplyOrder
                            {
                                ID_supply = Convert.ToInt32(reader["ID_supply"]),
                                ID_Provider = Convert.ToInt32(reader["ID_Provider"]),
                                ID_material = Convert.ToInt32(reader["ID_material"]),
                                total_cost = Convert.ToSingle(reader["total_cost"]),
                                Date_order = Convert.ToDateTime(reader["Date_order"]),
                                Date_arrival = Convert.ToDateTime(reader["Date_arrival"]),
                                Status = reader["Status"].ToString(),
                                Warehouse = Convert.ToInt32(reader["Warehouse"]) // Загружаем ID_warehouse
                            });
                        }
                        PostavkaListView.ItemsSource = supplyOrders; // Обновляем источник данных
                    }
                }
            }
        }
        public class Provider
        {
            public int ID_Postavshik { get; set; }
            public string Name_Postavshik { get; set; }
            public string reliability { get; set; }
            public string phone { get; set; }
        }
        public class SupplyOrder
        {
            public int ID_supply { get; set; }
            public int ID_Provider { get; set; }
            public int ID_material { get; set; }
            public float total_cost { get; set; }
            public DateTime Date_order { get; set; }
            public DateTime Date_arrival { get; set; }
            public string Status { get; set; }
            public int Warehouse { get; set; } // Добавляем свойство для ID_warehouse
        }
        public class Material
        {
            public int ID_Materials { get; set; }
            public string Name_Maerials { get; set; }
            public int Сategory { get; set; }
            public float purchase_price { get; set; }
            public float sale_price { get; set; }
        }
        public class Warehouse
        {
            public int ID_warehouse { get; set; }
            public int ID_material { get; set; }
            public int Quantity_of_material { get; set; }
            public string Adress { get; set; }
        }
        private void LoadMaterials()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Materials", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Material> materials = new List<Material>();
                        while (reader.Read())
                        {
                            materials.Add(new Material
                            {
                                ID_Materials = Convert.ToInt32(reader["ID_Materials"]),
                                Name_Maerials = reader["Name_Maerials"].ToString(),
                                Сategory = Convert.ToInt32(reader["Сategory"]),
                                purchase_price = Convert.ToSingle(reader["purchase_price"]),
                                sale_price = Convert.ToSingle(reader["sale_price"])
                            });
                        }
                        MaterialsListView.ItemsSource = materials; // Обновляем источник данных
                    }
                }
            }
        }
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Сбрасываем выбранные идентификаторы перед загрузкой данных
                selectedMaterialId = -1;
                selectedWarehouseId = -1;
                selectedProviderId = -1;

                // Загрузка данных для материалов, складов и поставщиков
                LoadSupplyOrders();
                LoadMaterials(); // Загрузка материалов
                LoadWarehouses(); // Загрузка складов
                LoadProviders(); // Загрузка поставщиков
            }
        }
        private void LoadWarehouses()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Warehouse", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Warehouse> warehouses = new List<Warehouse>();
                        while (reader.Read())
                        {
                            warehouses.Add(new Warehouse
                            {
                                ID_warehouse = Convert.ToInt32(reader["ID_warehouse"]),
                                ID_material = Convert.ToInt32(reader["ID_material"]),
                                Quantity_of_material = Convert.ToInt32(reader["Quantity_of_material"]),
                                Adress = reader["Adress"].ToString()
                            });
                        }
                        WarehouseListView.ItemsSource = warehouses; // Обновляем источник данных
                    }
                }
            }
        }

        private void MaterialsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialsListView.SelectedItem is Material selectedMaterial)
            {
                selectedMaterialId = selectedMaterial.ID_Materials;
                MessageBox.Show(Convert.ToString(selectedMaterialId));
                Quantity_of_materials.IsEnabled = true;
            }
        }



        private void UpdateTotalCost()
        {

            if (selectedMaterialId > 0 && selectedWarehouseId > 0)
            {

            }
            else
            {
                var mat = ZXCZXCZXC.dbo.Materials.FirstOrDefault(G =>G.ID_Materials == selectedMaterialId);
                int price = Convert.ToInt32(mat.purchase_price);
                float totalCost = price * Convert.ToInt32(Quantity_of_materials.Text);
                TotalCostTextBox.Text = totalCost.ToString("C");
            }
        }

        private float GetMaterialPrice(int materialId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT purchase_price FROM Materials WHERE ID_Materials = @ID_Materials", connection))
                {
                    command.Parameters.AddWithValue("@ID_Materials", materialId);
                    return Convert.ToSingle(command.ExecuteScalar());
                }
            }
        }

        private int GetWarehouseQuantity(int warehouseId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Quantity_of_material FROM Warehouse WHERE ID_warehouse = @ID_warehouse", connection))
                {
                    command.Parameters.AddWithValue("@ID_warehouse", warehouseId);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        private void WarehouseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void AddSupplyOrder_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на валидность введенных данных
            if (selectedMaterialId == -1 || selectedWarehouseId == -1 || selectedProviderId == -1 ||
                DateOrderPicker.SelectedDate == null ||
                DateArrivalPicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(StatusTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, выберите материал, склад, поставщика и заполните все поля.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Supply_order (ID_Provider, ID_material, total_cost, Date_order, Date_arrival, Status, Warehouse, Quantity_of_material) VALUES (@ID_Provider, @ID_material, @total_cost, @Date_order, @Date_arrival, @Status, @Warehouse, @Quantity_of_material)", connection))
                {
                    command.Parameters.AddWithValue("@ID_Provider", selectedProviderId);
                    command.Parameters.AddWithValue("@ID_material", selectedMaterialId);
                    command.Parameters.AddWithValue("@total_cost", Convert.ToSingle(TotalCostTextBox.Text));
                    command.Parameters.AddWithValue("@Date_order", DateOrderPicker.SelectedDate.Value);
                    command.Parameters.AddWithValue("@Date_arrival", DateArrivalPicker.SelectedDate.Value);
                    command.Parameters.AddWithValue("@Status", StatusTextBox.Text);
                    command.Parameters.AddWithValue("@Warehouse", selectedWarehouseId); // Передаем id склада
                    command.Parameters.AddWithValue("@Quantity_of_material", Quantity_of_materials.Text);
                    command.ExecuteNonQuery();

                }

                // Здесь мы убираем Convert.ToInt32()
                var Warehouse = ADO.ZXCZXCZXC.dbo.Warehouse.FirstOrDefault(A => A.ID_warehouse == selectedWarehouseId);
                if (Warehouse != null)
                {
                    Warehouse.Quantity_of_material -= Convert.ToInt32(Quantity_of_materials.Text); // Обновляем количество материала
                    ZXCZXCZXC.dbo.SaveChanges();
                    LoadWarehouses();
                }
            }

            // Обновление списка заказов
            LoadSupplyOrders();
        }
        
        private void DeleteSupplyOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (PostavkaListView.SelectedItem is SupplyOrder selectedOrder)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM Supply_order WHERE ID_supply = @ID_supply", connection))
                    {
                        command.Parameters.AddWithValue("@ID_supply", selectedOrder.ID_supply);
                        command.ExecuteNonQuery();
                    }
                }

                // Удаляем элемент из ListView
                LoadSupplyOrders(); // Обновляем список заказов
                MessageBox.Show("Заказ успешно удален.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
            }
        }
        private void QuantityTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateTotalCost();
        }
    }
}