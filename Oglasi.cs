using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace PrevoznaSredstva
{
    public class Oglasi : INotifyPropertyChanged
    {
        private string naziv;
        private string znamka;
        private string starost;
        private string letoProizvodnje;
        private string prevozeniKm;
        private BitmapSource slika;
        private string tipAvtomobila;

        public Oglasi(string naziv, string znamka, string starost, string letoProizvodnje, string prevozeniKm, BitmapSource slika, string tipAvtomobila = "limuzina")
        {
            this.naziv = naziv;
            this.znamka = znamka;
            this.starost = starost;
            this.letoProizvodnje = letoProizvodnje;
            this.prevozeniKm = prevozeniKm;
            this.tipAvtomobila = tipAvtomobila;
            this.slika = slika;
        }
        public Oglasi()
        {

        }

        public string Naziv
        {
            get { return naziv; }
            set { 
                    naziv = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Naziv)));
                }
        }

        public string Znamka
        {
            get { return znamka; }
            set { 
                    znamka = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Znamka)));
                }
        }

        public string Starost
        {
            get { return starost; }
            set { 
                    starost = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Starost)));
                }
        }

        public string LetoProizvodnje
        {
            get { return letoProizvodnje; }
            set { 
                    letoProizvodnje = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(LetoProizvodnje)));
                }
        }

        public string PrevozeniKm
        {
            get { return prevozeniKm; }
            set { 
                    prevozeniKm = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(PrevozeniKm)));
                }
        }

        public string TipAvtomobila
        {
            get { return tipAvtomobila; }
            set {
                    tipAvtomobila = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(TipAvtomobila)));
                }
        }

        [XmlIgnore]
        public BitmapSource Slika { get; set; }

        [XmlElement("Image")]
        public byte[] ImageBuffer
        {
            get
            {
                byte[] imageBuffer = null;

                if (slika != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(slika));
                        encoder.Save(stream);
                        imageBuffer = stream.ToArray();
                    }
                }

                return imageBuffer;
            }
            set
            {
                if (value == null)
                {
                    slika = null;
                }
                else
                {
                    using (var stream = new MemoryStream(value))
                    {
                        var decoder = BitmapDecoder.Create(stream,
                            BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        slika = decoder.Frames[0];
                    }
                }
            }
        }

        public bool HaveImage
        {
            get { return Slika != null; }
        }

        public override string ToString()
        {
            return $"{naziv}\n{znamka}\n{letoProizvodnje}\n{starost}\n{prevozeniKm}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
