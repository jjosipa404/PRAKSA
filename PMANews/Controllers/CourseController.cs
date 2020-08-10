using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMANews.Areas.Identity.Data;
using PMANews.Data;

namespace PMANews.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly PMFNotesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(PMFNotesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Course/IndexAll
        public async Task<IActionResult> IndexAll()
        {
            //popis svih kolegija
            var listCourses = _context.Course.Include(c => c.Department);
            if (listCourses == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(await listCourses.ToListAsync());
        }

        // GET: /Course/1
        public async Task<IActionResult> Index(int id)//courseId
        {
            //popis postova po kolegiju
            var posts = _context.Post
                .Include(p => p.Course)
                .Include(p => p.Author)
                .Where(p => p.CourseId == id)
                .OrderByDescending(p => p.DateCreated);
            ViewBag.courseID = id;
            return View(await posts.ToListAsync());
        }

        // GET: /Course/MyCourses
        public async Task<IActionResult> MyCourses()
        {
            //popis kolegija po korisniku
            ApplicationUser user = await _userManager.GetUserAsync(User);
            var listCourses = _context.CourseApplicationUser.Include(cu => cu.ApplicationUser).Include(cu => cu.Course).Where(cu => cu.ApplicationUserId == user.Id);

            if (listCourses == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(await listCourses.ToListAsync());
        }


       
    }
}
