using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DocumentsJournal.Services;

namespace DocumentsJournal.Views
{
    public partial class LoginWindow : Window
    {
        private SupabaseService _supabaseService;

        public LoginWindow()
        {
            InitializeComponent();
            _supabaseService = App.SupabaseService;

            btnLogin.Click += async (s, e) => await LoginAsync();
            btnRegister.Click += (s, e) => OpenRegisterWindow();

            txtEmail.KeyDown += async (s, e) => { if (e.Key == Key.Enter) await LoginAsync(); };
            txtPassword.KeyDown += async (s, e) => { if (e.Key == Key.Enter) await LoginAsync(); };
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                tbStatus.Text = "❌ Введите email";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                tbStatus.Text = "❌ Введите пароль";
                return;
            }

            try
            {
                progressBar.Visibility = Visibility.Visible;
                btnLogin.IsEnabled = false;
                tbStatus.Text = "🔄 Вход...";

                var result = await _supabaseService.LoginAsync(txtEmail.Text, txtPassword.Password);

                if (result.Success)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    tbStatus.Text = $"❌ {result.Message}";
                }
            }
            catch (Exception ex)
            {
                tbStatus.Text = $"❌ {ex.Message}";
            }
            finally
            {
                progressBar.Visibility = Visibility.Collapsed;
                btnLogin.IsEnabled = true;
            }
        }

        private void OpenRegisterWindow()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Owner = this;
            registerWindow.ShowDialog();
        }
    }
}