using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class InstructorReportDTO
    {
        public string name { get; set; }
        public decimal average { get; set; }
        public int total_enrolled_students { get; set; }
        public int total_courses { get; set; }
    }
}
