﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMANews.Areas.Identity.Data;
using PMANews.Data;
using PMFNotes.Areas.Identity.Data;

namespace PMFNotes.Controllers
{
    [Authorize]
    public class CoursePageModelsController : Controller
    {
        private readonly PMFNotesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursePageModelsController(PMFNotesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CoursePageModels(int id)
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            if (IsEnroled(appUser, id))
            {
                ViewBag.IsEnroled = true;
            }
            else
            {
                ViewBag.IsEnroled = false;
                return RedirectToAction("EnroleCourse", "CourseApplicationUser");
            }

            ViewBag.courseID = id;
            ViewBag.courseName = _context.Course.Where(c => c.Id == id).FirstOrDefault().Name;

            CoursePageModel models = new CoursePageModel();
            models.Posts = _context.Post
                .Include(p => p.Author)
                .Include(p => p.Course)
                .Where(p => p.CourseId == id)
                .OrderByDescending(p => p.DateCreated)
                .ToList();
            models.PostImages = _context.PostImage
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Course)
                .Where(p => p.CourseId == id)
                .OrderByDescending(p => p.DateCreated)
                .ToList();
            models.PostFiles = _context.PostFile
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Course)
                .Where(p => p.CourseId == id)
                .OrderByDescending(p => p.DateCreated)
                .ToList();

            if(models.PostImages.Count > 5)
            {
                models.PostImages.RemoveRange(5, models.PostImages.Count - 5);

            }
            if(models.PostFiles.Count > 5)
            {
                models.PostFiles.RemoveRange(5, models.PostFiles.Count - 5);

            }


            return View(models);

        }

        public bool IsEnroled(ApplicationUser user, int courseId)
        {
            CourseApplicationUser c = _context.CourseApplicationUser.Where(c => c.CourseId == courseId & c.ApplicationUserId == user.Id).FirstOrDefault();
            if (c == null)
            {
                return false;
            }
            return true;
        }

    }
}
