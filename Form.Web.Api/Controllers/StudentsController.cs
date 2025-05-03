using Form.Web.Api.Models.Students;
using Form.Web.Api.Services.Students;
using Microsoft.AspNetCore.Mvc;
using Shared.Space.Models;

namespace Form.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService _studentService) =>
            this._studentService = _studentService;

        [HttpGet]
        [Route("GetAllStudents")]
        public ActionResult GetAllStudents()
        {
            var storageStudents = this._studentService.GetAllStudents();

            var response = new Response<IEnumerable<Student>>()
            {
                Data = storageStudents
            };

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Route("AddStudentAsync")]
        public async Task<ActionResult> AddStudentAsync([FromBody] AddStudentDto dto)
        {
            var response = await this._studentService.AddStudentAsyc(dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("GetStudentAsync")]
        public async Task<ActionResult> GetStudentAsync(int id)
        {
            var response = await this._studentService.GetStudentAsync(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [Route("UpdateStudentAsync")]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] Student dto)
        {
            var response = await this._studentService.UpdateStudentAsync(dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [Route("DeleteStudentAsync")]
        public async Task<ActionResult> DeleteStudentAsync(int id)
        {
            var response = await this._studentService.DeleteStudentAsync(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
