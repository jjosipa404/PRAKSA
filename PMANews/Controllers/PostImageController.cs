using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMANews.Areas.Identity.Data;
using PMANews.Data;

namespace PMANews.Controllers
{ 
    [Authorize]
    public class PostImageController : Controller
    {
        private readonly PMFNotesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostImageController(PMFNotesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

      

        [AllowAnonymous]
        // GET: /PostImage
        public async Task<IActionResult> Index(int id)
        {
            var pMANewsContext = _context.PostImage.Include(p => p.Course).Include(p => p.Category).Include(p => p.Author).Where(i => i.CourseId == id).OrderByDescending(p => p.DateCreated);
            ViewBag.course = _context.Course.Where(c => c.Id == id).FirstOrDefault();
            ViewBag.User = await _userManager.GetUserAsync(User);
            return View(await pMANewsContext.ToListAsync());
        }

        [AllowAnonymous]
        // GET: PostImage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //dohvaca se post prema id-u koji se posalje kao parametar
            var post = await _context.PostImage
                .Include(p => p.Course)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(post.Image, 0, post.Image.Length);
            return View(post);
          
        }
       

        // GET: PostImage/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

     
        // POST: PostImage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,CategoryId")] PostImageVM post, List<IFormFile> Image, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ApplicationUser appUser = await _userManager.GetUserAsync(User);

            post.Author = appUser;
            post.AuthorId = userId;

            post.CourseId = id;
            post.Course = _context.Course.Where(c => c.Id == id).FirstOrDefault();

            

            PostImage _post = new PostImage
            {
                Title = post.Title,
                CourseId = post.CourseId,
                Course = post.Course,
                Category = post.Category,
                CategoryId = post.CategoryId,
                AuthorId = post.AuthorId,
                Author = post.Author
            };

            foreach (var item in Image)
            {
                if(item.Length > 0)
                {
                    using(var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        _post.Image = stream.ToArray();
                    }
                }
            }

            

            if (ModelState.IsValid)
            {
                _context.PostImage.Add(_post);
                await _context.SaveChangesAsync();
                return RedirectToAction("CoursePageModels","CoursePageModels", new { id = post.CourseId });
            }

            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name", post.Course.Name);
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", post.Category.Name);
            return View(post);

        }

        // GET: PostImage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.PostImage
                .Include(p => p.Course)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: PostImage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.PostImage.FindAsync(id);
            _context.PostImage.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public FileStreamResult DownloadPngFileFromDB(int id)
        {
            var _fileUpload = _context.PostImage.SingleOrDefault(p => p.Id == id);
            // _fileUpload.FileContent column type is byte
            MemoryStream ms = new MemoryStream(_fileUpload.Image);
            return new FileStreamResult(ms, "image/png");
        }

    }
}
