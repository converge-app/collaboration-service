using System.ComponentModel.DataAnnotations;

namespace Application.Models.DataTransferObjects
{
    public class EventCreationDto
    {
        [Required]
        public string ProjectId { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Content { get; set; }
    }
}