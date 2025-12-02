using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Telefonszamok
{
    public partial class MainWindow : Window
    {
        private TelefonszamContext.cnTelefonszam _context;

        private bool ujHelysegMod = false;
        private bool manualUpdate = false;

        public MainWindow()
        {
            InitializeComponent();
            _context = new TelefonszamContext.cnTelefonszam();
        }

        private void miHelyseg_Click(object sender, RoutedEventArgs e)
        {
            grRacs.Visibility = Visibility.Visible;
            grHelyseg.Visibility = Visibility.Hidden;

            grRacs.Columns.Clear();
            grRacs.AutoGenerateColumns = false;

            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Név",
                Binding = new Binding("Nev")
            });
            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "IRSZ",
                Binding = new Binding("IRSZ")
            });

            grRacs.ItemsSource = _context.enHelysegek.ToList();
        }

        private void miMindenAdat_Click(object sender, RoutedEventArgs e)
        {
            grRacs.Visibility = Visibility.Visible;
            grHelyseg.Visibility = Visibility.Hidden;

            grRacs.Columns.Clear();
            grRacs.AutoGenerateColumns = false;

            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Vezetéknév",
                Binding = new Binding("Vezeteknev")
            });
            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Utónév",
                Binding = new Binding("Utonev")
            });
            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "IRSZ",
                Binding = new Binding("Irsz")
            });
            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Helység",
                Binding = new Binding("Helysegnev")
            });
            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Lakcím",
                Binding = new Binding("Lakcim")
            });
            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Telefonszámok",
                Binding = new Binding("Telefonszamok")
            });

            var lista = _context.enSzemelyek
                .Include(x => x.enHelyseg)
                .Include(x => x.enTelefonszamok)
                .ToList()
                .Select(x => new SzemelyesAdatok
                {
                    Vezeteknev = x.Vezeteknev,
                    Utonev = x.Utonev,
                    Irsz = x.enHelyseg.IRSZ,
                    Helysegnev = x.enHelyseg.Nev,
                    Lakcim = x.Lakcim,
                    Telefonszamok = string.Join(", ", x.enTelefonszamok.Select(t => t.Szam))
                }).ToList();

            grRacs.ItemsSource = lista;
        }

        private void miHelysegekAM_Click(object sender, RoutedEventArgs e)
        {
            grRacs.Visibility = Visibility.Hidden;
            grHelyseg.Visibility = Visibility.Visible;

            ujHelysegMod = false;

            ToltsdUjraHelysegekListajat();

            cbIrsz.IsEnabled = true;
            cbHelysegnev.IsEnabled = true;
            btUjHelyseg.IsEnabled = true;

            cbIrsz.SelectedIndex = 0;
        }


        private void ToltsdUjraHelysegekListajat()
        {
            var lista = _context.enHelysegek.OrderBy(x => x.Nev).ToList();
            cbIrsz.ItemsSource = lista;
            cbHelysegnev.ItemsSource = lista;
        }

        private void cbIrsz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (manualUpdate) return;
            if (cbIrsz.SelectedItem is not enHelyseg h) return;

            manualUpdate = true;
            cbHelysegnev.SelectedItem = h;
            manualUpdate = false;

            tbIrsz.Text = h.IRSZ;
            tbHelysegnev.Text = h.Nev;
        }

        private void cbHelysegnev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (manualUpdate) return;
            if (cbHelysegnev.SelectedItem is not enHelyseg h) return;

            manualUpdate = true;
            cbIrsz.SelectedItem = h;
            manualUpdate = false;

            tbIrsz.Text = h.IRSZ;
            tbHelysegnev.Text = h.Nev;
        }

        private void btUjHelyseg_Click(object sender, RoutedEventArgs e)
        {
            ujHelysegMod = true;

            cbIrsz.IsEnabled = false;
            cbHelysegnev.IsEnabled = false;
            btUjHelyseg.IsEnabled = false;

            tbIrsz.Text = "";
            tbHelysegnev.Text = "";

            MessageBox.Show("Új helység felvitele: töltsd ki a mezőket, majd kattints a 'Módosított adatpár rögzítése' gombra.");
        }

        private void btRogzit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbIrsz.Text) ||
                string.IsNullOrWhiteSpace(tbHelysegnev.Text))
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!");
                return;
            }

            enHelyseg h;

            if (ujHelysegMod)
            {
                h = new enHelyseg();
                _context.enHelysegek.Add(h);
            }
            else
            {
                if (cbIrsz.SelectedItem is not enHelyseg kivalasztott)
                {
                    MessageBox.Show("Nincs kijelölt helység!");
                    return;
                }
                h = kivalasztott;
            }

            h.IRSZ = tbIrsz.Text;
            h.Nev = tbHelysegnev.Text;

            MessageBox.Show("Módosítás rögzítve. Véglegesítéshez válaszd a Fájl → Mentés menüpontot!");
        }

        private void btVissza_Click(object sender, RoutedEventArgs e)
        {
            ujHelysegMod = false;

            cbIrsz.IsEnabled = true;
            cbHelysegnev.IsEnabled = true;
            btUjHelyseg.IsEnabled = true;

            tbIrsz.Text = "";
            tbHelysegnev.Text = "";

            grHelyseg.Visibility = Visibility.Hidden;
        }

        private void miMentes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _context.SaveChanges();
                MessageBox.Show("Változtatások elmentve.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba mentéskor: " + ex.Message);
            }

            ujHelysegMod = false;

            cbIrsz.IsEnabled = true;
            cbHelysegnev.IsEnabled = true;
            btUjHelyseg.IsEnabled = true;

            ToltsdUjraHelysegekListajat();

            tbIrsz.Text = "";
            tbHelysegnev.Text = "";
        }



        //--------------------------------------------------------------------------
        private void miKilepes_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
