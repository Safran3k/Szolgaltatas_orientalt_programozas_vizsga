using FilmekClient.Formok;
using FilmekClient.Osztalyok;
using RestSharp;
using RestSharp.Serializers.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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

namespace FilmekClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RestClient loginClient = new RestClient(ConfigurationManager.AppSettings["LoginPath"]);
        Felhasznalo felhasznalo = null;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (felhNevTb.Text != "" && jelszoTb.Password != "")
            {
                RestRequest request = new RestRequest();
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddObject(
                    new
                    {
                        FelhasznaloNev = felhNevTb.Text.Trim(),
                        Jelszo = jelszoTb.Password.Trim()
                    });

                RestResponse response = loginClient.Get(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show(response.ErrorMessage);
                    return;
                }
                try
                {
                    felhasznalo = new SystemTextJsonSerializer().Deserialize<Felhasznalo>(response);

                    FoForm main = new FoForm(felhasznalo);
                    main.Show();
                    Close();
                }
                catch
                {
                    MessageBox.Show("Sikertelen bejelentkezés: Érvénytelen adatok", "Sikertelen bejelentkezés", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Sikertelen bejelentkezés: adatok megadása kötelező", "Sikertelen bejelentkezés", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        private void regisztracioBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisztracioForm reg = new RegisztracioForm();
            reg.Show();
            Close();
        }

        private void tovabbLb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FoForm fo = new FoForm(null);
            fo.Show();
            Close();
        }
    }
}
