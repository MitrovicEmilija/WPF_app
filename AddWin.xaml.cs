using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace PrevoznaSredstva
{
    /// <summary>
    /// Interaction logic for AddWin.xaml
    /// </summary>
    public partial class AddWin : Window
    {
        public AddWin()
        {
            InitializeComponent();
            ViewModelZnamke vmZnamke = new ViewModelZnamke();
            this.DataContext = vmZnamke;
            comboBox.ItemsSource = vmZnamke.znamkeKolekcija;
        }

        private void potrdi_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void izberi_Sliko_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "(*.png;*.jpg)|*.png;*.jpg";
            opf.Multiselect = false;
            if (opf.ShowDialog() == true)
            {
                slika.Source = new BitmapImage(new Uri(opf.FileName, UriKind.Absolute));
                slika.Source = new BitmapImage(new Uri(opf.FileName));

            }
        }
    }
}
