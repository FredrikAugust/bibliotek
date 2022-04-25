using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Web.Extensions;
using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Logging;
using Serilog;
using Serilog.Debugging;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
Log.Logger = Logger.Create(builder.Configuration["Serilog:Elastic:ConnectionString"]);

builder.Host.UseSerilog(Log.Logger);

SelfLog.Enable(Console.Error);

builder.Services.AddSingleton(Log.Logger);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtensions(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zanaris API v1");
        options.OAuthClientId(app.Configuration["AzureAd:ClientId"]);
        options.OAuthScopeSeparator(" ");
        options.OAuthUsePkce();
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}