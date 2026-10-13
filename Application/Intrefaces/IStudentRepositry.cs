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
    }
}
