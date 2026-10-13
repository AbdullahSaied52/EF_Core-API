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



    }
}
