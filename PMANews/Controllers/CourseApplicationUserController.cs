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
    public class CourseApplicationUserController : Controller
    {
        private readonly PMFNotesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseApplicationUserController(PMFNotesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
    
        public async Task<IActionResult> Index()
        {
            //popis svih kolegija
            var listCourses = _context.CourseApplicationUser.Include(cu => cu.ApplicationUser).Include(cu => cu.Course);
            if (listCourses == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await listCourses.ToListAsync());
        }

        public IActionResult EnroleCourse(int? courseId)
        {
            
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name");
            return View();
        }

        // POST: Course/EnroleCourse/2
        [HttpPost, ActionName("EnroleCourse")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnroleCourse([Bind("CourseId")] CourseApplicationUser cu)
        {

            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            cu.ApplicationUserId = appUser.Id;
            cu.ApplicationUser = appUser;

            cu.CourseId = cu.CourseId;

            if (ModelState.IsValid)
            {
                if(IsEnroled(appUser, cu.CourseId))
                {
                    return RedirectToAction("MyCourses", "Course");
                }
                _context.CourseApplicationUser.Add(cu);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyCourses", "Course");
            }

            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "Id", "Name", cu.Course.Name);
            return View(cu);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.CourseApplicationUser.Include(cu => cu.Course).FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Course/QuitCourse/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cu = await _context.CourseApplicationUser.FindAsync(id);
            _context.CourseApplicationUser.Remove(cu);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCourses", "Course");
        }

        public bool IsEnroled(ApplicationUser user, int courseId)
        {
            CourseApplicationUser c = _context.CourseApplicationUser.Where(c => c.CourseId == courseId & c.ApplicationUserId == user.Id).FirstOrDefault();
            if(c == null)
            {
                return false;
            }
            return true;
        }
    }
}
