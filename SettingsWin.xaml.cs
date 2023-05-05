using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using System.Xml.Linq;

namespace PrevoznaSredstva
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {        
        public Window1()
        {
            InitializeComponent();
            ViewModelZnamke vmZnamke = new ViewModelZnamke();
            this.DataContext = vmZnamke;
            ListaZnamk.ItemsSource = vmZnamke.znamkeKolekcija;
        }

        private void AddZnamko_Click(object sender, RoutedEventArgs e)
        {
            ViewModelZnamke vm = (ViewModelZnamke)this.DataContext;
            vm.DodajZnamko.Execute(null);
            ListaZnamk.Items.Refresh();
        }

        private void RemoveZnamko_Click(object sender, RoutedEventArgs e)
        {
            ViewModelZnamke vm = (ViewModelZnamke)this.DataContext;
            vm.OdstraniZnamko.Execute(null);
            ListaZnamk.Items.Refresh();
        }

        private void UpdateZnamko_Click(object sender, RoutedEventArgs e)
        {
            ViewModelZnamke vm = (ViewModelZnamke)this.DataContext;
            vm.PosodobiZnamko.Execute(null);
            ListaZnamk.Items.Refresh();
        }

        /*
        // List the current items.
        private void ListItems()
        {
            ListaZnamk.ItemsSource =
                Properties.Settings.Default.Znamka.Cast<string>().ToArray();
            ListaZnamk.SelectedIndex = -1;
        }

        // Display the selected item.
        private void LstZnamk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListaZnamk.SelectedItem == null)
                txtNazivZnamke.Clear();
            else
                txtNazivZnamke.Text = ListaZnamk.SelectedItem.ToString();
        }

        // Add a value.
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Znamka.Add(txtNazivZnamke.Text);
            Properties.Settings.Default.Save();
            txtNazivZnamke.Clear();
            txtNazivZnamke.Focus();
            ListItems();
        }

        // Remove an item value.
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Znamka.Remove(ListaZnamk.SelectedItem.ToString());
            txtNazivZnamke.Clear();
            txtNazivZnamke.Focus();
            ListItems();
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {

        }
        */
    }
}
