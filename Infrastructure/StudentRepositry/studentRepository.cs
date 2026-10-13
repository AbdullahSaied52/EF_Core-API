using Application.Intrefaces;
using Domain.DTOs;
using EF_API.DBcontext;
using EF_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.StudentRepositry
{
    public class studentRepository:IStudentRepositry
    {
        private readonly TrainingCenterDbContext _context;

        public studentRepository(TrainingCenterDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Student>> ListAllAsync()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList< Student >> filterStudentsAsync(string? first_name, string? status, bool sorted_be_name = false)
        {
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(first_name))
                query = query.Where(s => s.FirstName == first_name);
            if (!string.IsNullOrEmpty(status))
                query = query.Where(s => s.Status == status);
            if (sorted_be_name)
                query = query.OrderBy(s => s.FirstName);
            var students = await query.ToListAsync();
            return students;
        }

        public async Task<IReadOnlyList<AverageGradeDTO>> AverageGradeForEachCourse()
        {
            var grade = await _context.Enrollments.GroupBy(e => e.Course.Title).Select(g => new AverageGradeDTO
            {
                course_title = g.Key,
                avg_grade =Convert.ToDecimal( g.Average(e => e.FinalGrade))
            }).ToListAsync();

            return grade;
        }

        public async Task<IReadOnlyList< StudentName_DateDTO>> ListOrderOfStudentsByDate()
        {
            var students = await _context.Enrollments
                    .OrderBy(e => e.EnrollmentDate).Select(a => new StudentName_DateDTO
                    {
                        name = a.Student.FirstName + " " + a.Student.LastName,
                        date = a.EnrollmentDate
                    }).ToListAsync();
            return students;            
        }

        public async Task<IReadOnlyList<Student>> ListGraduated()
        {
            var graduated_query =await _context.Students.AsNoTracking().Where(s => s.Status == "Graduated").ToListAsync();
            //if (graduated_query.Count == 0) return NotFound("no one is graduated");

            return graduated_query;
        }

        public async Task<IReadOnlyList<StatusCountDTO>> NumberOfEachStatus()
        {
            var num = await _context.Students.GroupBy(s => s.Status)
    .Select(g => new StatusCountDTO
    {
        status = g.Key,
        count = g.Count()
    }).ToListAsync();
            return num;

        }

        public async Task<bool> Update(int student_id, StudentStatus status)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == student_id);
            
            if (student == null) return false;

            student.Status = Convert.ToString(status);
            _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> Delete(int student_id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == student_id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
