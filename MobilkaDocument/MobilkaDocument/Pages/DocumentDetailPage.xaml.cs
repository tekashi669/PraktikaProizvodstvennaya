using MobilkaDocument.Models;

namespace MobilkaDocument.Pages
{
    public partial class DocumentDetailPage : ContentPage
    {
        private Document _document;
        private readonly int _documentId;

        public DocumentDetailPage(int documentId)
        {
            InitializeComponent();
            _documentId = documentId;

            btnEdit.Clicked += OnEditClicked;
            btnDelete.Clicked += OnDeleteClicked;

            LoadDocument();
        }

        private void LoadDocument()
        {
            // Тестовые данные
            _document = new Document
            {
                Id = _documentId,
                IncomingNumber = $"00{_documentId}",
                DateReceived = DateTime.Now,
                Sender = "ООО Ромашка",
                Summary = "Запрос информации",
                Status = "На рассмотрении",
                Executor = "Иванов И.И.",
                ExecutionDeadline = DateTime.Now.AddDays(7)
            };

            lblNumber.Text = _document.IncomingNumber;
            lblStatus.Text = _document.Status;
            borderStatus.BackgroundColor = _document.StatusColor;
            lblDateReceived.Text = $"📅 Дата: {_document.DateReceivedFormatted}";
            lblSender.Text = $"🏢 Отправитель: {_document.Sender}";
            lblSummary.Text = $"📄 Содержание: {_document.Summary}";
            lblExecutor.Text = $"👤 Исполнитель: {_document.Executor}";
            lblDeadline.Text = $"⏰ Срок: {_document.ExecutionDeadlineFormatted}";
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditDocumentPage(_document));
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Подтверждение", "Удалить документ?", "Да", "Нет");
            if (answer)
            {
                await Navigation.PopAsync();
            }
        }
    }
}