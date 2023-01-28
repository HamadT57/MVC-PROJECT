using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTR.Data;
using MovieTR.Models;


namespace MovieTR.Controllers
{
    
    public class MovieController : Controller
    {
        private MovieTDBcontext _Movieconnecter;
        private readonly IWebHostEnvironment webHostEnvironment;


        public MovieController(MovieTDBcontext dbContext, IWebHostEnvironment hostEnvironment)
        {


            _Movieconnecter = dbContext;
            webHostEnvironment = hostEnvironment;

        }
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> MovieView()
        {
            return View(await _Movieconnecter.Movie.ToListAsync());
        }
        [Authorize(Policy = "IsAdmin")]
        public IActionResult AddMovieView()
        {
            var Theaternames = _Movieconnecter.Theater.ToListAsync();
            
            return View();
        }
        [Authorize(Policy = "IsAdmin")]
        public IActionResult AddMovie(Movies movieinput)
        {
            if (movieinput.Mfile != null)
            {   
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ImagesU");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + movieinput.Mfile.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                string _filepath = filePath.Split("wwwroot").Last();
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    movieinput.Mfile.CopyTo(fileStream);
                }
                movieinput.Mimage = _filepath;

                _Movieconnecter.Movie.Add(movieinput);
                _Movieconnecter.SaveChanges();
                ViewData["Msg"] = "You have succesfully Added a Movie";

            }
            
            return View("AddMovieView");

        }
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> MEdit(int? id)
        {
            if (id == null || _Movieconnecter.Movie == null)
            {
                return NotFound();
            }

            var movies = await _Movieconnecter.Movie.FindAsync(id);
            if (movies == null)
            {
                return NotFound();
            }
            ViewData["TheaterId"] = new SelectList(_Movieconnecter.Theater, "Id", "Id", movies.TheaterId);
            return View(movies);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MEdit(int id, [Bind("Id,Title,Genre,Description,ReleaseDate,TheaterId")] Movies movies)
        {
            if (id != movies.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _Movieconnecter.Update(movies);
                    await _Movieconnecter.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesExists(movies.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MovieView));
            }
            ViewData["TheaterId"] = new SelectList(_Movieconnecter.Theater, "Id", "Id", movies.TheaterId);
            return View(movies);
        }
        [Authorize(Policy = "IsAdmin")]
        // GET: Movies/Delete/5
        public async Task<IActionResult> MDelete(int? id)
        {
            if (id == null || _Movieconnecter.Movie == null)
            {
                return NotFound();
            }

            var movies = await _Movieconnecter.Movie
                .Include(m => m.Theater)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }
        [Authorize(Policy = "IsAdmin")]
        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MDeleteConfirmed(int id)
        {
            if (_Movieconnecter.Movie == null)
            {
                return Problem("Entity set 'MovieTDBcontext.Movie'  is null.");
            }
            var movies = await _Movieconnecter.Movie.FindAsync(id);
            if (movies != null)
            {
                _Movieconnecter.Movie.Remove(movies);
            }

            await _Movieconnecter.SaveChangesAsync();
            return RedirectToAction(nameof(MovieView));
        }
        [Authorize(Policy = "IsAdmin")]
        private bool MoviesExists(int id)
        {
            return _Movieconnecter.Movie.Any(e => e.Id == id);
        }
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> TheaterView()
        {
            return View(await _Movieconnecter.Theater.ToListAsync());
        }
    }
}
