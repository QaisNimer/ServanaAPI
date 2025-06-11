using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServanaAPP;
using ServanaAPP.Helpers.JWT;
using ServanaAPP.Helpers.OtpUserSelection;
using ServanaAPP.Helpers.SendingEmail;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;
using ServanaAPP.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ServanaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddScoped<MailingHelper>();
builder.Services.AddScoped<OtpBasedOnUserRole>();
builder.Services.Configure<SendGridSettings>(
    builder.Configuration.GetSection("SendGrid"));
builder.Services.AddScoped<IAuthentication, AuthServices>();
builder.Services.AddScoped<GenerateJwtTokenHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
