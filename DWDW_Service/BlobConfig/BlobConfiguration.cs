using Microsoft.Extensions.Configuration;
using System;

namespace DWDW_Service.BlobConfig
{
    public static class BlobConfiguration
    {
        private static IConfiguration currentConfig;

        public static void SetConfig(IConfiguration configuration)
        {
            currentConfig = configuration;
        }
        public static string GetConfiguration(string configKey)
        {
            try
            {
                string connectionString = currentConfig.GetConnectionString(configKey);
                return connectionString;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return "";
        }
    }
}
