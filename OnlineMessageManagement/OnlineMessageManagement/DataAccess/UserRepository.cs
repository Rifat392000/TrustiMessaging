using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using OnlineMessageManagement.Models;

namespace OnlineMessageManagement.DataAccess
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Logpannel LoginIdAndPassword(decimal loginId, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("userLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@login_id", loginId);
                    command.Parameters.AddWithValue("@password", password);
                  

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Logpannel user = new Logpannel
                            {
                                Name = reader["name"].ToString(),
                               
                                
                            };
                            return user;
                        }
                    }
                }
            }

            return null;
        }




        public Logpannel LogInfo(decimal loginId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("userLoginfo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@login_id", loginId);
                 

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Logpannel userinfo = new Logpannel
                            {
                                Name = reader["name"].ToString(),
                                LoginId = (decimal)reader["login_id"],
                                UserId = (int)reader["userId"]


                            };
                            return userinfo;
                        }
                    }
                }
            }

            return null;
        }










    }
}
