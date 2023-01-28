using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTR.Models
{
    public class Theaters
    {
        
    public int Id { get; set; }
    public string TheaterName { get; set; }
    public int TheaterPrice { get; set; }
    public string TheaterLocation { get; set; }
    public ICollection<Movies> Movie { get; set; }
    public string? Timage { get; set; }
    [NotMapped]
    public IFormFile? Tfile { get; set; }

    }
}
