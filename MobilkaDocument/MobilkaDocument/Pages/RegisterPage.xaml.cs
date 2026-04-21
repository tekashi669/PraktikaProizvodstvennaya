namespace MobilkaDocument.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            btnRegister.Clicked += OnRegisterClicked;
            btnBack.Clicked += OnBackClicked;
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                lblStatus.Text = "Введите ФИО";
                return;
            }

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

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                lblStatus.Text = "Пароли не совпадают";
                return;
            }

            await DisplayAlert("Успех", "Регистрация успешна! Войдите в систему.", "OK");
            await Navigation.PopAsync();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}