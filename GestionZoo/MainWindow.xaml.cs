using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Data.SqlClient;
using System.Data;

namespace GestionZoo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            String connectionManager = ConfigurationManager.ConnectionStrings["GestionZoo.Properties.Settings.EsDirectaDbConnectionString"]
                                                           .ConnectionString;
            sqlConnection = new SqlConnection(connectionManager);
            MostrarZoos();
            MostrarAnimales();
        }

        private void MostrarZoos()
        {
            String consulta = "Select *from zoo ";
            SqlDataAdapter adapter = new SqlDataAdapter(consulta,sqlConnection);

            using (adapter)
            {
                try
                {
                    DataTable zooTable = new DataTable();
                    adapter.Fill(zooTable);
                    ListaZoos.DisplayMemberPath = "Ubicacion";
                    ListaZoos.SelectedValuePath = "Id";
                    ListaZoos.ItemsSource = zooTable.DefaultView;
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               

            }
        }

        private void MostrarAnimales()
        {
            String consulta = "Select *from Animal ";
            SqlDataAdapter adapter = new SqlDataAdapter(consulta, sqlConnection);

            using (adapter)
            {
                try
                {
                    DataTable AnimalTable = new DataTable();
                    adapter.Fill(AnimalTable);
                    ListaAnimales.DisplayMemberPath = "nombre";
                    ListaAnimales.SelectedValuePath = "Id";
                    ListaAnimales.ItemsSource = AnimalTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void EliminarRegistroZoo()
        {
            try
            {
                string consulta = "Delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.ExecuteScalar();  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarZoos();
            } 

        }

        private void MostrarZooElegidoTextBox()
        {

            String consulta = "Select Ubicacion from Zoo where Id = @ZooId";
            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

            using (adapter)
            {
                try
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                    DataTable ZooDataTable = new DataTable();
                    adapter.Fill(ZooDataTable);
                    miTextBox.Text = ZooDataTable.Rows[0]["Ubicacion"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void MostrarAnimalElegidoTextBox()
        {
            String consulta = "Select nombre from Animal where Id = @AnimalId";
            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

            using (adapter)
            {
                try
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);
                    DataTable AnimalTable = new DataTable();
                    adapter.Fill(AnimalTable);
                    miTextBox.Text = AnimalTable.Rows[0]["nombre"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void EliminarRegistroAnimal()
        {

            try
            {
                string consulta = "Delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarAnimales();
            }
        }

        private void IngresarDatosZoologicos()
        {
            
           
            try
            {
                String consulta = "Insert into Zoo values (@Ubicacion)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Ubicacion", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                    MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarZoos();
            }
        }

        private void MostrarAnimalesAsoiados()
        {
            String consulta = "Select *from Animal a Inner Join AnimalZoo az on a.Id = az.AnimalId  where az.ZooId = @ZooId";
            SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

            using (adapter)
            {
                try
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                    DataTable AnimalTabla = new DataTable();
                    adapter.Fill(AnimalTabla);
                    ListaAnimalesAsociados.DisplayMemberPath = "nombre";
                    ListaAnimalesAsociados.SelectedValuePath = "Id";
                    ListaAnimalesAsociados.ItemsSource = AnimalTabla.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void AgregarAnimal()
        {
            try
            {
                String consulta = "Insert into Animal values (@nombre)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@nombre", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarAnimales();
            }
        }

        private void ListaZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MostrarAnimalesAsoiados();
            MostrarZooElegidoTextBox();
        }

        private void EliminarZoo_Click_1(object sender, RoutedEventArgs e)
        {
            EliminarRegistroZoo();
        }

        private void AgregarAnimal_Click(object sender, RoutedEventArgs e)
        {
            AgregarAnimal();
        }

        private void EliminarAnimal_Click(object sender, RoutedEventArgs e)
        {
            EliminarRegistroAnimal();
        }

        private void AgregarZoo_Click(object sender, RoutedEventArgs e)
        {
            IngresarDatosZoologicos();
        }

        private void AgregarAnimalZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String consulta = "Insert into AnimalZoo values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarAnimalesAsoiados();
            }
        }


        private void miTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       
        private void ListaAnimales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          MostrarAnimalElegidoTextBox();
        }

        private void   ActualizarAnimal_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                String consulta = "Update Animal Set nombre = @nombre where Id=@AnimalId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimales.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@nombre", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarAnimales();
            }
        }

        private void ActualizarZoo_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                String consulta = "Update Zoo Set Ubicacion = @Ubicacion where Id=@ZooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId",ListaZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Ubicacion", miTextBox.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarZoos();
            }
        }

      

        private void AgregarAnimalAsociado_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from AnimalZoo where AnimalId = @AnimalId AND ZooId = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", ListaZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", ListaAnimalesAsociados.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                MostrarAnimalesAsoiados();
            }

        }
    }
}
