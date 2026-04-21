using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using MobilkaDocument.Models;

namespace MobilkaDocument.Services
{
    public class SupabaseService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken = "";

        private const string SUPABASE_URL = "https://xoxoplsoaustfqjfewmk.supabase.co";
        private const string SUPABASE_ANON_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhveG9wbHNvYXVzdGZxamZld21rIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzYzNjA2MDksImV4cCI6MjA5MTkzNjYwOX0.DA-q5amtoGxDWBRwfmYUBomA-I5eCID6kOAsZaTPprg";

        public SupabaseService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("apikey", SUPABASE_ANON_KEY);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SUPABASE_ANON_KEY}");
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            try
            {
                Debug.WriteLine($"Попытка входа: {email}");

                var request = new { email, password };
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{SUPABASE_URL}/auth/v1/token?grant_type=password", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Ответ: {response.StatusCode} - {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                    _accessToken = result?["access_token"]?.ToString() ?? "";

                    _httpClient.DefaultRequestHeaders.Remove("Authorization");
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

                    return new LoginResult
                    {
                        Success = true,
                        Message = "Вход выполнен",
                        AccessToken = _accessToken,
                        User = new User { Email = email, IsAuthenticated = true }
                    };
                }

                return new LoginResult { Success = false, Message = $"Ошибка: {response.StatusCode} - {responseBody}" };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Исключение: {ex.Message}");
                return new LoginResult { Success = false, Message = ex.Message };
            }
        }

        public Task LogoutAsync()
        {
            _accessToken = "";
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SUPABASE_ANON_KEY}");
            return Task.CompletedTask;
        }

        public async Task<List<Document>> GetDocumentsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{SUPABASE_URL}/rest/v1/documents?order=date_received.desc");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Document>>(json) ?? new List<Document>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка GetDocuments: {ex.Message}");
            }
            return new List<Document>();
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{SUPABASE_URL}/rest/v1/documents?id=eq.{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var docs = JsonConvert.DeserializeObject<List<Document>>(json);
                    return docs?.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка GetDocumentById: {ex.Message}");
            }
            return null;
        }

        public async Task<int> AddDocumentAsync(Document document)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{SUPABASE_URL}/rest/v1/documents", content);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<Document>>(json);
                    return result?.FirstOrDefault()?.Id ?? -1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка AddDocument: {ex.Message}");
            }
            return -1;
        }

        public async Task<bool> UpdateDocumentAsync(Document document)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{SUPABASE_URL}/rest/v1/documents?id=eq.{document.Id}")
                { Content = content };
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка UpdateDocument: {ex.Message}");
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
                Debug.WriteLine($"Ошибка DeleteDocument: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDocumentStatusAsync(int id, string status)
        {
            try
            {
                var updateData = new { status };
                var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{SUPABASE_URL}/rest/v1/documents?id=eq.{id}")
                { Content = content };
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка UpdateDocumentStatus: {ex.Message}");
                return false;
            }
        }
    }
}