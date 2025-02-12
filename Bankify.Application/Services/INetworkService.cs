using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Bankify.Application.Services
{
    public interface INetworkService
    {
        Task<bool> IsConnected();
    }

    public class NetworkService : INetworkService
    {
        private readonly IConfiguration _configuration;

        public NetworkService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> IsConnected()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("Default");

                if (string.IsNullOrWhiteSpace(connectionString))
                    return false;

                var builder = new SqlConnectionStringBuilder(connectionString);
                string serverName = builder.DataSource;

               
                // Check if it's a local instance
                if (IsLocalServer(serverName))
                {
                    return true;
                }

                using (Ping ping = new Ping())
                {
                    PingReply reply = await ping.SendPingAsync(serverName, 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool IsLocalServer(string serverName)
        {
            try
            {
                string machineName = Environment.MachineName;
                string[] localHosts = [ "(local)", "localhost","mssql" ];

                // Check against known local identifiers
                foreach (var item in localHosts)
                {
                     if(serverName.Contains(item, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;                     

                    };
                    
                }                

                // Check if the server name resolves to a loopback IP (127.x.x.x or ::1)
                IPAddress[] addresses = Dns.GetHostAddresses(serverName);
                return addresses.Any(ip => IPAddress.IsLoopback(ip));
            }
            catch
            {
                return false;
            }
        }
    }
}
