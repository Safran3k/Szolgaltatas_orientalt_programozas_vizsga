using FilmekClient.Osztalyok;
using Newtonsoft.Json.Linq;
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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FilmekClient.Formok
{
    /// <summary>
    /// Interaction logic for FilmHozzaadasaFrom.xaml
    /// </summary>
    public partial class FilmHozzaadasaFrom : Window
    {
        RestClient filmekClient = new RestClient(ConfigurationManager.AppSettings["FilmekPath"]);
        const string UTVONAL = "http://localhost/Szop_vizsgara/PHP/filmek.php";
        Felhasznalo aktFelhasznalo;
        uint filmId;
        string filmCime, rendezo, mufaj;
        DateTime premierDatuma;

        public FilmHozzaadasaFrom(Felhasznalo aktFelhasznalo, uint filmId, string filmCime, string rendezo, string mufaj, DateTime premierDatuma)
        {
            this.aktFelhasznalo = aktFelhasznalo;
            this.filmId = filmId;
            this.filmCime = filmCime;
            this.rendezo = rendezo;
            this.mufaj = mufaj;
            this.premierDatuma = premierDatuma;

            InitializeComponent();

            ModositVagyHozzaad();
        }

        private void visszaAFooldalra_Click(object sender, RoutedEventArgs e)
        {
            if (aktFelhasznalo != null)
            {
                FoForm fo = new FoForm(aktFelhasznalo);
                fo.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Jogosulatlan", "Nem jogosult hozzáférés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void hozzaadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aktFelhasznalo != null)
            {
                if (filmCimeTb.Text.Trim() == "" || rendezoTb.Text.Trim() == "" || mufajTb.Text.Trim() == "")
                {
                    MessageBox.Show("Minden mező kitöltése kötelező.");
                }
                else
                {
                    filmekClient = new RestClient(ConfigurationManager.AppSettings["FilmekPath"]);
                    RestRequest request = new RestRequest("http://localhost/Szop_vizsgara/PHP/filmek.php", Method.Post);
                    request.RequestFormat = RestSharp.DataFormat.Json;

                    request.AddObject(new
                    {
                        FelhasznaloNev = aktFelhasznalo.FelhasznaloNev,
                        Jelszo = aktFelhasznalo.Jelszo,
                        FelhasznaloId = aktFelhasznalo.ID,
                        FilmCime = filmCimeTb.Text.Trim(),
                        Rendezo = rendezoTb.Text.Trim(),
                        Mufaj = mufajTb.Text.Trim(),
                        PremierDatuma = premierDatumaDp.SelectedDate.Value.Date.ToString("yyyy-MM-dd")
                    });

                    RestResponse response = filmekClient.Execute(request);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show(response.Content);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Sikeres hozzáadás", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                        FoForm fo = new FoForm(aktFelhasznalo);
                        fo.Show();
                        fo.filmekDgv.Items.Refresh();
                        Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Jogosulatlan", "Nem jogosult hozzáférés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void modositBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aktFelhasznalo != null)
            {
                if (filmCimeModositTb.Text.Trim() == "" || rendezoModositTb.Text.Trim() == "" || mufajModositTb.Text.Trim() == "")
                {
                    MessageBox.Show("Minden mező kitöltése kötelező.", "Hiányzó adatok", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    RestRequest request = new RestRequest(UTVONAL, Method.Put);
                    request.RequestFormat = RestSharp.DataFormat.Json;

                    request.AddBody(
                        new
                        {
                            id = filmId,
                            felhasznalonev = aktFelhasznalo.FelhasznaloNev,
                            jelszo = aktFelhasznalo.Jelszo,
                            felhasznaloid = aktFelhasznalo.ID,
                            filmcime = filmCimeModositTb.Text.Trim(),
                            rendezo = rendezoModositTb.Text.Trim(),
                            mufaj = mufajModositTb.Text.Trim(),
                            premierdatuma = premierDatumaModositDp.SelectedDate.Value.Date.ToString("yyyy-MM-dd")
                        });

                    RestResponse response = filmekClient.Execute(request);
                    
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show(response.StatusDescription);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Sikeres módosítás", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                        FoForm fo = new FoForm(aktFelhasznalo);
                        fo.Show();
                        fo.filmekDgv.Items.Refresh();
                        Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Jogosulatlan", "Nem jogosult hozzáférés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ModositVagyHozzaad()
        {
            if (filmId == 0)
            {
                hozzaadGb.Visibility = Visibility.Visible;
                modositGb.Visibility = Visibility.Hidden;
            }
            else
            {
                modositGb.Visibility = Visibility.Visible;
                hozzaadGb.Visibility = Visibility.Hidden;

                filmCimeModositTb.Text = filmCime;
                rendezoModositTb.Text = rendezo;
                mufajModositTb.Text = mufaj;
                premierDatumaModositDp.SelectedDate = premierDatuma;
            }
        }
    }
}
