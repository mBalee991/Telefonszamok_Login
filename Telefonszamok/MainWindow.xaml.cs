using System.Collections.Generic;
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
        TelefonszamContext.cnTelefonszam _context;

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
                Header = "IRSZ",
                Binding = new Binding("IRSZ")
            });

            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Név",
                Binding = new Binding("Nev")
            });

            var adat = _context.enHelysegek
                               .OrderBy(x => x.Nev)
                               .ToList();

            grRacs.ItemsSource = adat;
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
                Header = "Helység",
                Binding = new Binding("Helysegnev")
            });

            grRacs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Irsz",
                Binding = new Binding("Irsz")
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
                    Helysegnev = x.enHelyseg.Nev,
                    Irsz = x.enHelyseg.IRSZ,
                    Lakcim = x.Lakcim,
                    Telefonszamok = x.Telefonszamok()
                }).ToList();

            grRacs.ItemsSource = lista;
        }

        private void miHelysegekAM_Click(object sender, RoutedEventArgs e)
        {
            grRacs.Visibility = Visibility.Hidden;
            grHelyseg.Visibility = Visibility.Visible;

            var lista = _context.enHelysegek.OrderBy(x => x.Nev).ToList();

            cbIrsz.ItemsSource = lista;
            cbHelysegnev.ItemsSource = lista;

            cbIrsz.SelectedIndex = 0;
        }

        private void cbIrsz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbIrsz.SelectedItem is not enHelyseg h)
                return;

            cbHelysegnev.SelectedItem = h;
            tbIrsz.Text = h.IRSZ;
            tbHelysegnev.Text = h.Nev;
        }
        private void cbHelysegnev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if (cbHelysegnev.SelectedItem is not enHelyseg h)
                return;

                cbIrsz.SelectedItem = h;
            tbIrsz.Text = h.IRSZ;
            tbHelysegnev.Text = h.Nev;
        }

        private void btUjHelyseg_Click(object sender, RoutedEventArgs e)
        {
            cbIrsz.IsEnabled = false;
            cbHelysegnev.IsEnabled = false;
            btUjHelyseg.IsEnabled = false;

            tbIrsz.Text = "";
            tbHelysegnev.Text = "";

            MessageBox.Show("Új helység felvitele: töltsd ki a mezőket, majd nyomd meg a 'Módosított adatpár rögzítése' gombot.");
        }

        private void btRogzit_Click(object sender, RoutedEventArgs e)
        {
            // LEHET NULL – PDF SZERINT NEM BAJ!
            var h = cbIrsz.SelectedItem as enHelyseg;

            // ÚJ helység felvitele esetén:
            if (!btUjHelyseg.IsEnabled)
            {
                h = new enHelyseg();
                _context.enHelysegek.Add(h);
            }

            h!.IRSZ = tbIrsz.Text;
            h!.Nev = tbHelysegnev.Text;

            grHelyseg.Visibility = Visibility.Hidden;
        }
        private void btVissza_Click(object sender, RoutedEventArgs e)
        {
            grHelyseg.Visibility = Visibility.Hidden;

            cbIrsz.IsEnabled = true;
            cbHelysegnev.IsEnabled = true;
            btUjHelyseg.IsEnabled = true;
        }

        private void miMentes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _context.SaveChanges();

                cbIrsz.IsEnabled = true;
                cbHelysegnev.IsEnabled = true;
                btUjHelyseg.IsEnabled = true;

                MessageBox.Show("Változtatások elmentve.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba mentéskor: " + ex.Message);
            }
        }
        private void miKilepes_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
