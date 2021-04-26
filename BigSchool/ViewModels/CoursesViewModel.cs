using BigSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BigSchool.ViewModels
{
    public class CoursesViewModel
    {
        public IEnumerable<Course>UpcommingCoures{ get; set; }
        public bool ShowAction { get; set; }
    }
}