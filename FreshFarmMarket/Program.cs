using FreshFarmMarket.Model;
using FreshFarmMarket.Services;
using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<FreshFarmMarket.Model.DatabaseContext>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 3;

})
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));

builder.Services.AddTransient(typeof(GoogleCaptcha));

builder.Services.AddDataProtection();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(5);
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "AspNetCore.Identity.Application";
    options.ExpireTimeSpan = TimeSpan.FromSeconds(5);
    options.SlidingExpiration = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
    options.Lockout.MaxFailedAccessAttempts = 3;

});

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "4815300734-a65m2otnpr1452j561gvmhpt1qeic6a0.apps.googleusercontent.com";
    options.ClientSecret = "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";

});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePages(context =>
{
    context.HttpContext.Response.ContentType = "text/plain";
    switch (context.HttpContext.Response.StatusCode)
    {
        case 404:
            context.HttpContext.Response.Redirect("/ErrorPage/Error404");
            break;
        case 403:
            context.HttpContext.Response.Redirect("/ErrorPage/Error403");
            break;
        default:
            context.HttpContext.Response.Redirect("/ErrorPage/Error");
            break;
    }
    return Task.CompletedTask;

});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
