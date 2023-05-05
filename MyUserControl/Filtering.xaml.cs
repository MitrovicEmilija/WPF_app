using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace PrevoznaSredstva.MyUserControl
{
    /// <summary>
    /// Interaction logic for Filtering.xaml
    /// </summary>
    public partial class Filtering : UserControl
    {

        public Filtering()
        {
            InitializeComponent();
            this.DataContext = ViewModel.getInstance();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ViewModel.getInstance().listaOglasa);
            view.Filter = FilterOglas;
        }
        private bool FilterOglas(object oglas)
        {
            if (String.IsNullOrEmpty(filterText.Text))
            {
                return true;
            } else
            {
                return (oglas as Oglasi).Naziv.Contains(filterText.Text, StringComparison.OrdinalIgnoreCase);
            }
        }

        private void FilterText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                ViewModel vm = this.DataContext as ViewModel;
                CollectionViewSource.GetDefaultView(vm.listaOglasa).Refresh();
            } catch(Exception ex)
            {
                _ = ex.Message;
            }
        }
    }
}
