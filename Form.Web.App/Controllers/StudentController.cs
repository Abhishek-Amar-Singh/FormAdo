using System.Net.Http;
using System.Security.Policy;
using System.Text.Json;
using Form.Web.Api.Models.Students;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Space.Models;

namespace Form.Web.App.Controllers
{
    public class StudentController : Controller
    {
        private readonly string baseUrl = "https://localhost:7034/api/";

        // GET: StudentController
        [HttpGet]
        public async Task<ActionResult> GetAllStudents()
        {
            using HttpClient http = new HttpClient()
            {
                BaseAddress = new Uri($"{baseUrl}")
            };
            var url = string.Format("Students/GetAllStudents");
            var response = await http.GetAsync(url);
            Response<IEnumerable<Student>> storageStudentsRes = new()
            {
                Data = Enumerable.Empty<Student>()
            };

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                storageStudentsRes = JsonSerializer.Deserialize<Response<IEnumerable<Student>>>(stringResponse,
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    })!;
            }
            
            return View(storageStudentsRes.Data);
        }

        // GET: StudentController/Details/5

        [HttpGet]
        public async Task<ActionResult> GetStudent(int id)
        {
            using HttpClient http = new HttpClient()
            {
                BaseAddress = new Uri($"{baseUrl}")
            };
            var url = string.Format($"Students/GetStudentAsync?id={id}");
            var response = await http.GetAsync(url);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var storageStudentRes = JsonSerializer.Deserialize<Response<Student>>(stringResponse,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })!;

            return (storageStudentRes.StatusCode == StatusCodes.Status200OK) ? View(storageStudentRes.Data) : RedirectToAction("GetAllStudents");
        }

        // GET: StudentController/Create
        [HttpGet]
        public ActionResult CreateStudent()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        public async Task<ActionResult> CreateStudent(AddStudentDto dto)
        {
            using HttpClient http = new HttpClient()
            {
                BaseAddress = new Uri($"{baseUrl}")
            };
            var url = string.Format("Students/AddStudentAsync");
            HttpResponseMessage response = await http.PostAsJsonAsync(url, dto);

            return (response.IsSuccessStatusCode) ? RedirectToAction("GetAllStudents") : View();
        }

        // GET: StudentController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            using HttpClient http = new HttpClient()
            {
                BaseAddress = new Uri($"{baseUrl}")
            };
            var url = string.Format($"Students/GetStudentAsync?id={id}");
            var response = await http.GetAsync(url);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var storageStudentRes = JsonSerializer.Deserialize<Response<Student>>(stringResponse,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })!;

            return (storageStudentRes.StatusCode == StatusCodes.Status200OK) ? View(storageStudentRes.Data) :  RedirectToAction("GetAllStudents");
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Student dto)
        {
            using HttpClient http = new HttpClient()
            {
                BaseAddress = new Uri($"{baseUrl}")
            };
            var url = string.Format("Students/UpdateStudentAsync");
            HttpResponseMessage response = await http.PutAsJsonAsync(url, dto);

            return (response.IsSuccessStatusCode) ? RedirectToAction("GetAllStudents") : RedirectToAction("Edit");

        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
