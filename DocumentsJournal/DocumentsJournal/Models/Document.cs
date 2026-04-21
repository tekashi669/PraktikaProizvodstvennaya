using System;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace DocumentsJournal.Models
{
    public class Document
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("incoming_number")]
        public string IncomingNumber { get; set; } = "";

        [JsonPropertyName("date_received")]
        public DateTime DateReceived { get; set; }

        [JsonPropertyName("sender")]
        public string Sender { get; set; } = "";

        [JsonPropertyName("sender_address")]
        public string SenderAddress { get; set; } = "";

        [JsonPropertyName("document_type")]
        public string DocumentType { get; set; } = "";

        [JsonPropertyName("document_number")]
        public string DocumentNumber { get; set; } = "";

        [JsonPropertyName("document_date")]
        public DateTime? DocumentDate { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = "";

        [JsonPropertyName("number_of_sheets")]
        public int NumberOfSheets { get; set; } = 1;

        [JsonPropertyName("number_of_copies")]
        public int NumberOfCopies { get; set; } = 1;

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; } = "";

        [JsonPropertyName("executor")]
        public string Executor { get; set; } = "";

        [JsonPropertyName("execution_deadline")]
        public DateTime? ExecutionDeadline { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("is_controlled")]
        public bool IsControlled { get; set; } = true;

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = "";

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        // Вычисляемые поля для отображения
        [JsonIgnore]
        public string DateReceivedFormatted => DateReceived.ToString("dd.MM.yyyy");

        [JsonIgnore]
        public string ExecutionDeadlineFormatted => ExecutionDeadline?.ToString("dd.MM.yyyy") ?? "Не указан";

        [JsonIgnore]
        public Brush StatusColor
        {
            get
            {
                switch (Status)
                {
                    case "На рассмотрении": return new SolidColorBrush(Colors.Orange);
                    case "В работе": return new SolidColorBrush(Colors.DodgerBlue);
                    case "На исполнении": return new SolidColorBrush(Colors.Purple);
                    case "Исполнен": return new SolidColorBrush(Colors.SeaGreen);
                    case "Снят с контроля": return new SolidColorBrush(Colors.Gray);
                    case "Отказ": return new SolidColorBrush(Colors.Crimson);
                    default: return new SolidColorBrush(Colors.Gray);
                }
            }
        }
    }

    // Модель для создания документа (без лишних полей)
    public class DocumentCreate
    {
        [JsonPropertyName("incoming_number")]
        public string IncomingNumber { get; set; } = "";

        [JsonPropertyName("date_received")]
        public DateTime DateReceived { get; set; }

        [JsonPropertyName("sender")]
        public string Sender { get; set; } = "";

        [JsonPropertyName("sender_address")]
        public string SenderAddress { get; set; } = "";

        [JsonPropertyName("document_type")]
        public string DocumentType { get; set; } = "";

        [JsonPropertyName("document_number")]
        public string DocumentNumber { get; set; } = "";

        [JsonPropertyName("document_date")]
        public DateTime? DocumentDate { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = "";

        [JsonPropertyName("number_of_sheets")]
        public int NumberOfSheets { get; set; } = 1;

        [JsonPropertyName("number_of_copies")]
        public int NumberOfCopies { get; set; } = 1;

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; } = "";

        [JsonPropertyName("executor")]
        public string Executor { get; set; } = "";

        [JsonPropertyName("execution_deadline")]
        public DateTime? ExecutionDeadline { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";

        [JsonPropertyName("is_controlled")]
        public bool IsControlled { get; set; } = true;

        [JsonPropertyName("notes")]
        public string Notes { get; set; } = "";
    }
}