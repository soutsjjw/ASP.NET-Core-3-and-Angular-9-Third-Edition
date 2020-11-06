using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthCheck.Settings.StaticFiles;

namespace HealthCheck.Models.StaticFiles
{
    public class Headers
    {
        public Headers(HeadersSettings config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            Cache_Control = config.Cache_Control;
            Pragma = config.Pragma;
            Expires = config.Expires;
        }

        public string Cache_Control { get; protected set; }
        public string Pragma { get; protected set; }
        public string Expires { get; protected set; }
    }
}
