using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Data;
using System.Linq;
using System.Numerics;

namespace MVC.Controllers
{
	public class NavController : Controller
	{
		private readonly MarketingDbContext _dbContext;
		private readonly ProductDbContext _dbContextProduct;
		public NavController(MarketingDbContext dbContext, ProductDbContext dbProdContext)
		{
			_dbContext = dbContext;
			_dbContextProduct = dbProdContext;
		}
		public async Task<IActionResult> News()
		{
			var newsList = await _dbContext.News
			.OrderByDescending(n => n.DateTime)
			.ToListAsync();

			return View(newsList);
		}

		public async Task<IActionResult> WhereToBuy(bool isFronts, bool isProfile, bool isFurniture, string city, string country = "Беларусь")
		{
            IQueryable<Dealer> dealers =  _dbContext.Dealers
									.Where(d => d.Lat!=null && d.Long!=null && d.Country!=null && d.Name!=null)
									.OrderBy(d => d.City);
            List<string?> countryList = await dealers
										.Select(s => s.Country)
										.Distinct().ToListAsync();
            List<string?> cityList = await dealers
										.Where(d=>d.Country==country)
										.Select(s => s.City)
										.Distinct().ToListAsync();
			cityList.Insert(0, "-");

            if (country != null)
            {
                dealers = dealers.Where(p => p.Country == country);
            }

            if (!String.IsNullOrEmpty(city) && city != "-")
            {
                dealers = dealers.Where(p => p.City == city);
            }

			if(isFronts)
                dealers = dealers.Where(p => p.isFronts == true);
            if (isProfile)
                dealers = dealers.Where(p => p.isProfile == true);
            if (isFurniture)
                dealers = dealers.Where(p => p.isFurniture == true);



            DealersListViewModel dlvm = new DealersListViewModel
            {
                Dealers = await dealers.ToListAsync(),
                City = new SelectList(cityList),
                Country = new SelectList(countryList)
            };
            return View(dlvm);
            
        }

		public IActionResult Downloads()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Contacts()
		{
			return View();
		}
        public async Task<IActionResult> Main()
        {
			var latestList = await _dbContextProduct.Products
				.Where(p => p.Latest==true)
				.ToListAsync();

			return View(latestList);
        }
    }
}
