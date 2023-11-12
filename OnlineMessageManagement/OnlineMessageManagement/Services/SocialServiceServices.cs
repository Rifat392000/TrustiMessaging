using OnlineMessageManagement.Models;
using System.Data.SqlClient;


namespace OnlineMessageManagement.Services
{
    public class SocialServiceServices : ISocialServiceServices
    {
        public string Constr { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SocialServiceServices(IConfiguration configuration)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DefaultConnection");
        }

        public List<SocialService> GetSocialServiceRecord()
        {
            List<SocialService> socialServiceList = new List<SocialService>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("GetAllSocialServices", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SocialService ssr = new SocialService
                        {
                            ServiceId = Convert.ToInt32(rdr["ServiceId"]),
                            ServiceName = rdr["ServiceName"].ToString(),
                            ServiceStatus = Convert.ToInt32(rdr["ServiceStatus"])
                        };

                        socialServiceList.Add(ssr);
                    }
                }
                return socialServiceList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateServiceStatus(int serviceId, int status) // Change the type to int
        {
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("UpdateServiceStatus", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                    cmd.Parameters.AddWithValue("@ServiceStatus", status); // Use int directly
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTotalServiceUserCount()
        {
            int totalServiceUser = 0;
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("totalServiceUserCount", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    totalServiceUser = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return totalServiceUser;
        }

        public Dictionary<int, int> GetServiceUserCountByService()
        {
            Dictionary<int, int> serviceUserCounts = new Dictionary<int, int>();

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("differentServices", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        int value = Convert.ToInt32(rdr["value"]);
                        int totalcount = Convert.ToInt32(rdr["totalcount"]);

                        serviceUserCounts.Add(value, totalcount);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return serviceUserCounts;
        }

        public SocialService GetServiceById(int serviceId)
        {
            SocialService? socialService = null;

            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("GetServiceById", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        socialService = new SocialService
                        {
                            ServiceId = Convert.ToInt32(rdr["ServiceId"]),
                            ServiceName = rdr["ServiceName"].ToString()
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return socialService;
        }



        public void AddSocialService(string serviceName)
        {
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("AddSocialService", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceName", serviceName);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateService(int serviceId, string serviceName)
        {
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("UpdateService", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                    cmd.Parameters.AddWithValue("@ServiceName", serviceName);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteService(int serviceId)
        {
            try
            {
                using (con = new SqlConnection(Constr))
                {
                    con.Open();
                    var cmd = new SqlCommand("DeleteService", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceId", serviceId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public interface ISocialServiceServices
    {
        List<SocialService> GetSocialServiceRecord();
        int GetTotalServiceUserCount();
        Dictionary<int, int> GetServiceUserCountByService();
        void UpdateServiceStatus(int serviceId, int status);
        void UpdateService(int serviceId, string serviceName);
        void DeleteService(int serviceId);
        void AddSocialService(string serviceName);
        SocialService GetServiceById(int serviceId);
    }





}
