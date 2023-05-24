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
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;

namespace PrevoznaSredstva
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CanExecuteChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        DispatcherTimer dt = new DispatcherTimer();
        private ObservableCollection<Oglasi> noviOglasi = new ObservableCollection<Oglasi>(); // list of new ads

        public ObservableCollection<Oglasi> ListaOglasi
        {
            get { return noviOglasi; }
            set
            {
                if (noviOglasi != value)
                {
                    noviOglasi = value;
                    OnPropertyChanged(nameof(noviOglasi));
                }
            }
        }

        private Grid defaultLayout;
        private DockPanel alternativeLayout;
        public MainWindow()
        {
            InitializeComponent();

            defaultLayout = CreateDefaultLayout();
            alternativeLayout = CreateAlternativeLayout();

            ViewModel viewModel = new ViewModel();
            DataContext = viewModel;

            ListaOglasa.ItemsSource = noviOglasi;
            ListaOglasa.MouseDoubleClick += new MouseButtonEventHandler(ListaOglasa_MouseDoubleClick);

            dt.Interval = TimeSpan.FromMinutes(1);
            dt.Tick += dt_Tick;
            dt.Start();
            void dt_Tick(object sender, EventArgs e)
            {
                Export();
            }

            if (File.Exists("data.xml"))
            {
                using (TextReader tr = new StreamReader("data.xml"))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Oglasi>));
                    noviOglasi = (ObservableCollection<Oglasi>)xs.Deserialize(tr);
                    ListaOglasa.ItemsSource = noviOglasi;
                }
            }
            else
            {
                noviOglasi.Add(new Oglasi()
                {
                    Naziv = "oglas1",
                    Znamka = "Audi",
                    LetoProizvodnje = "2010",
                    PrevozeniKm = "20000",
                    Starost = "13",
                    Slika = new BitmapImage(new Uri("Images/logo.png")),

                });
                noviOglasi.Add(new Oglasi()
                {
                    Naziv = "oglas2",
                    Znamka = "BMW",
                    LetoProizvodnje = "2011",
                    PrevozeniKm = "20000",
                    Starost = "12",
                    Slika = new BitmapImage(new Uri("Images/logo.png")),
                });
                noviOglasi.Add(new Oglasi()
                {
                    Naziv = "oglas3",
                    Znamka = "Mercedes",
                    LetoProizvodnje = "2012",
                    PrevozeniKm = "20000",
                    Starost = "11",
                    Slika = new BitmapImage(new Uri("Images/logo.png")),
                });
            }
        }
        private void Export()
        {
            using (TextWriter tw = new StreamWriter("data.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Oglasi>));
                xs.Serialize(tw, noviOglasi);
            }
        }
        public void StopTimer()
        {
            if (dt.IsEnabled == true)
            {
                dt.Stop();
            }
        }
        public void StartTimer()
        {
            if (dt.IsEnabled == false)
            {
                dt.Start();
            }
        }
        private void ListaOglasa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListaOglasa.SelectedItem != null)
            {
                var itemName = ListaOglasa.SelectedItem.ToString();
                MessageBox.Show($"You have chosen: {itemName}", "Name of input", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("You didn't select the ad!");

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
                MessageBox.Show("You have to select an ad in order to edit it!");
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files (*.xml)|*.xml";
            if (ofd.ShowDialog() == true)
            {
                string filePath = ofd.FileName;

                if (File.Exists(filePath))
                {
                    try
                    {
                        using (TextReader tr = new StreamReader(filePath))
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Oglasi>));
                            noviOglasi = (ObservableCollection<Oglasi>)xs.Deserialize(tr);
                            ListaOglasa.ItemsSource = noviOglasi;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save1 = new SaveFileDialog();
            save1.Filter = "XML Files (*.xml)|*.xml";
            if (save1.ShowDialog() == true)
            {
                string filePath = save1.FileName;

                try
                {
                    using (TextWriter tw = new StreamWriter(filePath))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Oglasi>));
                        xs.Serialize(tw, noviOglasi);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Layout implementation

        private Grid CreateDefaultLayout()
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            // Top Row: Menu and Logo
            Grid topRowGrid = new Grid();
            topRowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            topRowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            Button logoButton = new Button()
            {
                Style = (Style)FindResource("ButtonImageStyle")
            };
            Image logoImage = new Image()
            {
                Source = new BitmapImage(new Uri("Images/logo.png", UriKind.Relative)),
                Height = 70,
                Width = 122,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            logoButton.Content = logoImage;
            Grid.SetColumn(logoButton, 0);
            topRowGrid.Children.Add(logoButton);

            Menu menu = new Menu()
            {
                Style = (Style)FindResource("MenuStyle"),
            };

            MenuItem fileMenuItem = new MenuItem() { Header = "File", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(fileMenuItem);

            MenuItem importMenuItem = new MenuItem() { Header = "Import", Style = (Style)FindResource("MenuItemStyle") };
            importMenuItem.Click += Import_Click;

            MenuItem exportMenuItem = new MenuItem() { Header = "Export", Style = (Style)FindResource("MenuItemStyle") };
            exportMenuItem.Click += Export_Click;

            fileMenuItem.Items.Add(importMenuItem);
            fileMenuItem.Items.Add(exportMenuItem);

            MenuItem ads = new MenuItem() { Header = "Ads", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(ads);

            MenuItem dodaj = new MenuItem() { Header = "Add", Style = (Style)FindResource("MenuItemStyle") };
            dodaj.Click += Dodaj_Click;

            MenuItem odstrani = new MenuItem() { Header = "Remove", Style = (Style)FindResource("MenuItemStyle") };
            odstrani.Click += Brisi_Click;

            MenuItem uredi = new MenuItem() { Header = "Edit", Style = (Style)FindResource("MenuItemStyle") };
            uredi.Click += Edit_Click;

            ads.Items.Add(dodaj);
            ads.Items.Add(odstrani);
            ads.Items.Add(uredi);

            MenuItem orodja = new MenuItem() { Header = "Tools", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(orodja);

            MenuItem nastavitve = new MenuItem() { Header = "Settings", Style = (Style)FindResource("MenuItemStyle") };
            nastavitve.Click += NastavitveClick;
            menu.Items.Add(nastavitve);

            MenuItem layout = new MenuItem() { Header = "Layout", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(layout);

            MenuItem defaultLayout = new MenuItem() { Header = "Def", Style = (Style)FindResource("MenuItemStyle") };
            defaultLayout.Click += DefaultLayout_Click;

            MenuItem altLayout = new MenuItem() { Header = "Alt", Style = (Style)FindResource("MenuItemStyle") };
            altLayout.Click += AlternativeLayout_Click;

            MenuItem exit = new MenuItem() { Header = "Exit", Style = (Style)FindResource("MenuItemStyle") };
            exit.Click += ExitApp;

            layout.Items.Add(defaultLayout);
            layout.Items.Add(altLayout);
            layout.Items.Add(exit);

            Grid.SetColumn(menu, 1);
            topRowGrid.Children.Add(menu);

            Grid.SetRow(topRowGrid, 0);
            grid.Children.Add(topRowGrid);

            // Middle Row: Content
            Grid middleRowGrid = new Grid();
            middleRowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            middleRowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            // Left Panel: ListView
            ListView listView = new ListView()
            {
                Style = (Style)FindResource("ListViewStyle"),
                Name = "ListaOglasa",
                ItemsSource = noviOglasi
            };
            listView.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ListaOglasi"));
            listView.SetBinding(Selector.SelectedItemProperty, new Binding("CurrSelected"));

            Grid.SetColumn(listView, 0);
            middleRowGrid.Children.Add(listView);

            // Right Panel: Filtering UserControl
            Filtering filteringControl = new Filtering()
            {
                Style = (Style)FindResource("FilteringStyle")
            };
            Grid.SetColumn(filteringControl, 1);
            middleRowGrid.Children.Add(filteringControl);

            Label currentLabel = new Label()
            {
                Style = (Style)FindResource("CurrentLabelStyle"),
                Name = "current"
            };
            currentLabel.SetBinding(ContentProperty, new Binding("CurrSelected.Naziv"));
            Grid.SetColumn(currentLabel, 1);
            middleRowGrid.Children.Add(currentLabel);

            Grid.SetRow(middleRowGrid, 1);
            grid.Children.Add(middleRowGrid);

            return grid;
        }

        private DockPanel CreateAlternativeLayout()
        {
            DockPanel dockPanel = new DockPanel();
            dockPanel.LastChildFill = true;

            // Top Area: Menu and Logo
            StackPanel topAreaStackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };

            Button logoButton = new Button()
            {
                Style = (Style)FindResource("ButtonImageStyle")
            };
            Image logoImage = new Image()
            {
                Source = new BitmapImage(new Uri("Images/logo.png", UriKind.Relative)),
                Height = 70,
                Width = 122,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            logoButton.Content = logoImage;
            topAreaStackPanel.Children.Add(logoButton);

            Menu menu = new Menu()
            {
                Style = (Style)FindResource("MenuStyle"),
            };

            MenuItem fileMenuItem = new MenuItem() { Header = "File", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(fileMenuItem);

            MenuItem importMenuItem = new MenuItem() { Header = "Import", Style = (Style)FindResource("MenuItemStyle") };
            importMenuItem.Click += Import_Click;

            MenuItem exportMenuItem = new MenuItem() { Header = "Export", Style = (Style)FindResource("MenuItemStyle") };
            exportMenuItem.Click += Export_Click;

            fileMenuItem.Items.Add(importMenuItem);
            fileMenuItem.Items.Add(exportMenuItem);

            MenuItem ads = new MenuItem() { Header = "Ads", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(ads);

            MenuItem dodaj = new MenuItem() { Header = "Add", Style = (Style)FindResource("MenuItemStyle") };
            dodaj.Click += Dodaj_Click;

            MenuItem odstrani = new MenuItem() { Header = "Remove", Style = (Style)FindResource("MenuItemStyle") };
            odstrani.Click += Brisi_Click;

            MenuItem uredi = new MenuItem() { Header = "Edit", Style = (Style)FindResource("MenuItemStyle") };
            uredi.Click += Edit_Click;

            ads.Items.Add(dodaj);
            ads.Items.Add(odstrani);
            ads.Items.Add(uredi);

            MenuItem orodja = new MenuItem() { Header = "Tools", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(orodja);

            MenuItem nastavitve = new MenuItem() { Header = "Settings", Style = (Style)FindResource("MenuItemStyle") };
            nastavitve.Click += NastavitveClick;
            menu.Items.Add(nastavitve);

            MenuItem layout = new MenuItem() { Header = "Layout", Style = (Style)FindResource("MenuItemStyle") };
            menu.Items.Add(layout);

            MenuItem defaultLayout = new MenuItem() { Header = "Def", Style = (Style)FindResource("MenuItemStyle") };
            defaultLayout.Click += DefaultLayout_Click;

            MenuItem altLayout = new MenuItem() { Header = "Alt", Style = (Style)FindResource("MenuItemStyle") };
            altLayout.Click += AlternativeLayout_Click;

            MenuItem exit = new MenuItem() { Header = "Exit", Style = (Style)FindResource("MenuItemStyle") };
            exit.Click += ExitApp;

            layout.Items.Add(defaultLayout);
            layout.Items.Add(altLayout);
            layout.Items.Add(exit);

            topAreaStackPanel.Children.Add(menu);

            DockPanel.SetDock(topAreaStackPanel, Dock.Top);
            dockPanel.Children.Add(topAreaStackPanel);

            Grid contentGrid = new Grid();
            contentGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            contentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            // ListView
            ListView listView = new ListView()
            {
                Style = (Style)FindResource("ListViewStyle"),
                Name = "ListaOglasa",
                ItemsSource = noviOglasi
            };
            listView.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ListaOglasi"));
            listView.SetBinding(Selector.SelectedItemProperty, new Binding("CurrSelected"));

            Grid.SetRow(listView, 0);
            contentGrid.Children.Add(listView);

            // Right Panel: Filtering UserControl
            Filtering filteringControl = new Filtering()
            {
                Style = (Style)FindResource("FilteringStyle")
            };
            Grid.SetRow(filteringControl, 1);
            contentGrid.Children.Add(filteringControl);

            DockPanel.SetDock(contentGrid, Dock.Right);
            dockPanel.Children.Add(contentGrid);

            return dockPanel;
        }

        private void DefaultLayout_Click(object sender, RoutedEventArgs e)
        {
            FormLayoutGrid.Children.Clear();
            FormLayoutGrid.Children.Add(defaultLayout);
        }

        private void AlternativeLayout_Click(object sender, RoutedEventArgs e)
        {
            FormLayoutGrid.Children.Clear();
            FormLayoutGrid.Children.Add(alternativeLayout);
        }
    }
}
