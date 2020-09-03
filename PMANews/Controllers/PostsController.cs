using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMANews.Areas.Identity.Data;
using PMANews.Data;

namespace PMANews.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly PMFNotesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(PMFNotesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        [AllowAnonymous]
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //dohvaca se post prema id-u koji se posalje kao parametar
            var post = await _context.Post
                .Include(p => p.Course)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);
            //dohvacaju se svi komentari koji pripadaju tom postu da se na View-u ispise ukupan broj komentara
            var comm = _context.Comment
                .Include(c => c.Post)
                .Include(c => c.User)
                .Where(c => c.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.NumberOfComments = comm.Count();
            }
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create(int courseid)
        {
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name");
            ViewBag.cid = courseid;

            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] Post post, int courseid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ApplicationUser appUser = await _userManager.GetUserAsync(User);

            post.Author = appUser;
            post.AuthorId = userId;

            var course = await _context.Course.Where(c => c.Id == courseid).FirstOrDefaultAsync();
            post.Course = course;
            post.CourseId = course.Id;
            ViewBag.cid = courseid;

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("CoursePageModels", "CoursePageModels", new { id = courseid });

            }
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name", post.Course.Name);
            return View(post);

        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ApplicationUser appUser = _context.ApplicationUser.Include(u => u.Role).Where(u => u.Id == userId).FirstOrDefault();
            ViewBag.LoggedInUser = appUser;
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name");
            ViewData["AuthorId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName");
            return View(post);

        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,AuthorId,CourseId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            post.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CoursePageModels", "CoursePageModels", new { id = post.CourseId });

            }
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name", post.Course.Name);
            ViewData["AuthorId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", post.Author.UserName);
            return View(post);

        }

     
        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.Post
                .Include(p => p.Course)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            var courseID = post.CourseId;
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("CoursePageModels", "CoursePageModels", new { id = courseID });

        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
        
   
    }
}
