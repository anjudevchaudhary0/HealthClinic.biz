using Microsoft.EntityFrameworkCore;
using HealthClinicDemo.Api.Data;
using HealthClinicDemo.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyHealthContext>(options =>
    options.UseInMemoryDatabase("HealthClinicDemo"));

builder.Services.AddScoped<PatientsRepository>();
builder.Services.AddScoped<DoctorsRepository>();
builder.Services.AddScoped<ClinicAppointmentsRepository>();
builder.Services.AddScoped<HomeAppointmentRepository>();
builder.Services.AddScoped<MedicinesRepository>();
builder.Services.AddScoped<ReportsRepository>();
builder.Services.AddScoped<TipsRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyHealthContext>();
    SeedData.Initialize(context);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
