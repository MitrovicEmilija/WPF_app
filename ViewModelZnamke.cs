using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace PrevoznaSredstva
{
    internal class ViewModelZnamke : INotifyPropertyChanged
    {
        private string newZnamka;
        private string currZnamka;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CanExecuteChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public String NewZnamka
        {
            get { return newZnamka; }
            set
            {
                if (newZnamka != value)
                {
                    newZnamka = value;
                    OnPropertyChanged(nameof(newZnamka));
                }
            }
        }

        public String CurrZnamka
        {
            get { return currZnamka; }
            set
            {
                if (currZnamka != value)
                {
                    currZnamka = value;
                    OnPropertyChanged(nameof(currZnamka));
                }
            }
        }

        public StringCollection znamkeKolekcija;

        public ViewModelZnamke()
        {
            if (Properties.Settings.Default.Znamka == null)
            {
                Properties.Settings.Default.Znamka = new StringCollection();
            }
            znamkeKolekcija = Properties.Settings.Default.Znamka;
        }
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

        public ICommand DodajZnamko => new TestHandler(AddZnamko);
        public ICommand OdstraniZnamko => new TestHandler(RemoveZnamko);
        public ICommand PosodobiZnamko => new TestHandler(EditZnamko);

        private void AddZnamko(object obj)
        {
            if (NewZnamka == null)
            {
                MessageBox.Show("Moras vnesti naziv znamke.");
                return;
            }
            Properties.Settings.Default.Znamka.Add(NewZnamka);
            Properties.Settings.Default.Save();
            znamkeKolekcija = Properties.Settings.Default.Znamka;
            OnPropertyChanged(nameof(znamkeKolekcija));
        }

        private void RemoveZnamko(object obj)
        {
            if (CurrZnamka == null)
            {
                MessageBox.Show("Moras izbrati znamko.");
                return;
            }
            Properties.Settings.Default.Znamka.Remove(CurrZnamka);
            Properties.Settings.Default.Save();
            znamkeKolekcija = Properties.Settings.Default.Znamka;
            OnPropertyChanged(nameof(znamkeKolekcija));
        }

        private void EditZnamko(object obj)
        {
            if (NewZnamka == null || CurrZnamka == null)
            {
                MessageBox.Show("Moras izbrati in vnesti naziv znamke.");
                return;
            }
            int indeks = Properties.Settings.Default.Znamka.IndexOf(CurrZnamka);
            Properties.Settings.Default.Znamka[indeks] = NewZnamka;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            znamkeKolekcija = Properties.Settings.Default.Znamka;
            OnPropertyChanged(nameof(znamkeKolekcija));
        }
    }
}
