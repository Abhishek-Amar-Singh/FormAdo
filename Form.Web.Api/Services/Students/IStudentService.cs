using Form.Web.Api.Models.Students;
using Shared.Space.Models;

namespace Form.Web.Api.Services.Students
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAllStudents();
        Task<Response<int>> AddStudentAsyc(AddStudentDto dto);
        Task<Response<Student>> GetStudentAsync(int id);
        Task<Response<int>> UpdateStudentAsync(Student dto);
        Task<Response<int>> DeleteStudentAsync(int id);
    }
}
