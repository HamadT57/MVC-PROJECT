using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTR.Data;
using MovieTR.Models;

namespace MovieTR.Controllers
{
    public class UserController : Controller
    {
        private MovieTDBcontext _Movieconnecter;


        public UserController(MovieTDBcontext dbContext)
        {


            _Movieconnecter = dbContext;

        }
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UserList()
        {
            return PartialView(await _Movieconnecter.User.ToListAsync());
        }
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UEdit(int? id)
        {
            if (id == null || _Movieconnecter.User == null)
            {
                return NotFound();
            }

            var users = await _Movieconnecter.User.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UEdit(int id, [Bind("id,Name,Email,Password,ConfirmPassword,Address,PhoneNumber,Age,Role")] Users users)
        {
            if (id != users.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _Movieconnecter.Update(users);
                    await _Movieconnecter.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserList));
            }
            return View(users);
        }

        // GET: Users/Delete/5
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> UDelete(int? id)
        {
            if (id == null || _Movieconnecter.User == null)
            {
                return NotFound();
            }

            var users = await _Movieconnecter.User
                .FirstOrDefaultAsync(m => m.id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [Authorize(Policy = "IsAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UDeleteConfirmed(int id)
        {
            if (_Movieconnecter.User == null)
            {
                return Problem("Entity set 'MovieTDBcontext.User'  is null.");
            }
            var users = await _Movieconnecter.User.FindAsync(id);
            if (users != null)
            {
                _Movieconnecter.User.Remove(users);
            }

            await _Movieconnecter.SaveChangesAsync();
            return RedirectToAction(nameof(UserList));
        }

        private bool UsersExists(int id)
        {
            return _Movieconnecter.User.Any(e => e.id == id);
        }
    }
}
