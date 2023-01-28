

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTR.Models
{
    public class Movies
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate{ get; set; }
        //Foreign Key
        public int TheaterId { get; set; }
        public Theaters Theater { get; set; }
        public string? Mimage { get; set; }
        [NotMapped]
        public IFormFile? Mfile { get; set; } 
        
        
        

    }
}
