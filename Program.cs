using BTL_SA.Infrastructure;
using BTL_SA.Modules.StaffMana.Infrastructure;
using BTL_SA.Modules.StaffMana.Application;
using BTL_SA.Modules.StaffMana.Facade;

using BTL_SA.Modules.PatientMana.Infrastructure;
using BTL_SA.Modules.PatientMana.Application;
using BTL_SA.Modules.PatientMana.Facade;

using Microsoft.OpenApi.Models;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Application.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<IStaffRepository, StaffRepositoryDb>();
builder.Services.AddSingleton<ICredentialRepository, CredentialRepositoryDb>();

builder.Services.AddSingleton<IQuerySevice, QueryService>();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IAssignmentService, AssignmentService>();
builder.Services.AddSingleton<ICredentialService, CredentialService>();

builder.Services.AddSingleton<FacadeStaffInformationManagement>();

// patient contianer
builder.Services.AddSingleton<IPatientRepository, PatientRepository>();
builder.Services.AddSingleton<IPatientVisitRepository, PatientVisitRepository>();
builder.Services.AddSingleton<IMedicalHistoryRepository, MedicalHistoryRepository>();

builder.Services.AddSingleton<IPatientCreator, PatientCreator>();
builder.Services.AddSingleton<IPatientRetriever, PatientRetriever>();
builder.Services.AddSingleton<IPatientUpdater, PatientUpdater>();
builder.Services.AddSingleton<IPatientDeleter, PatientDeleter>();

builder.Services.AddSingleton<IPatientVisitCreator, PatientVisitCreator>();
builder.Services.AddSingleton<IPatientVisitRetriever, PatientVisitRetriever>();
builder.Services.AddSingleton<IPatientVisitUpdater, PatientVisitUpdater>();
builder.Services.AddSingleton<IPatientVisitDeleter, PatientVisitDeleter>();

builder.Services.AddSingleton<IMedicalHistoryCreator, MedicalHistoryCreator>();
builder.Services.AddSingleton<IMedicalHistoryRetriever, MedicalHistoryRetriever>();
builder.Services.AddSingleton<IMedicalHistoryUpdater, MedicalHistoryUpdater>();
builder.Services.AddSingleton<IMedicalHistoryDeleter, MedicalHistoryDeleter>();



// builder.Services.AddSingleton<IPatientService, PatientService>();

// builder.Services.AddSingleton<IPatientVisitService, PatientVisitService>();

builder.Services.AddSingleton<FacadePatientManagement>();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BTL-SA API",
        Version = "v1",
        Description = "API documentation for the BTL-SA application"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // Enable Swagger in development mode
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BTL-SA API v1");
        c.RoutePrefix = "swagger"; // Swagger UI will be available at /swagger
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
