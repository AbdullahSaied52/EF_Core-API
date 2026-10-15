using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StudentInfoForCourseDTO
    {
        public string name { get; set; }
        public int completed_courses { get; set; }

        public int number_of_courses { get; set; }
    }
}
