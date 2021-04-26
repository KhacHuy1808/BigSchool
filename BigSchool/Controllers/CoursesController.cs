using BigSchool.Models;
using BigSchool.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Courses
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place
            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();

            return RedirectToAction("Mine", "Courses");
        }
        [System.Web.Http.Authorize]
        public ActionResult Attending()
        {
            var UserID = User.Identity.GetUserId();

            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == UserID)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();
            var viewModel = new CourseViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }
        [System.Web.Http.Authorize]
        public ActionResult Mine()
        {
            var UserID = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(c => c.LecturerId == UserID && c.DateTime > DateTime.Now)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();

            return View(courses);
        }

        [System.Web.Http.Authorize]
        public ActionResult Edit(int id)
        {
            var UserID = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == id && c.LecturerId == UserID);

            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = course.DateTime.ToString("dd/M/yyyy"),
                Time = course.DateTime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                heading = "Edit Course",
                Id = course.Id

            };
            return View("Create", viewModel);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }

            var userID = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userID);
            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


    }
}