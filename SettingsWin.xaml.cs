using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PrevoznaSredstva
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private DispatcherTimer autoSaveTimer;
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
        private void TurnOn_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).StartTimer();
            txtAutoSave.Text = "Auto save is on";
        }

        private void TurnOff_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).StopTimer();
            txtAutoSave.Text = DateTime.Now.ToString() + "Auto save is off!";
        }
    }
}
