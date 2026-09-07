using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace HealthClinicDemo.Api.Data
{
    public static class HttpRequestExtensions
    {
        public static int GetTenant(this HttpRequest request)
        {
            int tenantId = 0;
            StringValues headerValues;
            if (request.Headers.TryGetValue("TenantId", out headerValues))
            {
                int.TryParse(headerValues.First(), out tenantId);
            }

            return tenantId;
        }
    }
}
