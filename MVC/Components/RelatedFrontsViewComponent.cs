using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MVC.Models;

namespace MVC.Components
{
    public class RelatedFrontsViewComponent : ViewComponent
    {
        private readonly ProductDbContext _dbContext;
		public RelatedFrontsViewComponent(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync(int matrixId, string name)
        {
            ViewBag.MainName = name;
            using (var connection = new SqlConnection(_dbContext.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"SELECT DISTINCT CCI.ImageID,CCI.FileName, CCI.Name, CCI.Category, CCI.ProductType, CCI.ConfigID
                                FROM [infiniu2_catalog].[dbo].[CollectionsConfig] AS CC
	                                RIGHT JOIN [infiniu2_catalog].[dbo].[FrontsConfig] AS FC
		                                ON CC.ConfigId2 = FC.MatrixId
	                                RIGHT JOIN [infiniu2_catalog].[dbo].[ClientsCatalogFrontsConfig] AS CCFC
		                                ON CCFC.FrontID = FC.FrontID
			                                AND CCFC.ColorID = FC.ColorID
			                                AND CCFC.InsetTypeID = FC.InsetTypeID
			                                AND CCFC.InsetColorID = FC.InsetColorID
			                                AND CCFC.PatinaID = FC.PatinaID
	                                RIGHT JOIN [infiniu2_catalog].[dbo].[ClientsCatalogImages] AS CCI
		                                ON CCFC.ConfigID = CCI.ConfigID
                                WHERE CC.ConfigId1 = @matrixId AND ProductType = 0 AND ToSite = 1";
                IEnumerable<ProductDetailsViewModel> products = await connection.QueryAsync<ProductDetailsViewModel>(sql, new { matrixId });
                var sortedProducts = products
                                        .OrderBy(p => p.Name == name ? 1 : 0) // Главный товар будет в конце
                                        .ToList();
                return View(sortedProducts);
            }
        }

        
    }
}
