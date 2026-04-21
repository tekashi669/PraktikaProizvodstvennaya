using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DocumentsJournal.Models;
using DocumentsJournal.Services;

namespace DocumentsJournal.Views
{
    public partial class AddEditDocumentWindow : Window
    {
        private SupabaseService _supabaseService;
        private Document _editingDocument;
        private bool _isEditMode;

        public AddEditDocumentWindow(Document document = null)
        {
            InitializeComponent();
            _supabaseService = App.SupabaseService;

            if (document != null && document.Id > 0)
            {
                _isEditMode = true;
                _editingDocument = document;
                tbTitle.Text = "✏️ Редактирование документа";
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
                tbTitle.Text = "➕ Добавление документа";
                dpDateReceived.SelectedDate = DateTime.Now;
            }

            btnSave.Click += async (s, e) => await SaveAsync();
            btnCancel.Click += (s, e) => Close();
        }

        private void LoadDocumentData()
        {
            txtIncomingNumber.Text = _editingDocument.IncomingNumber;
            dpDateReceived.SelectedDate = _editingDocument.DateReceived;
            txtSender.Text = _editingDocument.Sender;
            txtSummary.Text = _editingDocument.Summary;
            txtExecutor.Text = _editingDocument.Executor;
            if (_editingDocument.ExecutionDeadline.HasValue)
                dpExecutionDeadline.SelectedDate = _editingDocument.ExecutionDeadline.Value;

            // Установка типа документа
            for (int i = 0; i < cmbDocumentType.Items.Count; i++)
            {
                var item = cmbDocumentType.Items[i] as ComboBoxItem;
                if (item != null && item.Content.ToString() == _editingDocument.DocumentType)
                {
                    cmbDocumentType.SelectedIndex = i;
                    break;
                }
            }

            // Установка статуса
            for (int i = 0; i < cmbStatus.Items.Count; i++)
            {
                var item = cmbStatus.Items[i] as ComboBoxItem;
                if (item != null && item.Content.ToString() == _editingDocument.Status)
                {
                    cmbStatus.SelectedIndex = i;
                    break;
                }
            }

            chkIsControlled.IsChecked = _editingDocument.IsControlled;
            txtNotes.Text = _editingDocument.Notes;
        }

        private Document CollectDocumentData()
        {
            _editingDocument.IncomingNumber = txtIncomingNumber.Text;
            if (dpDateReceived.SelectedDate.HasValue)
                _editingDocument.DateReceived = dpDateReceived.SelectedDate.Value;
            _editingDocument.Sender = txtSender.Text;

            if (cmbDocumentType.SelectedItem is ComboBoxItem docTypeItem)
                _editingDocument.DocumentType = docTypeItem.Content.ToString();

            _editingDocument.Summary = txtSummary.Text;
            _editingDocument.Executor = txtExecutor.Text;

            if (dpExecutionDeadline.SelectedDate.HasValue)
                _editingDocument.ExecutionDeadline = dpExecutionDeadline.SelectedDate.Value;

            if (cmbStatus.SelectedItem is ComboBoxItem statusItem)
                _editingDocument.Status = statusItem.Content.ToString();

            _editingDocument.IsControlled = chkIsControlled.IsChecked ?? true;
            _editingDocument.Notes = txtNotes.Text;

            return _editingDocument;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtIncomingNumber.Text))
            {
                MessageBox.Show("Введите входящий номер!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSender.Text))
            {
                MessageBox.Show("Введите отправителя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSummary.Text))
            {
                MessageBox.Show("Введите содержание!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private async Task SaveAsync()
        {
            if (!Validate()) return;

            try
            {
                btnSave.IsEnabled = false;
                var doc = CollectDocumentData();

                if (_isEditMode)
                {
                    var success = await _supabaseService.UpdateDocumentAsync(doc);
                    if (success)
                    {
                        MessageBox.Show("Документ обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить документ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    var id = await _supabaseService.AddDocumentAsync(doc);
                    if (id > 0)
                    {
                        MessageBox.Show("Документ добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось добавить документ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }
    }
}