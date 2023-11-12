using System.Data;
using System.Data.SqlClient;
using OnlineMessageManagement.Models;

namespace OnlineMessageManagement.Data
{
    public class AppUserDataAccess
    {
        private readonly string _connectionString;

        public AppUserDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SocialService> GetAllSocialServices()
        {
            List<SocialService> socialServices = new List<SocialService>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetAllSocialServices", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SocialService socialService = new SocialService
                            {
                                ServiceId = (int)reader["ServiceId"],
                                ServiceName = (string)reader["ServiceName"],
                                ServiceStatus = (int)reader["ServiceStatus"]
                            };

                            socialServices.Add(socialService);
                        }
                    }
                }
            }

            return socialServices;
        }


        public List<AppUserModel> GetAppUsers(int serviceId)

        {
            List<AppUserModel> appUsers = new List<AppUserModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("AppUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@serviceId", serviceId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AppUserModel appUser = new AppUserModel
                            {
                                Cid = (int)reader["Cid"],
                                Cname = (string)reader["Cname"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                ServiceName = (string)reader["ServiceName"],
                                ServiceId = (int)reader["ServiceId"]
                            };

                            appUsers.Add(appUser);
                        }
                    }
                }
            }

            return appUsers;
        }



        public void InsertSingleMessage(int cid, string cname, string phoneNumber, string serviceName, int serviceId, string customerMessage)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("InsertSingleMessage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Cid", cid);
                    command.Parameters.AddWithValue("@Cname", cname);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@ServiceName", serviceName);
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    command.Parameters.AddWithValue("@CustomerMessage", customerMessage);

                    command.ExecuteNonQuery();
                }
            }
        }


        public void InsertbulkMessage(int serviceId, string customerMessage)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("InsertbulkMessage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServiceId", serviceId);
                    command.Parameters.AddWithValue("@CustomerMessage", customerMessage);

                    command.ExecuteNonQuery();
                }
            }
        }




        public List<SingleMessage> GetSingleMessages()
        {
            List<SingleMessage> singleMessages = new List<SingleMessage>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetSingleMessage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SingleMessage singleMessage = new SingleMessage
                            {
                                Cid = (int)reader["Cid"],
                                Cname = (string)reader["Cname"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                ServiceName = (string)reader["ServiceName"],
                                CustomerMessage = (string)reader["CustomerMessage"],
                                messageStatus = (int)reader["messageStatus"],
                                MsgCreated = Convert.ToDateTime(reader["MsgCreated"]),
                            MsgSend = Convert.ToDateTime(reader["MsgSend"])
                            };

                            singleMessages.Add(singleMessage);
                        }
                    }
                }
            }

            return singleMessages;
        }

        


        public List<bulkMessage> GetbulkMessages()
        {
            List<bulkMessage> bulkMessages = new List<bulkMessage>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetbulkMessage", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bulkMessage bulkMessage = new bulkMessage
                            {
                                Cid = (int)reader["Cid"],
                                Cname = (string)reader["Cname"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                ServiceName = (string)reader["ServiceName"],
                                CustomerMessage = (string)reader["CustomerMessage"],
                                messageStatus = (int)reader["messageStatus"],
                                MsgCreated = Convert.ToDateTime(reader["MsgCreated"]),
                                MsgSend = Convert.ToDateTime(reader["MsgSend"])
                            };

                            bulkMessages.Add(bulkMessage);
                        }
                    }
                }
            }

            return bulkMessages;
        }



        public List<SingleMessage> SingleMessageUncheckeds()
        {
            List<SingleMessage> singleMessages = new List<SingleMessage>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SingleMessageUnchecked", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SingleMessage singleMessage = new SingleMessage
                            {
                                logSingleMessage = (int)reader["logSingleMessage"],
                                checkedmsg = (int)reader["checkedmsg"],
                                Cid = (int)reader["Cid"],
                                Cname = (string)reader["Cname"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                ServiceName = (string)reader["ServiceName"],
                                CustomerMessage = (string)reader["CustomerMessage"],
                                messageStatus = (int)reader["messageStatus"],
                                MsgCreated = Convert.ToDateTime(reader["MsgCreated"]),

                            };

                            singleMessages.Add(singleMessage);
                        }
                    }
                }
            }

            return singleMessages;
        }



        public List<bulkMessage> bulkMessageUncheckeds()
        {
            List<bulkMessage> bulkMessages = new List<bulkMessage>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("bulkMessageUnchecked", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bulkMessage bulkMessage = new bulkMessage
                            {
                                logSingleMessage = (int)reader["logSingleMessage"],
                                checkedmsg = (int)reader["checkedmsg"],
                                Cid = (int)reader["Cid"],
                                Cname = (string)reader["Cname"],
                                PhoneNumber = (string)reader["PhoneNumber"],
                                ServiceName = (string)reader["ServiceName"],
                                CustomerMessage = (string)reader["CustomerMessage"],
                                messageStatus = (int)reader["messageStatus"],
                                MsgCreated = Convert.ToDateTime(reader["MsgCreated"]),

                            };

                            bulkMessages.Add(bulkMessage);
                        }
                    }
                }
            }

            return bulkMessages;
        }


      

        public void SingleMessagecheckedStatus(int logSingleMessage,int checkedmsg,int messageStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SingleMessagechecked", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@logSingleMessage", logSingleMessage);
                    command.Parameters.AddWithValue("@checkedmsg", checkedmsg);  
                    command.Parameters.AddWithValue("@messageStatus", messageStatus); 

                    command.ExecuteNonQuery();
                }
            }
        }



        public void bulkMessagecheckedStatus(int logSingleMessage, int checkedmsg, int messageStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("bulkMessagechecked", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@logSingleMessage", logSingleMessage);
                    command.Parameters.AddWithValue("@checkedmsg", checkedmsg);
                    command.Parameters.AddWithValue("@messageStatus", messageStatus);

                    command.ExecuteNonQuery();
                }
            }
        }

        //

        public List<MessageInfoByService> BulkMessageInfoByService()
{
    List<MessageInfoByService> results = new List<MessageInfoByService>();

    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        connection.Open();

        using (SqlCommand command = new SqlCommand("bulkMsgInfoByService", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    MessageInfoByService result = new MessageInfoByService
                    {
                        ServiceName = (string)reader["ServiceName"],
                        Total = (int)reader["total"],
                        Process = (int)reader["process"],
                        Pending = (int)reader["pending"],
                        Success = (int)reader["success"],
                        Failed = (int)reader["failed"]
                    };

                    results.Add(result);
                }
            }
        }
    }

    return results;
}

        public List<MessageInfoByService> SendTimeForBulkMessages()
        {
            List<MessageInfoByService> results = new List<MessageInfoByService>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("messageSendTimeBulk", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MessageInfoByService result = new MessageInfoByService
                            {
                                DistinctDate = (string)reader["distinct_date"],
                                NumMessagesPerMinute = (int)reader["num_msg_per_min"]
                            };

                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }




        public List<MessageInfoByService> SingleMessageInfoByService()
        {
            List<MessageInfoByService> results = new List<MessageInfoByService>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("singleMsgInfoByService", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MessageInfoByService result = new MessageInfoByService
                            {
                                ServiceName = (string)reader["ServiceName"],
                                Total = (int)reader["total"],
                                Process = (int)reader["process"],
                                Pending = (int)reader["pending"],
                                Success = (int)reader["success"],
                                Failed = (int)reader["failed"]
                            };

                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }

        public List<MessageInfoByService> SendTimeForSingleMessages()
        {
            List<MessageInfoByService> results = new List<MessageInfoByService>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("messageSendTimeSingle", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MessageInfoByService result = new MessageInfoByService
                            {
                                DistinctDate = (string)reader["distinct_date"],
                                NumMessagesPerMinute = (int)reader["num_msg_per_min"]
                            };

                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }




    }


}
