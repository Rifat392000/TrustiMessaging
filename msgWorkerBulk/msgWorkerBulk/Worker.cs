using System.Data.SqlClient;
using RestSharp;
namespace msgWorkerBulk
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string _connectionString = "Server=.;Database=OnlineMessageManagement;Trusted_Connection=true; TrustServerCertificate=true";


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }


        private async Task<int> GetServiceStatusAsync(int serviceId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using SqlCommand command = new SqlCommand("chkserviceisrunning", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ServiceId", serviceId);

                return (int)await command.ExecuteScalarAsync();
            }
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(stoppingToken);
                    using SqlCommand command = new SqlCommand("MsgSendBulk", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using SqlDataReader reader = await command.ExecuteReaderAsync(stoppingToken);
                    while (reader.Read())
                    {
                        int logSingleMessage = (int)reader["logSingleMessage"];
                        int serviceId = (int)reader["ServiceId"];
                        string? phoneNumber = reader["PhoneNumber"].ToString();
                        string? customerMessage = reader["CustomerMessage"].ToString();

                        // Call the stored procedure to get ServiceStatus
                        int serviceStatus = await GetServiceStatusAsync(serviceId);

                        if (serviceStatus == 1)
                        {
                            await SendMessageAsync(logSingleMessage, serviceId, phoneNumber, customerMessage);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Delay for 10 seconds

            }
        }





        private async Task UpdateMessageStatusAsync(int logSingleMessage, int messageStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using SqlCommand updateCommand = new SqlCommand("FinalStatusMsgBulk", connection);
                updateCommand.CommandType = System.Data.CommandType.StoredProcedure;

                updateCommand.Parameters.AddWithValue("@logSingleMessage", logSingleMessage);
                updateCommand.Parameters.AddWithValue("@messageStatus", messageStatus);

                await updateCommand.ExecuteNonQueryAsync();
            }
        }


        private async Task SendMessageAsync(int logSingleMessage, int serviceId, string phoneNumber, string message)
        {
            // Adding +88 prefix to the phone number
            phoneNumber = "+88" + phoneNumber;

            var url = "https://api.ultramsg.com/instance59332/messages/chat";
            var client = new RestClient(url);

            var request = new RestRequest(url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", "mjh55k79yism9i1t");
            request.AddParameter("to", phoneNumber);
            request.AddParameter("body", message);

            RestResponse response = await client.ExecuteAsync(request);
            var output = response.Content;

            _logger.LogInformation("Message sent: {output}", output);

            if (output.Contains("\"sent\":\"true\"") && output.Contains("\"message\":\"ok\""))
            {
                // Parse the sent message ID from the output JSON
                dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(output);
                int sentMessageId = responseJson.id;

                // Send GET request to the URL
                var getUrl = $"https://api.ultramsg.com/instance59332/messages?token=mjh55k79yism9i1t&page=1&limit=100&status=invalid";
                var getClient = new RestClient(getUrl);

                RestResponse getResponse = await getClient.ExecuteAsync(new RestRequest());
                var getOutput = getResponse.Content;

                // Parse the JSON response
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(getOutput);
                foreach (var msg in jsonResponse.messages)
                {
                    int messageId = (int)msg.id;
                    if (messageId == sentMessageId)
                    {
                        await UpdateMessageStatusAsync(logSingleMessage, -1);
                        break;
                    }
                }
                await UpdateMessageStatusAsync(logSingleMessage, 2);

            }

        }

    }
}
