using DevForge_Connect.Data;
using DevForge_Connect.SendGrid;
using DevForge_Connect.SendGrid.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using DevForge_Connect.Entities.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:8000");
            builder.WithHeaders("Access-Control-Allow-Headers");
            builder.WithHeaders("Access-Control-Allow-Origin");
        });
});

//Send Grid service setup
builder.Services.Configure<SendGridEmailSenderOptions>(options =>
{
    // Gets sendgrid secrets from azure key config / azure key vault. 
    options.ApiKey = builder.Configuration["SendGrid:ApiKey"]!;
    options.SenderEmail = builder.Configuration["SendGrid:SenderEmail"]!;
    options.SenderName = "DevForge_Connect";
});

builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

builder.Services.AddHostedService<PythonServerHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Enables app cors policy for calling FastAPI.
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
