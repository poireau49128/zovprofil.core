using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductDbContext _dbContext;
        public CatalogController(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Production()
        {
            var products = await _dbContext.Products.Where(p => p.ProductType == 0 && p.ToSite == true).OrderBy(p => p.Category).ToListAsync();
            return View(products);
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
