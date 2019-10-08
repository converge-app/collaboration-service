using System;

namespace Application.Models.DataTransferObjects
{
    public class EventDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string OwnerId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public long TimeStamp { get; set; }
    }
}