using Application.Intrefaces;
using Domain.DTOs;
using EF_API.DBcontext;
using EF_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
        private readonly IStudentRepositry _studenRepository;

        public MainController(IStudentRepositry studentRepositry)
        {
            _studenRepository = studentRepositry;
        }

        [HttpGet("List-All-Students")]
        public async Task<ActionResult<IEnumerable<Student>>> ListAllStudents()
        {
            var student =await _studenRepository.ListAllAsync();
            
            return Ok( student);
        }

        [HttpGet("Dynamic-Filter-for-Students")]
        public async Task<ActionResult> FilterStudents(string? first_name,string? status,bool sorted_be_name=false)
        {
            var list = await _studenRepository.filterStudentsAsync(first_name, status, sorted_be_name);

            return Ok(list);
        }

        [HttpGet("Average-Grade-for-each-Course")]
        public async Task<ActionResult> AverageGradeForEachCourse()
        {
            var grade =await _studenRepository.AverageGradeForEachCourse();
            
            return Ok(grade);
        }
        
        [HttpGet("List-All-Students-Order-by-date-of-enrollment")]
        public async Task<ActionResult> ListOrderOfStudentsByDate()
        {
            var students = await _studenRepository.ListOrderOfStudentsByDate();
            return Ok(students);
        }

        [HttpGet("List-Graduated-Students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< ActionResult<IEnumerable<Student>>>ListGraduated()

        {
            var graduated = await _studenRepository.ListGraduated();
            if (graduated.Count == 0) return NotFound("no one is graduated");
            return Ok( graduated);
        }

        [HttpGet("number-of-students-for-each-status")]
        public async Task< ActionResult> NumberOfEachStatus()
        {
            var num = await _studenRepository.NumberOfEachStatus();
            return Ok( num);
        }

        [HttpPut("Update-Student/{student_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> Update(int student_id,[FromBody]StudentStatus status)// input status : 0 or 1 or 2
        {
            if (student_id < 1) return BadRequest("the Student ID must be greater than 0");

            var check = await _studenRepository.Update(student_id,status);
            if (check == false) return NotFound("the Student is not Exist");

            _studenRepository.savechanges();

            return Ok("Updated");
        }


        [HttpDelete("Delete-Student/{student_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int student_id)
        {
            if (student_id < 1) return BadRequest("the ID must be greater than 0");
            var check = await _studenRepository.Delete(student_id);
            if (check == false) return NotFound("this student is not here");

            _studenRepository.savechanges();
            return NoContent();
        }

        [HttpGet("Get-Enrollments-for-A-student{student_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EnrollmentsForAStudent(int student_id)
        {
            if (student_id < 1) return BadRequest("the ID must be greater than 0");

            var student = await _studenRepository.EnrollmentsForAStudent(student_id);

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
            if (grade < 0)
                return BadRequest("grade must be greater than or equal 0");

            var edit = await _studenRepository.UpdateEnrollmentStatus(student_id, grade, status, course_code);
            _studenRepository.savechanges();
            return Ok("updated");

        }

        [HttpGet("Get-Students-of-Specific-Course/{course_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetStudentsforSpecificCourse(int course_id)
        {
            if (course_id < 0) return BadRequest("the id must be greater than 0");

            var students = await _studenRepository.GetStudentsforSpecificCourse(course_id);
            if (students.Count == 0) return NotFound("this course is not exist");
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

            var complete = await _studenRepository.GetCompletedCoursesForStudent(student_id);

            if (complete == null) return NotFound("this student is not exist");

            return Ok( complete);
        }

        [HttpGet("Instructors-Report/{instructor_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> InstructorReport(int instructor_id)
        {
            if (instructor_id < 1)
                return BadRequest("insrtuctor id must be greater than 0");

            var report = await _studenRepository.InstructorReport(instructor_id);

            if (report == null) return NotFound("this instructor not has a report");

            return Ok(report);
        }

    }

}
