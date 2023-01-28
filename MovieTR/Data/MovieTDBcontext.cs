using Microsoft.EntityFrameworkCore;
using MovieTR.Models;
using MovieTR.ViewModels;

namespace MovieTR.Data
{
    public class MovieTDBcontext:DbContext
    {
        public MovieTDBcontext(DbContextOptions<MovieTDBcontext> options) : base(options) { }
        public DbSet<Movies> Movie { get; set; }
        public DbSet<Users> User { get; set; }
        public DbSet<Theaters> Theater { get; set; }
        
        
        
        
    }
}
