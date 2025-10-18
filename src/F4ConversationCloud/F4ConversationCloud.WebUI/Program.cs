using F4ConversationCloud.Application.Common.Handler;
using F4ConversationCloud.Application.Common.Models;
using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using F4ConversationCloud.Infrastructure;
using F4ConversationCloud.Infrastructure.Service;
using F4ConversationCloud.WebUI;
using F4ConversationCloud.WebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
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
builder.Services.AddWebUIServices(builder.Configuration);

WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();

whatsAppConfig.AccessToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;

    // Optional: log specific request/response headers
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("my-response-header");

    // Optional: include media types you want to log as plain text
    logging.MediaTypeOptions.AddText("application/javascript");

    // Optional: limit the size of logged request/response bodies
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("HasPermission", policy =>
//    {
//        policy.Requirements.Add(new PermissionRequirement("IsCreateTemplate"));
//        policy.Requirements.Add(new PermissionRequirement("IsDeleteTemplate"));
//        policy.Requirements.Add(new PermissionRequirement("IsEditTemplate"));
//        policy.Requirements.Add(new PermissionRequirement("IsView"));

//    });
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Permission_IsCreateTemplate", policy =>
        policy.Requirements.Add(new PermissionRequirement("True")));

});

var app = builder.Build();

app.UseHttpLogging();
app.UseMiddleware<LoggingMiddleware>();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("MyAllowSpecificOrigins");



app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "F4ConversationCloud API V1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
