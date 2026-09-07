using System;
using System.Collections.Generic;
using System.Linq;
using HealthClinicDemo.Api.Models;

namespace HealthClinicDemo.Api.Data
{
    public static class SeedData
    {
        private static readonly Random Randomize = new Random();
        private const int AppointmentMonths = 6;

        public static void Initialize(MyHealthContext context)
        {
            if (context.Tenants.Any())
                return; // already seeded

            var tenantId = CreateDefaultTenant(context);
            CreateDoctors(context, tenantId);
            CreateSummaryInfo(context, tenantId);
            CreatePatients(context, tenantId);
            CreateMedicines(context, tenantId);
            CreateClinicAppointments(context, tenantId);
            CreateHomeAppointments(context, tenantId);
            CreateTips(context, tenantId);
        }

        static int CreateDefaultTenant(MyHealthContext context)
        {
            var tenant = new Tenant
            {
                Name = "HealthClinic.biz",
                Address = "Madison Ave 10037",
                City = "New York",
                WaitTimeAvg = Randomize.Next(1, 10),
                AssociatedUsername = "demo",
                Creator = "demo-admin"
            };

            context.Tenants.Add(tenant);
            context.SaveChanges();

            return tenant.TenantId;
        }

        static void CreateDoctors(MyHealthContext context, int tenantId)
        {
            var names = new[] { "Amanda Silver", "Casey Snider", "Clay McKnight", "Jasper Strader", "Eldon Caraway", "Irving Ingraham", "Denis Slattery", "Peter Ingraham" };
            var specialities = new[] { Specialities.Cardiologist, Specialities.Cardiologist, Specialities.Neurosurgeon, Specialities.Ophthalmologist, Specialities.Orthopedist, Specialities.Orthopedist, Specialities.Ophthalmologist, Specialities.Neurosurgeon };

            var doctors = new List<Doctor>();
            for (int i = 0; i < names.Length; i++)
            {
                doctors.Add(new Doctor
                {
                    Name = names[i],
                    Address = "Madison Ave 10037, New York, NY 10037",
                    Email = names[i].ToLower().Replace(" ", "") + "@healthclinic.biz",
                    Deleted = false,
                    TenantId = tenantId,
                    Speciality = specialities[i],
                    Synchronized = true,
                    PatientCount = Randomize.Next(50, 100),
                    CurrentRoomNumber = Randomize.Next(3, 15),
                    Description = "Monitoring and providing general care to patients on hospital wards and in outpatient clinics.",
                    Phone = "555-135-2245",
                    Mobile = "1-(546)-345-5678"
                });
            }

            context.Doctors.AddRange(doctors);
            context.SaveChanges();
        }

        static void CreatePatients(MyHealthContext context, int tenantId)
        {
            var names = new[] { "Kavin Gallo", "Scott Hanselman", "Cesar de la Torre", "Scott Guthrie", "David Carmona", "David Salgado", "Dmitry Lyalin", "Erika Ehrli Cabral", "Mitra Azizirad" };

            var patients = new List<Patient>();
            foreach (var name in names)
            {
                patients.Add(new Patient
                {
                    Name = name,
                    Address = "Madison Ave 10037, New York, NY 10037",
                    Email = name.ToLower().Replace(" ", ".") + "@outlook.com",
                    Deleted = false,
                    BloodType = "A+",
                    Gender = Gender.Male,
                    Height = 5.9,
                    Weight = 165,
                    ClinicId = "DFG-" + Randomize.Next(100000, 999999) + "-" + Randomize.Next(10000, 99999),
                    TenantId = tenantId,
                    Age = Randomize.Next(35, 45),
                    DateOfBirth = DateTime.UtcNow.AddYears(-Randomize.Next(30, 45)),
                    Phone = "555-" + Randomize.Next(100, 999) + "-" + Randomize.Next(1000, 9999)
                });
            }

            context.Patients.AddRange(patients);
            context.SaveChanges();
        }

        static void CreateMedicines(MyHealthContext context, int tenantId)
        {
            var data = new[]
            {
                new { Name = "Tylenol", Dose = 100.0, Unit = InternationalUnit.Milligrams, TimeOfDay = TimeOfDay.Dinner },
                new { Name = "Tamiflu", Dose = 100.0, Unit = InternationalUnit.Milligrams, TimeOfDay = TimeOfDay.Breakfast },
                new { Name = "Advil", Dose = 0.5, Unit = InternationalUnit.Milliliters, TimeOfDay = TimeOfDay.Lunch },
                new { Name = "Cafergot", Dose = 100.0, Unit = InternationalUnit.Milligrams, TimeOfDay = TimeOfDay.Breakfast },
            };

            var medicines = new List<Medicine>();
            var patients = context.Patients.Where(p => p.TenantId == tenantId).Select(p => p.PatientId).ToList();

            var globalIdx = 0;
            foreach (int patientId in patients)
            {
                foreach (var _ in Enumerable.Range(0, 4))
                {
                    var d = data[globalIdx];
                    medicines.Add(new Medicine
                    {
                        Name = d.Name,
                        Dose = d.Dose,
                        DoseUnit = d.Unit,
                        PatientId = patientId,
                        TimeOfDay = d.TimeOfDay,
                        TenantId = tenantId
                    });
                    globalIdx = (globalIdx + 1) % data.Length;
                }
            }

            context.Medicines.AddRange(medicines);
            context.SaveChanges();
        }

        static void CreateTips(MyHealthContext context, int tenantId)
        {
            context.Tips.Add(new Tip
            {
                Title = "Daily Health Tip",
                Content = "Drinking two glasses of water in the morning helps activate internal organs.",
                Date = DateTime.UtcNow,
                TenantId = tenantId
            });
            context.SaveChanges();
        }

        static void CreateClinicAppointments(MyHealthContext context, int tenantId)
        {
            var appointments = new List<ClinicAppointment>();
            var patients = context.Patients.Where(p => p.TenantId == tenantId).Select(p => p.PatientId).ToList();
            var doctors = context.Doctors.Where(d => d.TenantId == tenantId).ToList();

            foreach (int patientId in patients)
            {
                for (int i = 1; i <= AppointmentMonths; i++)
                {
                    var doctor = doctors[Randomize.Next(0, doctors.Count - 1)];
                    appointments.Add(new ClinicAppointment
                    {
                        PatientId = patientId,
                        DoctorId = doctor.DoctorId,
                        DateTime = GetAppointmentDate(i),
                        Speciality = doctor.Speciality,
                        RoomNumber = Randomize.Next(3, 15),
                        Description = "Follow up in order to determine the effectiveness of treatment received",
                        TenantId = tenantId,
                        IsUrgent = i % 2 == 0
                    });
                }
            }

            context.ClinicAppointments.AddRange(appointments);
            context.SaveChanges();
        }

        static void CreateHomeAppointments(MyHealthContext context, int tenantId)
        {
            var visits = new List<HomeAppointment>();
            var patients = context.Patients.Where(p => p.TenantId == tenantId).OrderBy(p => p.PatientId).Select(p => p.PatientId).Take(4).ToList();
            var doctors = context.Doctors.Where(d => d.TenantId == tenantId).Select(d => d.DoctorId).ToList();

            foreach (int doctorId in doctors)
            {
                for (int i = 1; i <= AppointmentMonths; i++)
                {
                    for (int p = 0; p < patients.Count; p++)
                    {
                        visits.Add(new HomeAppointment
                        {
                            PatientId = patients[p],
                            DoctorId = doctorId,
                            DateTime = GetAppointmentDate(i),
                            Latitude = 40.721847,
                            Longitude = -74.007326,
                            Address = "48 Wall St, New York, NY 10037",
                            Visited = p % 2 == 0,
                            IsUrgent = p % 3 == 0,
                            Description = "Follow up in order to determine the effectiveness of treatment received",
                            TenantId = tenantId
                        });
                    }
                }
            }

            context.HomeAppointments.AddRange(visits);
            context.SaveChanges();
        }

        static void CreateSummaryInfo(MyHealthContext context, int tenantId)
        {
            context.ClinicSummaries.Add(new ClinicSummary
            {
                AnualProfit = Randomize.Next(50000, 60000),
                AnualProfitVariation = Randomize.Next(1, 5),
                MonthProfit = Randomize.Next(4000, 6000),
                MonthProfitVariation = Randomize.Next(1, 5),
                NewPatients = Randomize.Next(100, 500),
                NewPatientsVariation = Randomize.Next(1, 10),
                Date = DateTime.UtcNow,
                TenantId = tenantId
            });

            var patientsSummaries = new List<PatientsSummary>();
            var expensesSummaries = new List<ExpensesSummary>();
            for (int i = 0; i < 24; i++)
            {
                var month = DateTime.UtcNow.AddMonths(-i);
                patientsSummaries.Add(new PatientsSummary { Year = month.Year, Month = month.Month, PatientsCount = Randomize.Next(500, 600), TenantId = tenantId });
                expensesSummaries.Add(new ExpensesSummary { Year = month.Year, Month = month.Month, Incomes = Randomize.Next(120000, 150000), Expenses = Randomize.Next(70000, 80000), TenantId = tenantId });
            }

            context.PatientsSummaries.AddRange(patientsSummaries);
            context.ExpensesSummaries.AddRange(expensesSummaries);
            context.SaveChanges();
        }

        private static DateTime GetAppointmentDate(int month = 0)
        {
            int startOfWorkingDay = 7;
            int endOfWorkingDay = 17;
            int slotMinutes = 15;
            int slotsPerHour = 4;

            var minutes = Randomize.Next(0, slotsPerHour - 1);
            var hour = Randomize.Next(startOfWorkingDay, endOfWorkingDay);
            var defaultDate = DateTime.UtcNow.AddMonths(month).AddDays(Randomize.Next(2, 20));

            return new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, slotMinutes * minutes, 0, DateTimeKind.Utc);
        }
    }
}
