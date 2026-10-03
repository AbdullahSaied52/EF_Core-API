using Application.Intrefaces;
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
    }
}
