using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using DocumentsJournal.Models;

namespace DocumentsJournal.Services
{
    public class SupabaseService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private AppUser _currentUser;

        // ТВОИ КЛЮЧИ ИЗ SUPABASE
        private const string SUPABASE_URL = "https://xoxoplsoaustfqjfewmk.supabase.co";
        private const string SUPABASE_ANON_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhveG9wbHNvYXVzdGZxamZld21rIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzYzNjA2MDksImV4cCI6MjA5MTkzNjYwOX0.DA-q5amtoGxDWBRwfmYUBomA-I5eCID6kOAsZaTPprg";

        // Настройки JSON для сериализации (для .NET 6/7)
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public AppUser CurrentUser => _currentUser;

        public SupabaseService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("apikey", SUPABASE_ANON_KEY);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SUPABASE_ANON_KEY}");
        }

        // ============= АВТОРИЗАЦИЯ =============

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            try
            {
                var request = new { email, password };
                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{SUPABASE_URL}/auth/v1/token?grant_type=password", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                    _accessToken = result["access_token"]?.ToString();

                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

                    _currentUser = new AppUser { Email = email };

                    return new AuthResponse
                    {
                        Success = true,
                        Message = "Вход выполнен",
                        User = _currentUser,
                        AccessToken = _accessToken
                    };
                }

                return new AuthResponse { Success = false, Message = "Неверный email или пароль" };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<AuthResponse> RegisterAsync(string email, string password, string fullName)
        {
            try
            {
                var request = new { email, password };
                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{SUPABASE_URL}/auth/v1/signup", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                    _accessToken = result["access_token"]?.ToString();

                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

                    _currentUser = new AppUser { Email = email, FullName = fullName };

                    return new AuthResponse
                    {
                        Success = true,
                        Message = "Регистрация успешна. Подтвердите email.",
                        User = _currentUser,
                        AccessToken = _accessToken
                    };
                }

                return new AuthResponse { Success = false, Message = "Ошибка регистрации" };
            }
            catch (Exception ex)
            {
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task LogoutAsync()
        {
            _accessToken = null;
            _currentUser = null;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SUPABASE_ANON_KEY}");
        }

        // ============= РАБОТА С ДОКУМЕНТАМИ =============

        public async Task<ObservableCollection<Document>> GetDocumentsAsync()
        {
            var documents = new ObservableCollection<Document>();

            try
            {
                var response = await _httpClient.GetAsync($"{SUPABASE_URL}/rest/v1/documents?order=date_received.desc");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var docs = JsonSerializer.Deserialize<List<Document>>(json, _jsonOptions);

                    if (docs != null)
                    {
                        foreach (var doc in docs)
                        {
                            documents.Add(doc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки документов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return documents;
        }

        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{SUPABASE_URL}/rest/v1/documents?id=eq.{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var docs = JsonSerializer.Deserialize<List<Document>>(json, _jsonOptions);

                    if (docs != null && docs.Count > 0)
                    {
                        return docs[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки документа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public async Task<int> AddDocumentAsync(Document document)
        {
            try
            {
                // Используем отдельную модель для отправки
                var docToSend = new DocumentCreate
                {
                    IncomingNumber = document.IncomingNumber,
                    DateReceived = document.DateReceived,
                    Sender = document.Sender,
                    SenderAddress = document.SenderAddress,
                    DocumentType = document.DocumentType,
                    DocumentNumber = document.DocumentNumber,
                    DocumentDate = document.DocumentDate,
                    Summary = document.Summary,
                    NumberOfSheets = document.NumberOfSheets,
                    NumberOfCopies = document.NumberOfCopies,
                    Resolution = document.Resolution,
                    Executor = document.Executor,
                    ExecutionDeadline = document.ExecutionDeadline,
                    Status = document.Status,
                    IsControlled = document.IsControlled,
                    Notes = document.Notes
                };

                var content = new StringContent(JsonSerializer.Serialize(docToSend), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{SUPABASE_URL}/rest/v1/documents", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<Document>>(json, _jsonOptions);
                    return result?[0]?.Id ?? -1;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка API: {response.StatusCode}\n{error}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления документа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return -1;
        }

        public async Task<bool> UpdateDocumentAsync(Document document)
        {
            try
            {
                // Используем отдельную модель для отправки
                var docToSend = new DocumentCreate
                {
                    IncomingNumber = document.IncomingNumber,
                    DateReceived = document.DateReceived,
                    Sender = document.Sender,
                    SenderAddress = document.SenderAddress,
                    DocumentType = document.DocumentType,
                    DocumentNumber = document.DocumentNumber,
                    DocumentDate = document.DocumentDate,
                    Summary = document.Summary,
                    NumberOfSheets = document.NumberOfSheets,
                    NumberOfCopies = document.NumberOfCopies,
                    Resolution = document.Resolution,
                    Executor = document.Executor,
                    ExecutionDeadline = document.ExecutionDeadline,
                    Status = document.Status,
                    IsControlled = document.IsControlled,
                    Notes = document.Notes
                };

                var content = new StringContent(JsonSerializer.Serialize(docToSend), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{SUPABASE_URL}/rest/v1/documents?id=eq.{document.Id}")
                { Content = content };
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка API: {response.StatusCode}\n{error}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления документа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{SUPABASE_URL}/rest/v1/documents?id=eq.{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления документа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> UpdateDocumentStatusAsync(int id, string status)
        {
            try
            {
                var updateData = new { status };
                var content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{SUPABASE_URL}/rest/v1/documents?id=eq.{id}")
                { Content = content };
                var response = await _httpClient.SendAsync(request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // ============= ПОИСК И СТАТИСТИКА =============

        public async Task<ObservableCollection<Document>> SearchDocumentsAsync(string searchText, DateTime? dateFrom, DateTime? dateTo)
        {
            var documents = new ObservableCollection<Document>();

            try
            {
                var url = $"{SUPABASE_URL}/rest/v1/documents?order=date_received.desc";

                if (!string.IsNullOrEmpty(searchText))
                {
                    url += $"&incoming_number=ilike.*{Uri.EscapeDataString(searchText)}*";
                }

                if (dateFrom.HasValue)
                {
                    url += $"&date_received=gte.{dateFrom.Value:yyyy-MM-dd}";
                }

                if (dateTo.HasValue)
                {
                    url += $"&date_received=lte.{dateTo.Value:yyyy-MM-dd}";
                }

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var docs = JsonSerializer.Deserialize<List<Document>>(json, _jsonOptions);

                    if (docs != null)
                    {
                        foreach (var doc in docs)
                        {
                            documents.Add(doc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return documents;
        }

        public async Task<ObservableCollection<Document>> GetOverdueDocumentsAsync()
        {
            var documents = new ObservableCollection<Document>();

            try
            {
                var allDocs = await GetDocumentsAsync();
                var overdue = allDocs.Where(d => d.ExecutionDeadline < DateTime.Now &&
                                                 d.Status != "Исполнен" &&
                                                 d.Status != "Снят с контроля");
                foreach (var doc in overdue)
                {
                    documents.Add(doc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки просроченных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return documents;
        }

        public async Task<Dictionary<string, int>> GetStatisticsAsync()
        {
            var stats = new Dictionary<string, int>();

            try
            {
                var documents = await GetDocumentsAsync();

                foreach (var doc in documents)
                {
                    if (stats.ContainsKey(doc.Status))
                        stats[doc.Status]++;
                    else
                        stats[doc.Status] = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка статистики: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return stats;
        }
    }

    // ============= МОДЕЛИ ДЛЯ АВТОРИЗАЦИИ =============

    public class AppUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Role { get; set; } = "user";
        public string Department { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public AppUser User { get; set; }
        public string AccessToken { get; set; } = "";
    }
}