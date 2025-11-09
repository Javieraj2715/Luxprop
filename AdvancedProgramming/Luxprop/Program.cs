using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Luxprop.Business.Services;
using Luxprop.Data.Models;
using Luxprop.Data.Repositories;
using Luxprop.Hubs;
using Luxprop.Services;
using Microsoft.EntityFrameworkCore;
// Alias explícito al hub correcto
using ChatHubType = Luxprop.Hubs.ChatHub;

var builder = WebApplication.CreateBuilder(args);

// 1) Servicios (TODO antes de Build)
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContextFactory<LuxpropContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Luxprop")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();
builder.Services.AddScoped<PasswordHelper>();
builder.Services.AddScoped<AuditoriaService>();
builder.Services.AddScoped<SecurityService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddHostedService<ReminderNotifier>();
builder.Services.AddSingleton<IEmailService, SmtpEmailService>();
builder.Services.AddHttpContextAccessor();




// 🔹 Repositorios base
builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepository>();
builder.Services.AddScoped<IHistorialExpedienteRepository, HistorialExpedienteRepository>();


builder.Services.AddScoped<IHistorialExpedienteService, HistorialExpedienteService>();
builder.Services.AddScoped<IExpedienteService, ExpedienteService>();

// SignalR
builder.Services.AddSignalR();

// (Opcional) Firebase credencial por variable de entorno
var credentialPath = @"C:\Users\pepon\Documents\GitHub\Luxprop\AdvancedProgramming\Luxprop\App_Data\firebase-config.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

// 2) Construcción de la app (después de registrar servicios)
var app = builder.Build();

// 3) Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// carpeta /wwwroot/uploads
var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseRouting();

// Endpoints
app.MapBlazorHub();
app.MapHub<ChatHubType>("/hubs/chat");   // <- Hub de SignalR
app.MapFallbackToPage("/_Host");

app.Run();
