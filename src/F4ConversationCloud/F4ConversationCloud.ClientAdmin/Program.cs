using F4ConversationCloud.Application.Common.Models.MetaModel.Configurations;
using F4ConversationCloud.ClientAdmin;
using F4ConversationCloud.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "SessionCookie";
});

builder.Services.AddAuthentication("CookieAuthentication")
                 .AddCookie("CookieAuthentication", config =>
                 {
                     config.Cookie.Name = "UserLoginCookie";
                     config.LoginPath = "/Auth/Login";
                     config.AccessDeniedPath = "/Auth/InvalidUrl";
                     config.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                     config.SlidingExpiration = true;
                 });
builder.Services.AddInfrastructureServices(builder.Configuration);

WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();

whatsAppConfig.AccessToken = builder.Configuration.GetSection("WhatsAppBusinessCloudApiConfiguration")["AccessToken"];
builder.Services.AddWhatsAppBusinessCloudApiService(whatsAppConfig);

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddWebUIServices(builder.Configuration);
builder.Services.AddControllersWithViews();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Auth/InvalidUrl");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();

