namespace DocumentsJournal.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ColorCode { get; set; } = "";
        public int SortOrder { get; set; }
    }
}