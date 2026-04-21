using MobilkaDocument.Pages;

namespace MobilkaDocument
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}