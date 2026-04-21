using System.Collections.ObjectModel;
using MobilkaDocument.Models;

namespace MobilkaDocument.Pages
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Document> _documents;

        public MainPage()
        {
            InitializeComponent();

            btnAdd.Clicked += OnAddClicked;
            btnRefresh.Clicked += OnRefreshClicked;
            btnLogout.Clicked += OnLogoutClicked;
            cvDocuments.SelectionChanged += OnDocumentSelected;

            LoadDocuments();
        }

        private void LoadDocuments()
        {
            // Тестовые данные
            _documents = new ObservableCollection<Document>
            {
                new Document { Id = 1, IncomingNumber = "001", DateReceived = DateTime.Now, Sender = "ООО Ромашка", Summary = "Запрос информации", Status = "На рассмотрении", Executor = "Иванов И.И.", ExecutionDeadline = DateTime.Now.AddDays(7) },
                new Document { Id = 2, IncomingNumber = "002", DateReceived = DateTime.Now, Sender = "Иванов И.И.", Summary = "Заявление на благоустройство", Status = "В работе", Executor = "Петров П.П.", ExecutionDeadline = DateTime.Now.AddDays(14) },
                new Document { Id = 3, IncomingNumber = "003", DateReceived = DateTime.Now, Sender = "Прокуратура", Summary = "Запрос документов", Status = "На исполнении", Executor = "Сидоров С.С.", ExecutionDeadline = DateTime.Now.AddDays(3) }
            };

            cvDocuments.ItemsSource = _documents;
            lblCount.Text = _documents.Count.ToString();
            lblStatus.Text = $"Загружено документов: {_documents.Count}";
        }

        private async void OnDocumentSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Document selectedDocument)
            {
                ((CollectionView)sender).SelectedItem = null;
                await Navigation.PushAsync(new DocumentDetailPage(selectedDocument.Id));
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditDocumentPage());
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadDocuments();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}