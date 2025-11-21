using System.Configuration;
using System.Data;
using System.Windows;

namespace Telefonszamok
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoginWindow lw = new LoginWindow();
            lw.Show();
        }
    }

}
