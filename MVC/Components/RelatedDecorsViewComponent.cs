using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Drawing.Drawing2D;

namespace MVC.Components
{
    public class RelatedDecorsViewComponent : ViewComponent
    {
        private readonly ProductDbContext _dbContext;
        public RelatedDecorsViewComponent(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(int matrixId)
        {
            return View(matrixId);
        }
    }
}
