using EF_API.DBcontext;
using EF_API.Models;
using Infrastructure.StudentRepositry;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public async Task MainController_WhenStudentIDisGreaterThanZero_ShouldReturnTrue()
        {
            var context = new InMemoryDBContext();
            context.Students.Add(new Student {
                StudentId = 1,
                FirstName = "Ahmed",
                LastName = "Ali",
                Email = "ahmed@example.com",
                DateOfBirth = new DateOnly(2000, 1, 1),
                RegisteredAt = DateTime.UtcNow,
                Status = StudentStatus.Active.ToString()
            });
            await context.SaveChangesAsync();

            var sut =new studentRepository(context);

            var result =await sut.Update(1,StudentStatus.Graduated);

            Assert.True( result);
        }
    }
}