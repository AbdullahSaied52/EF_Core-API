using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AverageGradeDTO
    {
        public string course_title { get; set; }
        public decimal avg_grade { get; set; }
    }
}
