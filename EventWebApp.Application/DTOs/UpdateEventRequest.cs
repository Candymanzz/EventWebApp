namespace EventWebApp.Application.DTOs
{
    public class UpdateEventRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DateTime { get; set; }
        public string Location { get; set; } = "";
        public string Category { get; set; } = "";
        public int MaxParticipants { get; set; }
        public string ImageUrl { get; set; } = "";
    }
}
