using Application.Intrefaces;
using EF_API.Models;
using System;
using System.Collections.Generic;
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
    }
}
