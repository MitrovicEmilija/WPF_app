using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EditWin.xaml
    /// </summary>
    public partial class EditWin : Window
    {
        public EditWin()
        {
            InitializeComponent();
            //this.DataContext = ViewModel.getInstance();
            ViewModelZnamke vmZnamke = new ViewModelZnamke();
            this.DataContext = vmZnamke;
            comboBox.ItemsSource = vmZnamke.znamkeKolekcija;
        }
        private void potrdiEdit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            //ViewModel viewModel = this.DataContext as ViewModel;
            //viewModel.UrediOglasCommand.Execute(true);
            //this.Close();
        }
        private void cancelEdit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void izberi_SlikoEdit_Click(object sender, RoutedEventArgs e)
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
