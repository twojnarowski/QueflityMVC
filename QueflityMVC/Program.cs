using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QueflityMVC.Application;
using QueflityMVC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<QueflityMVC.Infrastructure.Context>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<QueflityMVC.Infrastructure.Context>();
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase= true;
    options.Password.RequiredUniqueChars = 0;

    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddControllersWithViews();

builder.Services.AddInfrastructure();

builder.Services.AddApplication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    IConfigurationSection googleOAuthSection = builder.Configuration.GetSection("Authentication:Google");
    googleOptions.ClientId = googleOAuthSection["ClientId"];
    googleOptions.ClientSecret = googleOAuthSection["ClientSecret"];
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("CanManageIngredients", policy =>
        policy.RequireClaim("AddIngredient", "EditIngredient", "DeleteIngredient"));
    options.AddPolicy("CanManageItems", policy =>
        policy.RequireClaim("ViewItemsList","AddItem", "EditItem", "DeleteItem"));
    options.AddPolicy("CanManageItemCategories", policy =>
        policy.RequireClaim("ViewItemCategoriesList", "AddItemCategory", "EditItemCategory", "DeleteItemCategory"));
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
