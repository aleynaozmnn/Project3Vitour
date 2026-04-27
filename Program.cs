using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Project3Vitour.Services.CategoryServices;
using Project3Vitour.Services.DestinationService;
using Project3Vitour.Services.ImageService;
using Project3Vitour.Services.ReservationService;
using Project3Vitour.Services.ReviewServices;
using Project3Vitour.Services.SetingsService;
using Project3Vitour.Services.SettingsServices;
using Project3Vitour.Services.TourPlanService;
using Project3Vitour.Services.TourPlanServices;
using Project3Vitour.Services.TourServices;
using Project3Vitour.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- YENÝ: Kimlik Doðrulama (Authentication) Servisi Eklendi ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Index";
        options.Cookie.Name = "VitourAdminCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettingKey"));
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ITourPlanService, TourPlanService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- YENÝ: Authentication ve Authorization Sýralamasý Düzenlendi ---
app.UseAuthentication(); // Önce kimlik kontrolü
app.UseAuthorization();  // Sonra yetki kontrolü

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();