using Application.Intrefaces;
using EF_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StudentService:IStudentRepositry
    {
        private readonly IStudentRepositry _studenRepository;

        public StudentService(IStudentRepositry studentRepositry)
        {
            _studenRepository = studentRepositry;
        }

        public async Task<IReadOnlyList<Student>> ListAllAsync()
        {
            return await _studenRepository.ListAllAsync();
        }

        public async Task<IReadOnlyList<Student>> filterStudentsAsync(string? first_name, string? status, bool sorted_be_name = false)
        {
            return await _studenRepository.filterStudentsAsync(first_name, status, sorted_be_name);
        }

    }
}
