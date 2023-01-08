using RestSharp;
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
using System.Windows.Shapes;

namespace FilmekClient.Formok
{
    /// <summary>
    /// Interaction logic for RegisztracioForm.xaml
    /// </summary>
    public partial class RegisztracioForm : Window
    {
        RestClient loginClient = new RestClient(ConfigurationManager.AppSettings["LoginPath"]);

        public RegisztracioForm()
        {
            InitializeComponent();
        }

        private void visszaBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            Close();
        }

        private void regisztracioBtn_Click(object sender, RoutedEventArgs e)
        {
            if (jelszoTb.Password.Trim() == "" || jelszoUjraTb.Password.Trim() == "" || felhNevTb.Text.Trim() == "")
            {
                MessageBox.Show("Minden mező kitöltése kötelező!", "Mezők kitöltése", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (jelszoTb.Password.Trim() != jelszoUjraTb.Password.Trim())
                {
                    MessageBox.Show("A két jelszó nem egyezik.", "Különböző jelszavak", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    bool darab = false;
                    RestRequest req = new RestRequest();
                    req.RequestFormat = RestSharp.DataFormat.Json;
                    req.AddObject(new
                    {
                        FelhasznaloNev = felhNevTb.Text.Trim()
                    });

                    RestResponse resp = loginClient.Get(req);

                    if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show(resp.ErrorMessage);
                        return;
                    }

                    try
                    {
                        darab = resp.Content.Equals("{\"COUNT(id)\":\"0\"}");
                    }
                    catch
                    {
                        return;
                    }

                    if (darab)
                    {
                        RestRequest request = new RestRequest();
                        request.RequestFormat = RestSharp.DataFormat.Json;

                        request.AddObject(
                            new
                            {
                                FelhasznaloNev = felhNevTb.Text.Trim(),
                                Jelszo = jelszoTb.Password.Trim()
                            });

                        RestResponse response = loginClient.Post(request);

                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            MessageBox.Show(response.ErrorMessage);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Sikeres regisztráció, tovább a bejelentkezéshez", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainWindow login = new MainWindow();
                            login.felhNevTb.Text = felhNevTb.Text;
                            login.Show();
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ez a felhasználónév már létezik!", "Foglalt felhasználónév", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void tovabbLb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FoForm fo = new FoForm(null);
            fo.Show();
            Close();
        }
    }
}
