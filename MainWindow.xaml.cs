using Microsoft.Win32;
using PrevoznaSredstva.MyUserControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Xml.Serialization;
using System.Windows.Threading;

namespace PrevoznaSredstva
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            // inicializacija view modela 

            ViewModel viewModel = new ViewModel();

            // uporaba view modela

            DataContext = viewModel;
            ListaOglasa.ItemsSource = viewModel.listaOglasa;

            ListaOglasa.MouseDoubleClick += new MouseButtonEventHandler(ListaOglasa_MouseDoubleClick);

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Tick += Dt_Tick;
            dt.Start();

        }

        private void Dt_Tick(object? sender, EventArgs e)
        {
            Label_Time.Content = DateTime.Now.ToString();
        }

        private void ListaOglasa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListaOglasa.SelectedItem != null)
            {
                var itemName = ListaOglasa.SelectedItem.ToString();
                MessageBox.Show($"Izbrali ste vnos z imenom: {itemName}", "Ime vnosa", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NastavitveClick(object sender, RoutedEventArgs e)
        {
            var settingsWin = new Window1();
            settingsWin.ShowDialog();
        }

        private ObservableCollection<Oglasi> noviOglasi = new ObservableCollection<Oglasi>();
        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddWin();
            addWin.ShowDialog();

            Oglasi oglas = new Oglasi();

            oglas.Naziv = addWin.naziv_oglasa.Text;
            oglas.Znamka = addWin.comboBox.Text;
            oglas.Starost = addWin.starost_avta.Text;
            oglas.LetoProizvodnje = addWin.leto_proizvodnje.Text;
            oglas.PrevozeniKm = addWin.prevozeni_km.Text;
            
            if(addWin.slika.Source != null)
            {
                oglas.Slika = new BitmapImage(new Uri(addWin.slika.Source.ToString()));
            }
            ListaOglasa.ItemsSource = noviOglasi;
            noviOglasi.Add(oglas);
        }

        private void Brisi_Click(object sender, RoutedEventArgs e)
        {
            if (ListaOglasa.SelectedIndex >= 0)
            {
                noviOglasi.RemoveAt(ListaOglasa.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Niste izbrali oglas!");

            }
        }
        public void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ListaOglasa.SelectedIndex >= 0)
            {
                Oglasi selectedItem = (Oglasi)ListaOglasa.SelectedItem;

                EditWin editWin = new EditWin();
                editWin.naziv_oglasa.Text = selectedItem.Naziv;
                editWin.comboBox.Text = selectedItem.Znamka;
                editWin.starost_avta.Text = selectedItem.Starost;
                editWin.leto_proizvodnje.Text = selectedItem.LetoProizvodnje;
                editWin.prevozeni_km.Text = selectedItem.PrevozeniKm;
                editWin.slika.Source = selectedItem.Slika;

                editWin.ShowDialog();

                // Update the item in the list
                selectedItem.Naziv = editWin.naziv_oglasa.Text;
                selectedItem.Znamka = editWin.comboBox.Text;
                selectedItem.Starost = editWin.starost_avta.Text;
                selectedItem.LetoProizvodnje = editWin.leto_proizvodnje.Text;
                selectedItem.PrevozeniKm = editWin.prevozeni_km.Text;
                selectedItem.Slika = (BitmapSource)editWin.slika.Source;

                // Refresh the list to reflect the changes
                ListaOglasa.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Morate izbrati oglas da boste lahko urejali!");
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = ".xml",
                Filter = "Images (*.xml," + "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            openFileDialog.ShowDialog();

            var vm = this.DataContext as ViewModel;
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    try
                    {
                        XmlSerializer xml = new XmlSerializer(vm.ListaOglasi.GetType());
                        ObservableCollection<Oglasi> oglasi = (ObservableCollection<Oglasi>)xml.Deserialize(sr);
                        if (oglasi != null)
                        {
                            vm.ListaOglasi = oglasi;
                        }

                        ListaOglasa.Items.Refresh();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("PODATKI NISO KOMPATIBILNI: " + exception.Message);
                    }
                    finally
                    {
                        vm.ListaOglasi = new ObservableCollection<Oglasi>();
                    }

                }
            }

        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            saveFileDialog.InitialDirectory = @"D:\";
            saveFileDialog.Title = "Save a XML File";
            saveFileDialog.ShowDialog();

            var vm = this.DataContext as ViewModel;
            var name = saveFileDialog.FileName;
            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                using (StreamWriter sw = new StreamWriter(name))
                {
                    XmlSerializer xml = new XmlSerializer(vm.ListaOglasi.GetType());
                    xml.Serialize(sw, vm.ListaOglasi);
                }
            }
        }
    }
}
