using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Components
{
    public class FlexMenuViewComponent : ViewComponent
    {
        Dictionary<int, List<string>> types = new Dictionary<int, List<string>>
        {
            {0, ["ФАСАДЫ", "fronts.png"]},
            {1, ["ДЕКОРЫ", "profile.png"]},
            {2, ["МЕБЕЛЬ", "cup.png"]},
            {4, ["РЕКЛАМНАЯ ПРОДУКЦИЯ", "promotion.png"]},
            {5, ["ИНТЕРЬЕРНЫЕ ДЕКОРЫ", "interior.png"]},
        };
        private readonly ProductDbContext _dbContext;
        public FlexMenuViewComponent(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<List<string>, List<string>> categoryDictionary = new Dictionary<List<string>, List<string>>();

            foreach (int type in types.Keys)
            {
                var categories = await _dbContext.Products
                    .Where(product => product.ProductType==type && product.ToSite==true && !product.Category.Contains("Эксклюзив ZOV"))
                    .Select(product => product.Category)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToListAsync();

                categoryDictionary.Add(new List<string> { type.ToString(), types[type][0], types[type][1] }, categories);
            }
            return View(categoryDictionary);
        }
    }
}
