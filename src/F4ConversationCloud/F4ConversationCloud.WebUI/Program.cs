using F4ConversationCloud.Infrastructure;
using F4ConversationCloud.Infrastructure.Service;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "F4ConversationCloud API",
        Version = "v1"
    });
});

builder.Services.AddHttpClient<WebhookService>();

builder.Services.AddInfrastructureServices(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("MyAllowSpecificOrigins");

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "F4ConversationCloud API V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();


app.Run();
