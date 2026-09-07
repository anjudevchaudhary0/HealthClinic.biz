using Microsoft.EntityFrameworkCore;
using HealthClinicDemo.Api.Models;

namespace HealthClinicDemo.Api.Data
{
    public class MyHealthContext : DbContext
    {
        public MyHealthContext(DbContextOptions<MyHealthContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<ClinicAppointment> ClinicAppointments { get; set; }
        public DbSet<HomeAppointment> HomeAppointments { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<ExpensesSummary> ExpensesSummaries { get; set; }
        public DbSet<PatientsSummary> PatientsSummaries { get; set; }
        public DbSet<ClinicSummary> ClinicSummaries { get; set; }
        public DbSet<Tip> Tips { get; set; }
    }
}
