using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVC.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProductDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDb")));
builder.Services.AddDbContext<MarketingDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("MarketingDb")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Catalog/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Nav}/{action=Main}"
);

// Редирект со старых адресов
//app.MapControllerRoute(
//    name: "oldType",
//    pattern: "Production",
//    defaults: new { controller = "Catalog", action = "Production" }
//);

//app.MapControllerRoute(
//    name: "oldCategory",
//    pattern: "Production/{type}/{category}",
//    defaults: new { controller = "Catalog", action = "Production" }
//);

//app.MapControllerRoute(
//    name: "oldCategoryProduct",
//    pattern: "Production/{type?}/{category?}/{item}",
//    defaults: new { controller = "Catalog", action = "ProductDetails" },
//    constraints: new { item = @"\d+" }
//);

//app.MapControllerRoute(
//    name: "oldSubCategory",
//    pattern: "Production/{type}/{category}/{subcategory}",
//    defaults: new { controller = "Catalog", action = "Production" }
//);

//app.MapControllerRoute(
//    name: "oldSubcategoryProduct",
//    pattern: "Production/{typ?}/{category?}/{subcategory?}/{item}",
//    defaults: new { controller = "Catalog", action = "ProductDetails" }
//);


app.Run();
