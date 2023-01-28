using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTR.Data;
using MovieTR.Models;

namespace MovieTR.Controllers
{
    public class TheaterController : Controller
    {
        private MovieTDBcontext _Movieconnecter;
        private readonly IWebHostEnvironment webHostEnvironment;


        public TheaterController(MovieTDBcontext dbContext ,IWebHostEnvironment hostEnvironment)
        {


            _Movieconnecter = dbContext;
            webHostEnvironment = hostEnvironment;

        }
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> TheaterView()
        {
            return View(await _Movieconnecter.Theater.ToListAsync());
        }
        [Authorize(policy: "IsAdmin")]
        public IActionResult AddTheaterView()
        {

            return View("AddTheaterView");
        }
        [Authorize(policy: "IsAdmin")]
        public IActionResult Create(Theaters theaterinput)

        {
            if (theaterinput.Tfile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ImagesU");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + theaterinput.Tfile.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                string _filepath = filePath.Split("wwwroot").Last();
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    theaterinput.Tfile.CopyTo(fileStream);
                }
                theaterinput.Timage = _filepath;

                _Movieconnecter.Theater.Add(theaterinput);
                _Movieconnecter.SaveChanges();
                ViewData["Msg"] = "You have succesfully Added a Theater";
                
            }
            return View("AddTheaterView");

        }
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _Movieconnecter.Theater == null)
            {
                return NotFound();
            }

            var theaters = await _Movieconnecter.Theater.FindAsync(id);
            if (theaters == null)
            {
                return NotFound();
            }
            return View(theaters);
        }
        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TheaterName,TheaterPrice,TheaterLocation")] Theaters theaters)
        {
            if (id != theaters.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _Movieconnecter.Update(theaters);
                    await _Movieconnecter.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheatersExists(theaters.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(TheaterView));
            }
            return View(theaters);
        }

        // GET: Theaters/Delete/5
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _Movieconnecter.Theater == null)
            {
                return NotFound();
            }

            var theaters = await _Movieconnecter.Theater
                .FirstOrDefaultAsync(m => m.Id == id);
            if (theaters == null)
            {
                return NotFound();
            }

            return View(theaters);
        }

        // POST: Theaters/Delete/5
        [Authorize(Policy = "IsAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_Movieconnecter.Theater == null)
            {
                return Problem("Entity set 'MovieTDBcontext.Theater'  is null.");
            }
            var theaters = await _Movieconnecter.Theater.FindAsync(id);
            if (theaters != null)
            {
                _Movieconnecter.Theater.Remove(theaters);
            }

            await _Movieconnecter.SaveChangesAsync();
            return RedirectToAction(nameof(TheaterView));
        }

        private bool TheatersExists(int id)
        {
            return _Movieconnecter.Theater.Any(e => e.Id == id);
        }




    }

}

