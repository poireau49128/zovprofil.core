using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MVC.Helpers;
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




app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/Production", async context =>
    {
        var query = context.Request.Query;

        var type = query["type"].ToString();
        var cat = query["cat"].ToString();
        var subcat = query["subcat"].ToString();
        var item = query["item"].ToString();

        bool isTypeValid = !string.IsNullOrEmpty(type);
        bool isCatValid = !string.IsNullOrEmpty(cat);
        bool isSubCatValid = !string.IsNullOrEmpty(subcat);
        bool isItemValid = !string.IsNullOrEmpty(item);

        bool isBasic;

        if (isTypeValid)
        {
            string newUrl = $"/catalog/{@TransliterationHelper.ProdutTypeIntToString(int.Parse(type))}";
            if (isCatValid)
            {
                newUrl = $"/catalog/{@TransliterationHelper.ProdutTypeIntToString(int.Parse(type))}/{@TransliterationHelper.ToLatin(cat)}";
                if (isSubCatValid)
                {
                    newUrl = $"/catalog/{@TransliterationHelper.ProdutTypeIntToString(int.Parse(type))}/{@TransliterationHelper.ToLatin(cat)}/{@TransliterationHelper.ToLatin(subcat)}";
                    if (isItemValid)
                    {
                        newUrl = $"/catalog/details/{item}";
                    } 
                }
                else if (isItemValid)
                {
                    newUrl = $"/catalog/details/{item}";
                }
            }
            context.Response.Redirect(newUrl);
        }
        else
        {
            context.Response.StatusCode = 400; // Bad Request
            await context.Response.WriteAsync("Invalid or missing parameters");
        }
    });

    // Other routes
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Nav}/{action=Main}");
});

// Ðåäèðåêò ñî ñòàðûõ àäðåñîâ
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
