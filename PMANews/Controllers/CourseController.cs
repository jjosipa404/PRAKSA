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
        public async Task<IActionResult> Index()
        {
            //popis svih kolegija
            var listCourses = _context.Course.Include(c => c.Department);
            if (listCourses == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(await listCourses.ToListAsync());
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

        // GET: Course/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Set<Department>(), "Id", "Name");
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShortName,DepartmentId")] Course course)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ApplicationUser appUser = await _userManager.GetUserAsync(User);

            var dep = _context.Department.Where(d => d.Id == course.DepartmentId).FirstOrDefault();
            course.Department = dep;

            ViewBag.depid = course.DepartmentId;

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Courses", "Department", new { id = course.DepartmentId });

            }

            ViewData["DepartmentId"] = new SelectList(_context.Set<Department>(), "Id", "Name", course.Department.Name);
            return View(course);

        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            var depId = course.DepartmentId;
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Courses", "Department", new { id = depId });

        }


    }
}
