using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DocumentsJournal.Models;
using DocumentsJournal.Services;
using DocumentsJournal.Views;

namespace DocumentsJournal
{
    public partial class MainWindow : Window
    {
        private SupabaseService _supabaseService;
        private ObservableCollection<Document> _documents;

        public MainWindow()
        {
            InitializeComponent();
            _supabaseService = App.SupabaseService;

            // Привязка событий
            btnAdd.Click += BtnAdd_Click;
            btnRefresh.Click += async (s, e) => await LoadDocuments();
            btnStatistics.Click += async (s, e) => await ShowStatistics();
            btnOverdue.Click += async (s, e) => await ShowOverdueDocuments();
            btnLogout.Click += BtnLogout_Click;
            btnSearch.Click += BtnSearch_Click;
            btnClearSearch.Click += BtnClearSearch_Click;

            // Управление кнопками редактирования/удаления
            dgDocuments.SelectionChanged += (s, e) =>
            {
                btnEdit.IsEnabled = dgDocuments.SelectedItem != null;
                btnDelete.IsEnabled = dgDocuments.SelectedItem != null;
            };

            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += async (s, e) => await DeleteDocument();

            // Загрузка данных при открытии окна
            Loaded += async (s, e) => await LoadDocuments();
        }

        private async Task LoadDocuments()
        {
            try
            {
                tbStatusMessage.Text = "Загрузка документов...";

                _documents = await _supabaseService.GetDocumentsAsync();

                dgDocuments.ItemsSource = _documents;
                tbRecordsCount.Text = _documents.Count.ToString();
                tbStatusMessage.Text = $"✅ Загружено документов: {_documents.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                tbStatusMessage.Text = $"❌ Ошибка: {ex.Message}";
            }
        }

        private async Task ShowStatistics()
        {
            try
            {
                var stats = await _supabaseService.GetStatisticsAsync();
                var message = "📊 СТАТИСТИКА ДОКУМЕНТОВ\n\n";

                foreach (var stat in stats)
                {
                    message += $"• {stat.Key}: {stat.Value} шт.\n";
                }

                message += $"\n📋 Всего: {_documents.Count} документов";

                MessageBox.Show(message, "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ShowOverdueDocuments()
        {
            try
            {
                tbStatusMessage.Text = "Поиск просроченных...";
                var overdueDocs = await _supabaseService.GetOverdueDocumentsAsync();

                if (overdueDocs.Count > 0)
                {
                    dgDocuments.ItemsSource = overdueDocs;
                    tbRecordsCount.Text = overdueDocs.Count.ToString();
                    tbStatusMessage.Text = $"⚠️ Просроченных документов: {overdueDocs.Count}";
                }
                else
                {
                    MessageBox.Show("Просроченных документов нет.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadDocuments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteDocument()
        {
            var selectedDoc = dgDocuments.SelectedItem as Document;
            if (selectedDoc == null) return;

            var result = MessageBox.Show($"Удалить документ №{selectedDoc.IncomingNumber} от {selectedDoc.Sender}?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var success = await _supabaseService.DeleteDocumentAsync(selectedDoc.Id);
                if (success)
                {
                    await LoadDocuments();
                    tbStatusMessage.Text = "✅ Документ удален";
                }
                else
                {
                    MessageBox.Show("Не удалось удалить документ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddEditDocumentWindow();
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                _ = LoadDocuments();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedDoc = dgDocuments.SelectedItem as Document;
            if (selectedDoc == null) return;

            var window = new AddEditDocumentWindow(selectedDoc);
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                _ = LoadDocuments();
            }
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbStatusMessage.Text = "Поиск...";
                DateTime? dateFrom = dpDateFrom.SelectedDate;
                DateTime? dateTo = dpDateTo.SelectedDate;

                var filtered = await _supabaseService.SearchDocumentsAsync(txtSearch.Text, dateFrom, dateTo);
                dgDocuments.ItemsSource = filtered;
                tbRecordsCount.Text = filtered.Count.ToString();
                tbStatusMessage.Text = $"🔍 Найдено документов: {filtered.Count}";
                searchPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            dpDateFrom.SelectedDate = null;
            dpDateTo.SelectedDate = null;
            dgDocuments.ItemsSource = _documents;
            tbRecordsCount.Text = _documents.Count.ToString();
            tbStatusMessage.Text = $"📋 Загружено документов: {_documents.Count}";
            searchPanel.Visibility = Visibility.Collapsed;
        }

        private async void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            await _supabaseService.LogoutAsync();

            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}