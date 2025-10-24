using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Luxprop.Business.Services;
using Luxprop.Data.Models;
using Luxprop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<LuxpropContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Luxprop")));
builder.Services.AddScoped<Luxprop.Services.AuthService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();


// Add Entity Framework
builder.Services.AddDbContext<LuxpropContext>();

// Add custom services
builder.Services.AddScoped<PasswordHelper>();
builder.Services.AddScoped<AuditoriaService>();

var app = builder.Build();

//FirebaseApp.Create(new AppOptions()
//{
//Credential = GoogleCredential.FromFile("../"),
//});

var credentialPath = @"C:\Luxprop\AdvancedProgramming\Luxprop\App_Data\firebase-config.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
