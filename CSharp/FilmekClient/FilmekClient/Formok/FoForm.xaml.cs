using FilmekClient.Osztalyok;
using Newtonsoft.Json;
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

namespace FilmekClient.Formok
{
    /// <summary>
    /// Interaction logic for FoForrm.xaml
    /// </summary>
    public partial class FoForm : Window
    {
        RestClient filmekClient = new RestClient(ConfigurationManager.AppSettings["FilmekPath"]);
        RestClient loginClient = new RestClient(ConfigurationManager.AppSettings["LoginPath"]);
        Felhasznalo aktFelhasznalo;
        JArray objektumok; 
        uint kivalasztottFilmId;

        public FoForm(Felhasznalo aktFelhasznalo)
        {
            this.aktFelhasznalo = aktFelhasznalo;

            InitializeComponent();

            FilmekFeltoltDGV();
            GroupBoxokBeallitasa();
            filmekDgv.Items.Refresh();
        }

        public void FilmekFeltoltDGV()
        {
            // Ha nincs bejelentkezve senki, akkor látja az összes filmet, ha valaki bejeletnkezik akkor csak a saját filmjeihez fér hozzá
            if (aktFelhasznalo == null)
            {
                RestRequest request = new RestRequest();
                request.RequestFormat = RestSharp.DataFormat.Json;
                RestResponse response = filmekClient.Get(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(response.ErrorMessage);
                    return;
                }
                try
                {
                    objektumok = (JArray)JsonConvert.DeserializeObject<object>(response.Content);
                    filmekDgv.ItemsSource = objektumok;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                filmekDgv.Items.Refresh();
            }
            else
            {
                RestRequest req = new RestRequest();
                req.RequestFormat = RestSharp.DataFormat.Json;
                req.AddObject(new
                {
                    FelhasznaloId = aktFelhasznalo.ID
                });

                RestResponse response = filmekClient.Get(req);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(response.ErrorMessage);
                    return;
                }
                try
                {
                    objektumok = (JArray)JsonConvert.DeserializeObject<object>(response.Content);
                    filmekDgv.ItemsSource = objektumok;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                filmekDgv.Items.Refresh();
            }
        }

        private void GroupBoxokBeallitasa()
        {
            if (aktFelhasznalo == null)
            {
                vendegGb.Visibility = Visibility.Visible;
                normalFelhGb.Visibility = Visibility.Hidden;
            }
            else
            {
                vendegGb.Visibility = Visibility.Hidden;
                normalFelhGb.Visibility = Visibility.Visible;
            }
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            RegisztracioForm reg = new RegisztracioForm();
            reg.Show();
            Close();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            Close();
        }

        private void filmekDgv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataGridRow sor = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
            DataGridCell id;
            try
            {
                id = dg.Columns[0].GetCellContent(sor).Parent as DataGridCell;
            }
            catch
            {
                return;
            }

            try
            {
                kivalasztottFilmId = uint.Parse(((TextBlock)id.Content).Text);
            }
            catch
            {
                kivalasztottFilmId = 0;
                return;
            }

            DataGridCell filmId = dg.Columns[0].GetCellContent(sor).Parent as DataGridCell;
            idLb.Content = ((TextBlock)filmId.Content).Text;

            DataGridCell cim = dg.Columns[2].GetCellContent(sor).Parent as DataGridCell;
            filmCimeLb.Content = ((TextBlock)cim.Content).Text;

            DataGridCell rendezo = dg.Columns[3].GetCellContent(sor).Parent as DataGridCell;
            rendezoLb.Content = ((TextBlock)rendezo.Content).Text;

            DataGridCell mufaj = dg.Columns[4].GetCellContent(sor).Parent as DataGridCell;
            mufajLb.Content = ((TextBlock)mufaj.Content).Text;

            DataGridCell premier = dg.Columns[5].GetCellContent(sor).Parent as DataGridCell;
            premierDatumaLb.Content = ((TextBlock)premier.Content).Text;
        }



        // Hozzáadás módosítás törlés, kijelentkezés
        private void filmHozzaadBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aktFelhasznalo != null)
            {
                FilmHozzaadasaFrom hozzaad = new FilmHozzaadasaFrom(aktFelhasznalo, 0, "", "", "", DateTime.MinValue);
                hozzaad.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Jogosulatlan", "Nem jogosult hozzáférés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void filmModositasBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aktFelhasznalo != null)
            {
                if (kivalasztottFilmId != 0)
                {
                    string filmCime = filmCimeLb.Content.ToString();
                    string rendezo = rendezoLb.Content.ToString();
                    string mufaj = mufajLb.Content.ToString();
                    DateTime premierDatuma = DateTime.Parse(premierDatumaLb.Content.ToString());

                    FilmHozzaadasaFrom modosit = new FilmHozzaadasaFrom(aktFelhasznalo, kivalasztottFilmId, filmCime, rendezo, mufaj, premierDatuma);
                    modosit.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Nincs film kiválasztva");
                }
            }
            else
            {
                MessageBox.Show("Jogosulatlan");
            }
        }

        private void filmTorleseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aktFelhasznalo != null)
            {
                if (kivalasztottFilmId != 0)
                {
                    filmekClient = new RestClient(ConfigurationManager.AppSettings["FilmekPath"]);
                    RestRequest request = new RestRequest("http://localhost/Szop_vizsgara/PHP/filmek.php", Method.Delete);

                    request.AddParameter("id", kivalasztottFilmId);
                    request.AddParameter("FelhasznaloNev", aktFelhasznalo.FelhasznaloNev);
                    request.AddParameter("Jelszo", aktFelhasznalo.Jelszo);

                    RestResponse response = filmekClient.Execute(request);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show(response.Content);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Film törölve.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                        FoForm fo = new FoForm(aktFelhasznalo);
                        fo.filmekDgv.Items.Refresh();
                        fo.Show();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Nincs film kiválasztva", "Nincs semmi kiválasztva", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Jogosulatlan");
            }
        }

        private void kijelentkezesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aktFelhasznalo != null)
            {
                RestRequest request = new RestRequest();
                request.RequestFormat = RestSharp.DataFormat.Json;

                RestResponse response = loginClient.Patch(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show(response.ErrorMessage);
                    return;
                }
                else
                {
                    aktFelhasznalo = null;
                    MainWindow login = new MainWindow();
                    login.Show();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Jogosulatlan");
            }
        }
    }
}
