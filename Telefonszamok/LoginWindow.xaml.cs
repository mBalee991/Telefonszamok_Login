using System.Linq;
using System.Windows;
using Model;

namespace Telefonszamok
{
    public partial class LoginWindow : Window
    {
        TelefonszamContext.cnTelefonszam _context;

        public LoginWindow()
        {
            InitializeComponent();
            _context = new TelefonszamContext.cnTelefonszam();
        }

        private void btBelepes_Click(object sender, RoutedEventArgs e)
        {
            string felh = tbFelhasznalo.Text.Trim();
            string jelszo = pbJelszo.Password.Trim();

            // Egyszerű PDF szerinti ellenőrzés
            var user = _context.enSzemelyek
                .FirstOrDefault(x => x.Felhasznalonev == felh && x.JelszoHash == jelszo);

            if (user != null)
            {
                // Siker -> főablak indul
                MainWindow mw = new MainWindow();
                mw.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Hibás felhasználónév vagy jelszó!");
            }
        }
    }
}
