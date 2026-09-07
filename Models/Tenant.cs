namespace HealthClinicDemo.Api.Models
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int WaitTimeAvg { get; set; }
        public string Creator { get; set; }
        public string AssociatedUsername { get; set; }
    }
}
