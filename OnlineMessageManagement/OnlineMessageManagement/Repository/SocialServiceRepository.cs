using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OnlineMessageManagement.Models;
using Microsoft.Extensions.Configuration;

namespace OnlineMessageManagement.Data
{
    public class SocialServiceRepository
    {
        private readonly string _connectionString;

        public SocialServiceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SocialService> GetAll()
        {
            List<SocialService> socialServices = new List<SocialService>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllSocialServices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SocialService socialService = new SocialService
                            {
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                ServiceName = reader.GetString(reader.GetOrdinal("ServiceName"))
                            };

                            socialServices.Add(socialService);
                        }
                    }
                }
            }

            return socialServices;
        }
    }
}
