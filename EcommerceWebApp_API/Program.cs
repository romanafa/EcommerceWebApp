using Azure.Storage.Blobs;
using EcommerceWebApp_API.Configurations;
using EcommerceWebApp_API.Data;
using EcommerceWebApp_API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Connection string for azure image storage
builder.Services.AddSingleton(x => new BlobServiceClient(
   builder.Configuration.GetConnectionString("StorageAccount")));
builder.Services.AddSingleton<IBlobService, BlobService>();

builder.Services.AddAutoMapper(typeof(MapperConfig));

// Identity with lockout settings (5 minutes lockout time, 5 maximum failed attempts, enabled for new users) --> subject to change
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // lockout time span
    options.Lockout.MaxFailedAccessAttempts = 5; // maximum failed login attempts
    options.Lockout.AllowedForNewUsers = true; // whether lockout is enabled for new users
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); 


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog configuration
builder.Host.UseSerilog((ctx, logConfig) =>
    logConfig.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

// CORS configuration
builder.Services.AddCors( options =>
{
    options.AddPolicy("AllowAll", 
        b => b.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});

var app = builder.Build();

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(C =>
    {
        C.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        C.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
 