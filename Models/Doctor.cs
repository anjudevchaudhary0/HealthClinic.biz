using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HealthClinicDemo.Api.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public byte[]? Picture { get; set; }
        public bool Deleted { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Specialities Speciality { get; set; }

        public int CurrentRoomNumber { get; set; }
        public int PatientCount { get; set; }
        public bool Synchronized { get; set; }

        public ICollection<ClinicAppointment> ClinicAppointments { get; set; }
        public ICollection<HomeAppointment> HomeAppointments { get; set; }
    }
}
