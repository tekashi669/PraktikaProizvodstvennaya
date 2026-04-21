using MobilkaDocument.Models;

namespace MobilkaDocument.Pages
{
    public partial class AddEditDocumentPage : ContentPage
    {
        private Document _editingDocument;
        private readonly bool _isEditMode;

        public AddEditDocumentPage(Document document = null)
        {
            InitializeComponent();
            dpDateReceived.Date = DateTime.Now;

            if (document != null && document.Id > 0)
            {
                _isEditMode = true;
                _editingDocument = document;
                Title = "✏️ Редактирование документа";
                LoadDocumentData();
            }
            else
            {
                _isEditMode = false;
                _editingDocument = new Document
                {
                    DateReceived = DateTime.Now,
                    Status = "На рассмотрении"
                };
                Title = "➕ Добавление документа";
                pckStatus.SelectedIndex = 0;
            }

            btnSave.Clicked += OnSaveClicked;
            btnCancel.Clicked += OnCancelClicked;
        }

        private void LoadDocumentData()
        {
            txtIncomingNumber.Text = _editingDocument.IncomingNumber;
            dpDateReceived.Date = _editingDocument.DateReceived;
            txtSender.Text = _editingDocument.Sender;
            txtSummary.Text = _editingDocument.Summary;
            txtExecutor.Text = _editingDocument.Executor;
            if (_editingDocument.ExecutionDeadline != null)
                dpExecutionDeadline.Date = _editingDocument.ExecutionDeadline.Value;

            var statusIndex = pckStatus.Items.IndexOf(_editingDocument.Status);
            if (statusIndex >= 0) pckStatus.SelectedIndex = statusIndex;
        }

        private Document CollectDocumentData()
        {
            _editingDocument.IncomingNumber = txtIncomingNumber.Text;
            if (dpDateReceived.Date != DateTime.MinValue)
                _editingDocument.DateReceived = dpDateReceived.Date;
            _editingDocument.Sender = txtSender.Text;
            _editingDocument.Summary = txtSummary.Text;
            _editingDocument.Executor = txtExecutor.Text;
            if (dpExecutionDeadline.Date != DateTime.MinValue)
                _editingDocument.ExecutionDeadline = dpExecutionDeadline.Date;
            if (pckStatus.SelectedIndex >= 0)
                _editingDocument.Status = pckStatus.Items[pckStatus.SelectedIndex];
            return _editingDocument;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtIncomingNumber.Text))
            {
                DisplayAlert("Ошибка", "Введите входящий номер", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSender.Text))
            {
                DisplayAlert("Ошибка", "Введите отправителя", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSummary.Text))
            {
                DisplayAlert("Ошибка", "Введите содержание", "OK");
                return false;
            }
            return true;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!Validate()) return;

            try
            {
                btnSave.IsEnabled = false;

                var doc = CollectDocumentData();

                if (_isEditMode)
                {
                    // Здесь будет обновление
                    await DisplayAlert("Успех", "Документ обновлен", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    // Здесь будет добавление
                    await DisplayAlert("Успех", "Документ добавлен", "OK");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}