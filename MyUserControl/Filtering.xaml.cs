using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class Filtering : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Filtering()
        {
            InitializeComponent();
            var vm = ViewModel.getInstance();
            this.DataContext = vm;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ViewModel.getInstance().ListaOglasi);
            view.Filter = FilterOglas;
        }
        private bool FilterOglas(object oglas)
        {
            if (String.IsNullOrEmpty(filterText.Text))
            {
                return true;
            } else
            {
                var oglasi = ViewModel.getInstance().ListaOglasi;
                return ((Oglasi)oglas).Naziv.IndexOf(filterText.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        private void FilterText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ViewModel.getInstance().ListaOglasi).Filter = FilterOglas;
            CollectionViewSource.GetDefaultView(ViewModel.getInstance().ListaOglasi).Refresh();
        }

        private String selectedZnamka;

        public String SelectedZnamka
        {
            get { return selectedZnamka; }
            set
            {
                if (selectedZnamka != value)
                {
                    selectedZnamka = value;
                    OnPropertyChanged(nameof(selectedZnamka));
                }
            }
        }
    }
}
