using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            using (var connection = new SqlConnection(_dbContext.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"SELECT CCI.ImageID,CCI.FileName, CCI.Name, CCI.Category, CCI.ProductType, CCI.ConfigID, DC.MatrixId
                                FROM [infiniu2_catalog].[dbo].[CollectionsConfig] AS CC
	                                RIGHT JOIN [infiniu2_catalog].[dbo].[DecorConfig] AS DC
		                                ON CC.ConfigId2 = DC.MatrixId
	                                RIGHT JOIN [infiniu2_catalog].[dbo].[ClientsCatalogDecorConfig] AS CCDC
		                                ON CCDC.DecorID = DC.DecorID
			                                AND CCDC.ColorID = DC.ColorID
			                                AND CCDC.InsetTypeID = DC.InsetTypeID
			                                AND CCDC.InsetColorID = DC.InsetColorID
			                                AND CCDC.PatinaID = DC.PatinaID
	                                RIGHT JOIN [infiniu2_catalog].[dbo].[ClientsCatalogImages] AS CCI
		                                ON CCDC.ConfigID = CCI.ConfigID
                                WHERE CC.ConfigId1 = @matrixId AND ProductType = 1 AND ToSite = 1 
                                ORDER BY Name";
                IEnumerable<ProductDetailsViewModel> products = await connection.QueryAsync<ProductDetailsViewModel>(sql, new { matrixId });
                return View(products.ToList());
            }
        }
    }
}
