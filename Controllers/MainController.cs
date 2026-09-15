using EF_API.DBcontext;
using EF_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
//packeges for EF Version 8.0.0
//Microsoft.EntityFrameworkCore
//Microsoft.EntityFrameworkCore.SqlServer
//Microsoft.EntityFrameworkCore.Tools
//Microsoft.Extensions.Configuration
//Microsoft.Extensions.Configuration.Binder
//Microsoft.Extensions.Configuration.Json

//scaffolding
//tools-> nuegetpackage manager-> package manager console
// Scaffold-DbContext "Server=localhost;Database=TrainingCenterDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models


namespace EF_API.Controllers
{
    [Route("api/Main")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly TrainingCenterDbContext _context;

        public MainController(TrainingCenterDbContext context)
        {
            _context = context;
        }

        [HttpGet("List-All-Students")]
        public async Task<ActionResult<IEnumerable<Student>>> ListAllStudents()
        {
            var student =await  _context.Students.AsNoTracking().ToListAsync();
            
            return Ok( student);
        }

        [HttpGet("Average-Grade-for-each-Course")]
        public async Task<ActionResult> AverageGradeForEachCourse()
        {
            var grade = await _context.Enrollments.GroupBy(e => e.Course.Title).Select(g => new
            {
                course_title = g.Key,
                avg_grade = g.Average(e=>e.FinalGrade)
            }).ToListAsync();
            return Ok(grade);
        }
        
        [HttpGet("List-All-Students-Order-by-date-of-enrollment")]
        public async Task<ActionResult> ListOrderOfStudentsByDate()
        {
            var students = await _context.Enrollments
                .OrderBy(e => e.EnrollmentDate).Select(e => new
            {
                student_name=e.Student.FirstName+" "+e.Student.LastName,
                date=e.EnrollmentDate
            }).ToListAsync();
            return Ok(students);
        }

        [HttpGet("List-Graduated-Students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< ActionResult<IEnumerable<Student>>>ListGraduated()

        {
            var graduated_query =await _context.Students.AsNoTracking().Where(s => s.Status == "Graduated").ToListAsync();
            if (graduated_query.Count == 0) return NotFound("no one is graduated");
            return Ok( graduated_query);
        }

        [HttpGet("number-of-students-for-each-status")]
        public async Task< ActionResult> NumberOfEachStatus()
        {
            var num = await _context.Students.GroupBy(s => s.Status)
                .Select(g => new
                {
                    status_1 = g.Key,
                    count_1 = g.Count()
                })
                .ToListAsync();//.ToDictionaryAsync(d => d.status_1, d => d.count_1);
            return Ok( num);
        }

        [HttpPut("Update-Student/{student_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> Update(int student_id,[FromBody]StudentStatus status)
        {
            if (student_id < 1) return BadRequest("the Student ID must be greater than 0");

            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == student_id);
            if (student == null) return NotFound("the Student is not Exist");

            student.Status = status.ToString();


            await _context.SaveChangesAsync();
            return Ok(student);
        }


        [HttpDelete("Delete-Student/{student_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int student_id)
        {
            if (student_id < 1) return BadRequest("the ID must be greater than 0");
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == student_id);
            if (student == null) return NotFound("this student is not here");


            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("Get-Enrollments-for-A-student{student_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EnrollmentsForAStudent(int student_id)
        {
            if (student_id < 1) return BadRequest("the ID must be greater than 0");

            var student = await _context.Students.Where(s=>s.StudentId==student_id)
                .Select(s => new
                {
                    fullname = s.FirstName + " " + s.LastName,
                     enrolled_at= s.Enrollments.Select(e=>new
                     {
                         course_title=e.Course.Title,
                         grade=e.FinalGrade,
                         status=e.Status
                     })
                }).FirstOrDefaultAsync();

            if (student == null) return NotFound("this student is not here");

            return Ok( student);

        }

        [HttpPut("Updated-Enrollment-Status-for-student/{student_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateEnrollmentStatus(int student_id,int grade ,EnrollmentStatus status,string course_code)
        {
            if (student_id < 1)
                return BadRequest("student id must be greater than 0");

            var enrollment = await _context.Enrollments.Where(e => e.StudentId == student_id &&
            e.Course.Code == course_code).FirstOrDefaultAsync();

            if (enrollment == null)
                return NotFound("this student is not enrolled in any courses");
            
            enrollment.Status = status.ToString();
            if (grade < 0)
                return BadRequest("grade must be greater than or equal 0");
            enrollment.FinalGrade = grade;
            enrollment.CompletionDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(enrollment);

        }

        [HttpGet("Get-Students-of-Specific-Course/{course_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetStudentsforSpecificCourse(int course_id)
        {
            if (course_id < 0) return BadRequest("the id must be greater than 0");

            var students =await _context.Enrollments.Where(e => e.CourseId == course_id)
                .Select(e => new
                {
                    student_name=e.Student.FirstName+" "+e.Student.LastName,
                    student_status=e.Status
                }).
                ToListAsync();
            if (students == null) return NotFound("this course is not exist");
            return Ok(students);
        }

        [HttpGet("Get-completed-courses-for-a-student/{student_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompletedCoursesForStudent(int student_id)
        {
            if (student_id < 1)
                return BadRequest("student id must be greater than 0");

            var complete = await _context.Students.Where(e => e.StudentId == student_id).Select(e => new
            {
                student_name = e.FirstName + " " + e.LastName,
                number_of_courses = e.Enrollments.Count(),
                completed_courses = e.Enrollments.Count(en => en.Status == "Completed")
            }).FirstOrDefaultAsync();

            if (complete == null) return NotFound("this student is not exist");

            return Ok( complete);
        }

        [HttpGet("Get-Full-student-profile/{student_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetFullProfile(int student_id)
        {
            if (student_id < 1)
                return BadRequest("student id must be greater than 0");

            var student = await _context.Students.Where(s => s.StudentId == student_id).Select(s => new
            {
                profile = s.StudentProfile,
                courses = s.Enrollments.Select(c=>new
                {
                    title=c.Course.Title,
                    grade=c.FinalGrade,
                    status=c.Status,
                    instructor=c.Course.Instructor.FirstName+" "+c.Course.Instructor.LastName
                })

            }).FirstOrDefaultAsync();

            if (student == null) return NotFound("this student is not exist");

            return Ok(student);
        }

        [HttpGet("Instructors-Report/{instructor_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> InstructorReport(int instructor_id)
        {
            if (instructor_id < 1)
                return BadRequest("insrtuctor id must be greater than 0");

            var report =await _context.Instructors.Where(e => e.InstructorId == instructor_id).Select(c => new
            {
                instructor_name = c.FirstName + " " + c.LastName,
                total_courses = c.Courses.Count(),
                total_enrolled_students = c.Courses.SelectMany(s=>s.Enrollments).Count(),
                average = c.Courses.SelectMany(s=>s.Enrollments).Average(a=>a.FinalGrade)
            }).FirstOrDefaultAsync();

            if (report == null) return NotFound("this instructor not has a report");


            return Ok(report);
        }

    }

}
