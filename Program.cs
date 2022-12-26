using System.Globalization;
using Microsoft.AspNetCore.Identity;
using WebMarkupMin.AspNetCore7;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using robotmanden.Code;
using robotmanden.Models;
using robotmanden.Resources;
using robotmanden.Services;
using robotmanden.SQL;
using WebMarkupMin.AspNet.Common.UrlMatchers;
using WebMarkupMin.Core;

var builder = WebApplication.CreateBuilder(args);

// Allows the static class of SQLConnectionClass to connect via configuration.
// Not a good practice, but the only one seeming available
SQLConnectionClass.StaticConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Project setup
builder.Services.AddSingleton<ProjectSetupClass>(_ =>
    builder.Configuration.GetSection("ProjectSetup").Get<ProjectSetupClass>());

#region Services

// Identity
builder.Services.AddTransient<IUserStore<AccountUserModel>, UserStoreClass>();
builder.Services.AddTransient<IRoleStore<AccountRoleModel>, RoleStoreClass>();
builder.Services.ConfigureApplicationCookie(o => // Identity cookie setup
{
    o.Cookie.HttpOnly = true;
    o.ExpireTimeSpan = TimeSpan.FromMinutes(240);
    o.SlidingExpiration = true;
});
builder.Services.AddIdentity<AccountUserModel, AccountRoleModel>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Global container as singleton
builder.Services.AddSingleton<GlobalContainerService>();

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();

// Localization
builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new[]
    {
        new CultureInfo("da-DK"), new CultureInfo("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("da-DK");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});
builder.Services.AddSingleton<GlobalTranslationService>(); // For custom data annotation localizations

// Additional services
builder.Services
    .AddMvc()
    .AddRazorPagesOptions(options => { options.Conventions.AuthorizeFolder("/Home"); })
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix) // For views
    .AddDataAnnotationsLocalization(options => // For default data annotations
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(CommonResource));
    });

// HTML Minification
builder.Services.AddWebMarkupMin(options =>
    {
        options.AllowMinificationInDevelopmentEnvironment = false;
        options.AllowCompressionInDevelopmentEnvironment = false;
    })
    .AddHtmlMinification(options =>
    {
        options.ExcludedPages = new List<IUrlMatcher>
        {
            new WildcardUrlMatcher("/minifiers/x*ml-minifier"),
        };

        HtmlMinificationSettings settings = options.MinificationSettings;
        settings.RemoveHtmlComments = true;
        settings.MinifyEmbeddedJsCode = true;
        settings.RemoveRedundantAttributes = true;
        settings.RemoveHttpProtocolFromAttributes = true;
        settings.RemoveHttpsProtocolFromAttributes = true;
    });

#endregion

#region app

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseWebMarkupMin(); //  HTML Minification

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

// Antiforgery
var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    string path = context.Request.Path.Value;

    if (!string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(path, "/index.html", StringComparison.OrdinalIgnoreCase)) return next(context);
    // The request token can be sent as a JavaScript-readable cookie, 
    // and Angular uses it by default.
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
        new CookieOptions()
        {
            // Set the secure flag, which Chrome's changes will require for SameSite none.
            // Note this will also require you to be running on HTTPS.
            Secure = true,

            // Set the cookie to HTTP only which is good practice unless you really do need
            // to access it client side in scripts.
            HttpOnly = true,

            // Add the SameSite attribute, this will emit the attribute with a value of none.
            // To not emit the attribute at all set
            // SameSite = (SameSiteMode)(-1)
            SameSite = SameSiteMode.Lax,

            // Expires after 2 hours
            Expires = DateTimeOffset.Now.AddHours(2),

            // Not sure?
            MaxAge = TimeSpan.FromMinutes(240)
        }
    );
    return next(context);
});


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

#endregion

app.Run();
