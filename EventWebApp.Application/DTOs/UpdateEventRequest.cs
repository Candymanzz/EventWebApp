﻿namespace EventWebApp.Application.DTOs
{
    public class UpdateEventRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MaxParticipants { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
