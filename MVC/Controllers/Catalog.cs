using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class Catalog : Controller
    {
        private readonly ProductDbContext _dbContext;
        public Catalog(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> GetProducts()
        {
            var products = await _dbContext.Products.Where(p => p.ProductType == 0 && p.ToSite == true).OrderBy(p => p.Category).ToListAsync();
            return View(products);
        }
    }
}
