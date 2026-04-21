namespace DocumentsJournal.Models
{
    public class Executor
    {
        public int Id { get; set; }
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Position { get; set; } = "";
        public int? DepartmentId { get; set; }
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }
}