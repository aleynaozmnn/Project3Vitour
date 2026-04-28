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
using System.Globalization; // Yeni: Dil ayarlarý için
using Microsoft.AspNetCore.Localization; // Yeni: Localization için

var builder = WebApplication.CreateBuilder(args);

// --- 1. LOCALIZATION SERVÝSLERÝ (YENÝ) ---
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization() // View'larda yerelleþtirme için
    .AddDataAnnotationsLocalization(); // Model validation'lar için

// --- Kimlik Doðrulama (Authentication) ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/Index";
        options.Cookie.Name = "VitourAdminCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// --- Diðer Servisler ---
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews().AddViewLocalization();

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

var app = builder.Build();

// --- 2. DÝL DESTEÐÝ YAPILANDIRMASI (YENÝ - CASE GEREKSÝNÝMÝ) ---
var supportedCultures = new[] {
    new CultureInfo("tr-TR"),
    new CultureInfo("en-US")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider(), // Case'de istenen Cookie tabanlý yönetim
        new QueryStringRequestCultureProvider()
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();