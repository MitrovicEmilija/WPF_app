using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PrevoznaSredstva
{
    public class ViewModel : INotifyPropertyChanged
    {
        // Singleton pattern za view model

        public static ViewModel instance = null;

        public static ViewModel getInstance()
        {
            if (instance == null)
            {
                instance = new ViewModel();
            }

            return instance;
        }

        // tukaj so metode za vmesnik INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CanExecuteChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // lista oglasa in ICommand objekti
        public ObservableCollection<Oglasi> listaOglasa { get; set; }
        public ICommand DodajOglasCommand { get; set; }
        public ICommand OdstraniOglasCommand { get; set; }
        public ICommand UrediOglasCommand { get; set; }

        // konstruktor ViewModel
        public ViewModel()
        {
            DodajOglasCommand = new TestHandler(dodajOglas);
            OdstraniOglasCommand = new TestHandler(odstraniOglas);
            UrediOglasCommand = new TestHandler(urediOglas);
            listaOglasa = new ObservableCollection<Oglasi>();
        }

        public Oglasi? currSelected;

        public Oglasi? CurrSelected
        {
            get => currSelected;
            set
            {
                if (currSelected != value)
                {
                    currSelected = value;
                    OnPropertyChanged(nameof(CurrSelected));
                }
            }
        }

        private String updateNaziv;
        private String updateZnamka;
        private String updateLeto;

        public String UpdateNaziv
        {
            get { return updateNaziv; }
            set
            {
                if (updateNaziv != value)
                {
                    updateNaziv = value;
                    OnPropertyChanged(nameof(updateNaziv));
                }
            }
        }

        public String UpdateZnamka
        {
            get { return updateZnamka; }
            set
            {
                if (updateZnamka != value)
                {
                    updateZnamka = value;
                    OnPropertyChanged(nameof(updateZnamka));
                }
            }
        }

        public String UpdateLeto
        {
            get { return updateLeto; }
            set
            {
                if (updateLeto != value)
                {
                    updateLeto = value;
                    OnPropertyChanged(nameof(updateLeto));
                }
            }
        }

        private void dodajOglas(object obj)
        {
            //listaOglasa.Add(new Oglasi(naziv: "Oglas4", znamka: "BMW", starost: "10", letoProizvodnje: "2010", prevozeniKm: "200000"));
        }

        private void odstraniOglas(object obj)
        {
            if (currSelected != null)
            {
                listaOglasa.Remove(currSelected);
            }
        }
        private void urediOglas(object obj)
        {
            /*
            if (UpdateNaziv == null || UpdateZnamka == null)
            {
                MessageBox.Show("Vnesi podatke: {0}, {1}" + UpdateNaziv, UpdateZnamka);
                return;
            }
            CurrSelected.Naziv = UpdateNaziv;
            CurrSelected.Znamka = UpdateZnamka;
            CurrSelected.LetoProizvodnje = UpdateLeto;
            OnPropertyChanged(nameof(Oglasi));
            OnPropertyChanged(nameof(CurrSelected));
            */
        }

        // ICommand interface
        public class TestHandler : ICommand
        {
            #region Private Members

            readonly Action<object> _execute;
            readonly Func<object, bool> _canExecute;

            #endregion

            public TestHandler(Action<object> execute) : this(execute, null)
            {
            }

            public TestHandler(Action<object> execute, Func<object, bool> canExecute)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                if (_canExecute == null)
                    return true;
                return _canExecute.Invoke(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }
        }
    }
}
