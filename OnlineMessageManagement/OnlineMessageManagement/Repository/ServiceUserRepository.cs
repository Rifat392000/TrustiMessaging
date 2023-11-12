using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineMessageManagement.Models;

namespace OnlineMessageManagement.Data
{
    public class ServiceUserRepository
    {
        private readonly string _connectionString;

        public ServiceUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ServiceUser> GetAll()
        {
            List<ServiceUser> serviceUsers = new List<ServiceUser>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllServiceUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServiceUser serviceUser = new ServiceUser();
                            serviceUser.Cid = Convert.ToInt32(reader["Cid"]);
                            serviceUser.Cname = reader["Cname"].ToString();
                            serviceUser.PhoneNumber = reader["PhoneNumber"].ToString();
                            serviceUser.ServiceUse = reader["ServiceUse"].ToString();
                            serviceUser.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                            // Handle DBNull.Value for UpdatedAt
                            if (reader["UpdatedAt"] != DBNull.Value)
                            {
                                serviceUser.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                            }

                            serviceUsers.Add(serviceUser);
                        }
                    }
                }
            }

            return serviceUsers;
        }

        public ServiceUser GetById(int id)
        {
            ServiceUser serviceUser = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetServiceUserById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Cid", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            serviceUser = new ServiceUser();
                            serviceUser.Cid = Convert.ToInt32(reader["Cid"]);
                            serviceUser.Cname = reader["Cname"].ToString();
                            serviceUser.PhoneNumber = reader["PhoneNumber"].ToString();
                            serviceUser.ServiceUse = reader["ServiceUse"].ToString();
                            serviceUser.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

                            // Handle DBNull.Value for UpdatedAt
                            if (reader["UpdatedAt"] != DBNull.Value)
                            {
                                serviceUser.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                            }
                        }
                    }
                }
            }

            return serviceUser;
        }

        public void Create(ServiceUser serviceUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("CreateServiceUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerCid", serviceUser.CustomerCid);
                    command.Parameters.AddWithValue("@CustomerCname", serviceUser.Cname);
                    command.Parameters.AddWithValue("@PhoneNumber", serviceUser.PhoneNumber);
                    command.Parameters.AddWithValue("@ServiceUse", serviceUser.ServiceUse);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(ServiceUser serviceUser)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateServiceUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Cid", serviceUser.Cid);
                   
                    command.Parameters.AddWithValue("@PhoneNumber", serviceUser.PhoneNumber);
                    command.Parameters.AddWithValue("@ServiceUse", serviceUser.ServiceUse);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int cid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteServiceUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Cid", cid);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public int GetMaxCid()
        {
            int maxCid = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT ISNULL(MAX(Cid), 0) FROM ServiceUser", connection))
                {
                    connection.Open();
                    maxCid = (int)command.ExecuteScalar();
                }
            }

            return maxCid;
        }
    }
}
