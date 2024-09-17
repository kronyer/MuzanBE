using System.ComponentModel.DataAnnotations;

namespace Muzan.DTO
{
    public class PostCreateDto
    {
        [Required]
        public string Title { get; set; }

        public List<string> Text { get; set; } = new List<string>();

    }
}
