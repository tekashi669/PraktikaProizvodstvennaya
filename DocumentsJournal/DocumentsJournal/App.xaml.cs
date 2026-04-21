using System.Windows;
using DocumentsJournal.Services;
using DocumentsJournal.Views;

namespace DocumentsJournal
{
    public partial class App : Application
    {
        public static SupabaseService SupabaseService { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SupabaseService = new SupabaseService();

            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}