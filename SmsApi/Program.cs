using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmsApi.Configuration;
using SmsApi.Data;
using SmsApi.Endpoints;
using SmsApi.Middleware;
using SmsApi.Repositories;
using SmsApi.Services;

var builder = WebApplication.CreateBuilder(args);

var smsOptions = builder.Configuration
    .GetSection("SmsService")
    .Get<SmsServiceOptions>();

if (string.IsNullOrWhiteSpace(smsOptions.BaseUrl))
{
    Console.WriteLine("Critical: SmsService:BaseUrl is not configured.");
    return;
}

builder.Services.Configure<SmsServiceOptions>(
    builder.Configuration.GetSection("SmsService"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("SmsDb"));

builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddHttpClient<IMessageService, MessageService>((sp, client) =>
{
    client.BaseAddress = new Uri(smsOptions.BaseUrl);
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapMessageEndpoints();
app.UseHttpsRedirection();

app.Run();
