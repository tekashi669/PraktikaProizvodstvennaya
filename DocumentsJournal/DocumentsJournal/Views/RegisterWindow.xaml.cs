using System;
using System.Threading.Tasks;
using System.Windows;
using DocumentsJournal.Services;

namespace DocumentsJournal.Views
{
    public partial class RegisterWindow : Window
    {
        private SupabaseService _supabaseService;

        public RegisterWindow()
        {
            InitializeComponent();
            _supabaseService = App.SupabaseService;

            btnRegister.Click += async (s, e) => await RegisterAsync();
            btnBack.Click += (s, e) => Close();
        }

        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                tbStatus.Text = "❌ Введите ФИО";
                return;
            }

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

            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                tbStatus.Text = "❌ Пароли не совпадают";
                return;
            }

            if (txtPassword.Password.Length < 6)
            {
                tbStatus.Text = "❌ Пароль должен быть не менее 6 символов";
                return;
            }

            try
            {
                progressBar.Visibility = Visibility.Visible;
                btnRegister.IsEnabled = false;
                tbStatus.Text = "🔄 Регистрация...";

                var result = await _supabaseService.RegisterAsync(txtEmail.Text, txtPassword.Password, txtFullName.Text);

                if (result.Success)
                {
                    MessageBox.Show("Регистрация успешна! Проверьте почту для подтверждения.",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
                btnRegister.IsEnabled = true;
            }
        }
    }
}