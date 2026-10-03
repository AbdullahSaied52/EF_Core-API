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

    }
}
