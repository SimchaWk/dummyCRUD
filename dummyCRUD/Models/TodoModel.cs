namespace DummyCRUD.Models
{
    public class TodoModel
    {
        public int Id { get; set; }
        public string Todo { get; set; } = string.Empty;
        public bool Completed { get; set; }
        public int UserId { get; set; }
    }
}
