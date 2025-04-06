using BTL_SA.Infrastructure;
using BTL_SA.Modules.StaffMana.Infrastructure;
using BTL_SA.Modules.StaffMana.Application;
using BTL_SA.Modules.StaffMana.Facade;

using Microsoft.OpenApi.Models;

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
