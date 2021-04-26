using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BigSchool.Models;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers.Api
{
    public class CourseController : ApiController
    {
        public ApplicationDbContext _dbContext { get; set; }

        public CourseController()
        {
            _dbContext = new ApplicationDbContext();
        }

        [System.Web.Http.HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userID = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == id && c.LecturerId == userID);
            if (course.IsCanceled)
            {
                return NotFound();
            }

            course.IsCanceled = true;
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}