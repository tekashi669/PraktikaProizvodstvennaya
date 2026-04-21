using MobilkaDocument.Pages;

namespace MobilkaDocument.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            btnLogin.Clicked += OnLoginClicked;
            btnRegister.Clicked += OnRegisterClicked;

            // Для теста подставим данные
            txtEmail.Text = "test@mail.ru";
            txtPassword.Text = "123456";
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblStatus.Text = "Введите email";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblStatus.Text = "Введите пароль";
                return;
            }

            // Простая имитация входа
            if (txtEmail.Text == "test@mail.ru" && txtPassword.Text == "123456")
            {
                lblStatus.Text = "Вход выполнен!";
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                lblStatus.Text = "Неверный email или пароль";
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}