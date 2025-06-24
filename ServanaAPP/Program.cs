using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServanaAPP;
using ServanaAPP.Helpers.Firebase;
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
builder.Services.AddScoped<IAllWorkersByCategoryId, AllWorkersByCategoryIdService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<SendNotificationHelper>();
builder.Services.AddScoped<IUpdateDeviceToken, UpdateDeviceTokenService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IWorkSession, WorkSessionService>();

var firebaseKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "firebase", "servana-ab0e5-firebase-adminsdk-fbsvc-debd198015.json");


FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(firebaseKeyPath)
});

var app = builder.Build();

// When You Want To Test Locally Comment This, If Publish Keep It
app.UseStaticFiles();


//app.UseSwagger();
//app.UseSwaggerUI(
//    c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Servana API V1");
//        c.RoutePrefix = string.Empty;
//    }
//    );
// Until Here To Test Publish

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BrainstormingFoodTek API V1");
    }  // Set Swagger endpoint
    );
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
