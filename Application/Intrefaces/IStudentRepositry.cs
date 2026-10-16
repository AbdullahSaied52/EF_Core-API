using Domain.DTOs;
using EF_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intrefaces
{
    public interface IStudentRepositry
    {
        Task<IReadOnlyList<Student>> ListAllAsync();

        Task<IReadOnlyList<Student>> filterStudentsAsync(string? first_name, string? status, bool sorted_be_name = false);

        Task<IReadOnlyList<AverageGradeDTO>> AverageGradeForEachCourse();

        Task<IReadOnlyList< StudentName_DateDTO>> ListOrderOfStudentsByDate();

        Task<IReadOnlyList<Student>> ListGraduated();

        Task<IReadOnlyList<StatusCountDTO>> NumberOfEachStatus();

        Task<bool> Update(int student_id, StudentStatus status);

        Task<bool> Delete(int student_id);

        Task<StudentInfoForEnrollmentDTO> EnrollmentsForAStudent(int student_id);

        Task<bool> UpdateEnrollmentStatus(int student_id, int grade, EnrollmentStatus status, string course_code);

        Task<IReadOnlyList<StudentName_DateDTO>> GetStudentsforSpecificCourse(int course_id);

        Task<StudentInfoForCourseDTO> GetCompletedCoursesForStudent(int student_id);
        Task<InstructorReportDTO> InstructorReport(int instructor_id);

        void savechanges();
    }
}
