using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck
{
    public class ICMPHealthCheck : IHealthCheck
    {
        private string Host { get; set; }
        private int Timeout { get; set; }

        public ICMPHealthCheck(string host, int timeout)
        {
            Host = host;
            Timeout = timeout;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(Host);
                    switch (reply.Status)
                    {
                        case IPStatus.Success:
                            var msg = $"IMCP to {Host} took {reply.RoundtripTime} ms.";

                            return (reply.RoundtripTime > Timeout)
                                    ? HealthCheckResult.Degraded(msg)
                                    : HealthCheckResult.Healthy(msg);
                        default:
                            var err = $"IMCP to {Host} faild: {reply.Status}";
                            return HealthCheckResult.Unhealthy(err);
                    }
                }
            }
            catch (Exception e)
            {
                var err = $"IMCP to {0} failed: {e.Message}";
                return HealthCheckResult.Unhealthy(err);
            }
        }
    }
}
