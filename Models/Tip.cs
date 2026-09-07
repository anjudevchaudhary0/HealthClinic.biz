using System;

namespace HealthClinicDemo.Api.Models
{
    public class Tip
    {
        public int TipId { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
