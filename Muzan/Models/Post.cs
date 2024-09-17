using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Muzan.Models

{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Text { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? Posted { get; set; }
        public bool? IsPosted { get; set; } = false; 
    }
}
