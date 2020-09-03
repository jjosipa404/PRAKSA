using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class PostFileController : Controller
    {
        private readonly PMFNotesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnv;

        public PostFileController(PMFNotesContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnv)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnv = hostingEnv;
        }

  
        // GET: /PostFile/Index
        public async Task<IActionResult> Index()
        {
            ViewBag.User = await _userManager.GetUserAsync(User);
            var pMANewsContext = _context.PostFile
                .Include(p => p.Course)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .OrderBy(p => p.Course.Name);
            return View(await pMANewsContext.ToListAsync());
        }

        // GET: /PostFile/IndexCourse/2
        public async Task<IActionResult> IndexCourse(int id)
        {
            ViewBag.User = await _userManager.GetUserAsync(User);
            ViewBag.course = _context.Course.Where(c => c.Id == id).FirstOrDefault();
            var pMANewsContext = _context.PostFile
                .Include(p => p.Course)
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Where(f => f.CourseId == id)
                .OrderBy(p => p.DateCreated);
            return View(await pMANewsContext.ToListAsync());
        }

        // GET: /PostFile/MyPostFiles
        public async Task<IActionResult> MyPostFiles()
        {
            //popis kolegija po korisniku
            ApplicationUser user = await _userManager.GetUserAsync(User);
            ViewBag.User = await _userManager.GetUserAsync(User);
            var listPosts = _context.PostFile
                .Include(p => p.Author)
                .Include(p => p.Course)
                .Include(p => p.Category)
                .Where(p => p.AuthorId == user.Id)
                .OrderByDescending(p => p.DateCreated);

            if (listPosts == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(await listPosts.ToListAsync());
        }


        public IActionResult DownloadFile(string filePath) //prezentacija.png
        {
            // for pdf download:
            // byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            // string fileName = "file.pdf";
            // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            // for preview pdf and the download:
            var fileName = Path.GetFileName(filePath);
            var path = Path.Combine(_hostingEnv.WebRootPath, "files", fileName);
            var stream = new FileStream(path, FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

        // GET: PostFile/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        // POST: PostFile/Create/2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,CategoryId,File")] PostFileVM post, int courseid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ApplicationUser appUser = await _userManager.GetUserAsync(User);

            post.Author = appUser;
            post.AuthorId = userId;

            post.CourseId = courseid;

            //upload files to wwwroot
            var fileName = Path.GetFileName(post.File.FileName);
            //judge if it is pdf file
            string ext = Path.GetExtension(post.File.FileName);
            if (ext.ToLower() != ".pdf")
            {
                return View();
            }
            var filePath = Path.Combine(_hostingEnv.WebRootPath, "files", fileName);

            using (var fileSteam = new FileStream(filePath, FileMode.Create))
            {
                await post.File.CopyToAsync(fileSteam);
            }

            PostFile _post = new PostFile
            {
                Title = post.Title,
                Course = post.Course,
                CourseId = post.CourseId,
                Category = post.Category,
                CategoryId = post.CategoryId,
                Author = post.Author,
                AuthorId = post.Author.Id,
                FilePath = fileName 

            };

            if (ModelState.IsValid)
            {
                _context.Add(_post);
                await _context.SaveChangesAsync();
                return RedirectToAction("CoursePageModels","CoursePageModels", new { id = post.CourseId });
            }

            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name", post.Course.Name);
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", post.Category.Name);
           
            return View(post);

        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.PostFile
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

        // POST: PostFile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.PostFile.FindAsync(id);
            _context.PostFile.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      


    }
}

