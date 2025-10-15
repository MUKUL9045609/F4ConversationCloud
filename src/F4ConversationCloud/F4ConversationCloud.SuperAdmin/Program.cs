using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using F4ConversationCloud.Infrastructure;
using F4ConversationCloud.SuperAdmin.Handler;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication("CookieAuthentication")
                 .AddCookie("CookieAuthentication", config =>
                 {
                     config.Cookie.Name = "UserLoginCookie"; // Name of cookie   
                     config.LoginPath = "/Auth/Login"; // Path for the redirect to user login page  
                     config.AccessDeniedPath = "/Auth/AccessDenied"; // Path for the Access Denied page
                 });

builder.Services.AddAuthorization(config =>
{
    var userAuthPolicyBuilder = new AuthorizationPolicyBuilder();
    config.DefaultPolicy = userAuthPolicyBuilder
                        .RequireAuthenticatedUser()
                        .RequireClaim(ClaimTypes.Role)
                        .Build();
});

builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices(builder.Configuration);


WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();

whatsAppConfig.AccessToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
