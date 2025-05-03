namespace Form.Web.Api.Models.Students
{
    public class AddStudentDto
    {
        public string? Name { get; set; }
        public string? Class { get; set; }
        public char? Gender { get; set; }
        public string? DOB { get; set; }
    }

    public class UpdateStudentDto: AddStudentDto { }
}
