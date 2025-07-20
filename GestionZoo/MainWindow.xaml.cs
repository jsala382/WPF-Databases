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
    }
}
