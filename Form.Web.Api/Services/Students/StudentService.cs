using Form.Web.Api.Models.Students;
using Microsoft.Data.SqlClient;
using Shared.Space.Models;

namespace Form.Web.Api.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly string connStr = "Server=localhost;Database=FormDb;TrustServerCertificate=True;Trusted_Connection=True";

        public IEnumerable<Student> GetAllStudents()
        {
            string sqlQuery = "SELECT * FROM StudentTbl;";
            using (SqlConnection connection = new(connStr))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Student()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = Convert.ToString(reader["Name"]),
                                Class = Convert.ToString(reader["Class"]),
                                Gender = Convert.ToChar(reader["Gender"]),
                                DOB = Convert.ToString(reader["DOB"]),
                            };
                        }
                    }
                }
            }
        }

        public async Task<Response<int>> AddStudentAsyc(AddStudentDto dto)
        {
            string insertSql = @"INSERT INTO StudentTbl (Name, Class, Gender, DOB)
                    VALUES (@Name, @Class, @Gender, @DOB);";

            using (SqlConnection conn = new(connStr))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new SqlCommand(insertSql, conn))
                {
                    command.Parameters.AddWithValue("@Name", dto.Name);
                    command.Parameters.AddWithValue("@Class", dto.Class);
                    command.Parameters.AddWithValue("@Gender", dto.Gender);
                    command.Parameters.AddWithValue("@DOB", dto.DOB);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0 ? new()
                    {
                        Data = rowsAffected
                    } :
                    new()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Failed",
                        Data = rowsAffected,
                        Errors = new List<string>() { "Insertion failed" }
                    };
                }
            }
        }

        public async Task<Response<Student>> GetStudentAsync(int id)
        {
            List<Student> students = new List<Student>();
            string sqlQuery = "SELECT * FROM StudentTbl WHERE Id = @Id;";
            using (SqlConnection connection = new(connStr))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            students.Add(new()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = Convert.ToString(reader["Name"]),
                                Class = Convert.ToString(reader["Class"]),
                                Gender = Convert.ToChar(reader["Gender"]),
                                DOB = Convert.ToString(reader["DOB"]),
                            });
                        }
                    }
                }
            }

            return students.Any() ? new()
            {
                Data = students.FirstOrDefault()
            } :
            new()
            {
                StatusCode = 400,
                Message = "Failed",
                Errors = new List<string> { $"Couldn't find student with id={id}"}
            };
        }

        public async Task<Response<int>> UpdateStudentAsync(Student dto)
        {
            string insertSql = @"UPDATE StudentTbl 
                                SET Name=@Name, Class=@Class, Gender=@Gender, DOB=@DOB
                                WHERE Id=@Id;";

            using (SqlConnection conn = new(connStr))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new(insertSql, conn))
                {
                    command.Parameters.AddWithValue("@Id", dto.Id);
                    command.Parameters.AddWithValue("@Name", dto.Name);
                    command.Parameters.AddWithValue("@Class", dto.Class);
                    command.Parameters.AddWithValue("@Gender", dto.Gender);
                    command.Parameters.AddWithValue("@DOB", dto.DOB);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0 ? new()
                    {
                        Data = rowsAffected
                    } :
                    new()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Failed",
                        Data = rowsAffected,
                        Errors = new List<string>() { "Update failed" }
                    };
                }
            }
        }

        public async Task<Response<int>> DeleteStudentAsync(int id)
        {
            string delSql = @"DELETE FROM StudentTbl WHERE Id=@Id;";

            using (SqlConnection conn = new(connStr))
            {
                await conn.OpenAsync();
                using (SqlCommand command = new(delSql, conn))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0 ? new()
                    {
                        Data = rowsAffected
                    } :
                    new()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Failed",
                        Data = rowsAffected,
                        Errors = new List<string>() { "Delete failed" }
                    };
                }
            }
        }
    }
}
