using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OnlineMessageManagement.Models;

namespace OnlineMessageManagement.Data
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Customer> GetAvailableCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAvailableCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer();
                            customer.Cid = Convert.ToInt32(reader["Cid"]);
                            customer.Cname = reader["Cname"].ToString();
                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;
        }

        public Customer GetById(int id)
        {
            Customer customer = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetCustomerById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Cid", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new Customer();
                            customer.Cid = Convert.ToInt32(reader["Cid"]);
                            customer.Cname = reader["Cname"].ToString();
                            
                            // Set other properties
                        }
                    }
                }
            }

            return customer;
        }
    }
}
