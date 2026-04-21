using Microsoft.Maui.Graphics;

namespace MobilkaDocument.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string IncomingNumber { get; set; } = "";
        public DateTime DateReceived { get; set; }
        public string Sender { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Executor { get; set; } = "";
        public DateTime? ExecutionDeadline { get; set; }
        public string Status { get; set; } = "";

        public string DateReceivedFormatted => DateReceived.ToString("dd.MM.yyyy");
        public string ExecutionDeadlineFormatted => ExecutionDeadline?.ToString("dd.MM.yyyy") ?? "Не указан";

        public Color StatusColor
        {
            get
            {
                switch (Status)
                {
                    case "На рассмотрении": return Colors.Orange;
                    case "В работе": return Colors.Blue;
                    case "На исполнении": return Colors.Purple;
                    case "Исполнен": return Colors.Green;
                    case "Снят с контроля": return Colors.Gray;
                    case "Отказ": return Colors.Red;
                    default: return Colors.Gray;
                }
            }
        }
    }
}