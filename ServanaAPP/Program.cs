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

// Read JWT values from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JWT");
string jwtKey = jwtSettings["Key"]; // or "Key"
string jwtIssuer = jwtSettings["Issuer"];
string jwtAudience = jwtSettings["Audience"];

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database
builder.Services.AddDbContext<ServanaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Custom services and DI
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
builder.Services.AddScoped<MailingHelper>();
builder.Services.AddScoped<OtpBasedOnUserRole>();
builder.Services.AddScoped<IAuthentication, AuthServices>();
builder.Services.AddScoped<GenerateJwtTokenHelper>();
builder.Services.AddScoped<IHomeScreenClient, ClientHomeScreenService>();
builder.Services.AddScoped<IProfile, ProfileService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
