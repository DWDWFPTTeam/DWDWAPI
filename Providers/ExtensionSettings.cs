using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DWDW_API.Providers
{
    public class ExtensionSettings
    {
        public IConfiguration Configuration { get; private set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public AppSettings AppSettings
        {
            get
            {
                var appSettingsSection = this.Configuration.GetSection("AppSettings");
                return appSettingsSection.Get<AppSettings>();
            }
        }

        public ExtensionSettings(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }
    }
}
