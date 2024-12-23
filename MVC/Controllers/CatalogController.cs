﻿using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC.Helpers;
using MVC.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;

namespace MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ProductDbContext _dbContext;
        public CatalogController(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        [Route("catalog/{productTypeTranslit}/{categoryTranslit?}/{nameTranslit?}")]
        public async Task<IActionResult> Production(string categoryTranslit, bool isBasic, string nameTranslit/*, int productType=0*/, string productTypeTranslit = "facade")
        {
            string category = TransliterationHelper.ToCyrillic(categoryTranslit);
            string name = TransliterationHelper.ToCyrillic(nameTranslit);
            int productType = TransliterationHelper.ProdutTypeStringToInt(productTypeTranslit);

            List<Product> products;
            if (category.IsNullOrEmpty())
            {
                products = await UniqueCategories(productType);
                ViewBag.Operation = "UniqueCategories";
            }
            else if (isBasic == true)
            {
                if(productType == 0)
                {
                    products = await BasicProducts(productType, category, isBasic);
                    ViewBag.Operation = "BasicProducts";
                }
                else{
                    products = await GroupedProducts(productType, category);
                    ViewBag.Operation = "GroupedProducts";
                }
            }
            else
            {
                products = await BasicProducts(productType, category, isBasic, name);
                ViewBag.Operation = "BasicProducts";
            }


            ViewBag.CatSlider = await GetCategorySlider(category);
            return View(products);
        }

        private async Task<List<Product>> UniqueCategories(int productType)
        {
            var query = _dbContext.Products
                                    .Where(p => p.ProductType == productType && p.ToSite == true && !p.Category.Contains("Эксклюзив ZOV"))
                                    .GroupBy(p => p.Category)
                                    .Select(group => group.First(p => p.Basic == true) ?? group.First());
            List<Product> uniqueCategories = await query.ToListAsync();
            return uniqueCategories;
        }

        //Выбирает основные/все продукты категории
        private async Task<List<Product>> BasicProducts(int productType, string category, bool isBasic, string name=null)
        {
            var query = _dbContext.Products
                                    .Where(p => p.ProductType == productType 
                                                && p.Category == category 
                                                && p.Basic == isBasic 
                                                && p.ToSite == true 
                                                && (name == null || p.Name.StartsWith(name)))
                                    .OrderBy(p => name != null ? p.Name : p.Color);
            List<Product> basicProducts = await query.ToListAsync();
            return basicProducts;
        }
        private async Task<List<Product>> GroupedProducts(int productType, string category)
        {
            var query = _dbContext.Products
                                    .Where(p => p.ProductType == productType && p.Category == category && p.ToSite == true)
                                    .AsEnumerable()
                                    .GroupBy(p => p.Category != "Столы" && p.Name.Contains(" ") ? p.Name.Substring(0, p.Name.IndexOf(' ')) : p.Name)
                                    .Select(g => g.First())
                                    .OrderBy(p=>p.Name);

            List<Product> groupedProducts = query.ToList();
            return groupedProducts;
        }

        [Route("catalog/details/{imageId}")]
        public async Task<IActionResult> ProductDetails(long imageId)
        {
            ProductDetailsViewModel? product = await _dbContext.Products
                                        .Where(p => p.ImageId == imageId)
                                        .Select(p => new ProductDetailsViewModel 
                                            {
                                                ConfigId = p.ConfigId,
                                                ProductType = p.ProductType,
                                                Name = p.Name,
                                                Color = p.Color,
                                                FileName = p.FileName,
                                                Description = p.Description,
                                                Sizes = p.Sizes,
                                                Material = p.Material,
                                                Category = p.Category,
                                            })
                                        .FirstOrDefaultAsync();
            if (product != null) {
                string? techFileName;
                string sql;

                using (var connection = new SqlConnection(_dbContext.GetConnectionString()))
                {
                    await connection.OpenAsync();
                    if (product.ProductType == 0)
                    {
                        sql = @"SELECT FrontId 
                                FROM [infiniu2_catalog].[dbo].[ClientsCatalogFrontsConfig]
                                WHERE ConfigID = @configid"; 
                    }
                    else
                    {
                        sql = @"SELECT DecorId 
                                FROM [infiniu2_catalog].[dbo].[ClientsCatalogDecorConfig]
                                WHERE ConfigID = @configid";

                    }
                    long techConfig = await connection.QueryFirstOrDefaultAsync<long>(sql, new { product.ConfigId });
                    sql = @"SELECT FileName 
                            FROM [infiniu2_catalog].[dbo].[TechStoreDocuments]
                            WHERE TechId = @techConfig AND DocType = 0";

                    techFileName = await connection.QueryFirstOrDefaultAsync<string>(sql, new { techConfig });
                }
                
                product.CategoryColors = await GetCategoryColors(product.Category, product.ProductType);
                product.CatSlider = await GetCategorySlider(product.Category);
                product.MatrixId = await GetMatrixIdFromConfigId(product.ConfigId, product.ProductType);
                product.techStoreFileName = techFileName;
            }

            return View(product);
        }
        private async Task<int> GetMatrixIdFromConfigId(long configId, int productType)
        {
            if (productType != 0)
                return 0;
            using (var connection = new SqlConnection(_dbContext.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"SELECT MatrixId 
                                FROM [infiniu2_catalog].[dbo].[FrontsConfig] as FC 
                                LEFT JOIN [ClientsCatalogFrontsConfig] AS CCF 
                                    ON CCF.FrontID = FC.FrontID 
                                    AND CCF.ColorID = FC.ColorID 
                                    AND CCF.InsetTypeID = FC.InsetTypeID 
                                    AND CCF.PatinaID = FC.PatinaID 
                                    AND CCF.InsetColorID = FC.InsetColorID 
                                WHERE ConfigID = @configid AND Enabled = 1";
                var matrixId = await connection.QueryFirstOrDefaultAsync<int>(sql, new { configId });
                return matrixId;
            }
        }

        private async Task<List<CategoryColors>> GetCategoryColors(string category, int productType)
        {
            if (productType != 0)
                return null;
            var colors = await _dbContext.Products
                    .Where(p => p.Category == category 
                                && p.ProductType == productType 
                                && p.Color != "" 
                                && p.Color != null 
                                && p.ToSite == true 
                                && p.Basic == true)
                    .Select(p => new CategoryColors
                    {
                        ImageId = p.ImageId,
                        FileName = p.FileName,
                        Color = p.Color,
                    })
                    .ToListAsync();

            return colors;
        }
        private async Task<Tuple<string, List<string>>> GetCategorySlider(string category)
        {
            var filesAndDescriptions = await _dbContext.Products
                                                .Where(p => p.Category == category && p.ProductType == 3 && p.CatSlider == true)
                                                .Select(p => new
                                                {
                                                    Description = p.Description,
                                                    FileName = p.FileName
                                                })
                                                .ToListAsync();

            if (filesAndDescriptions.Any())
            {
                string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "ClientsCatalogImages");

                var validFiles = new List<string>();
                foreach (var file in filesAndDescriptions)
                {
                    string fullPath = Path.Combine(imagesFolderPath, file.FileName);

                    if (System.IO.File.Exists(fullPath))
                    {
                        validFiles.Add(file.FileName);
                    }
                }

                if (validFiles.Any())
                {
                    var nonEmptyDescription = filesAndDescriptions
                                                .Select(f => f.Description)
                                                .FirstOrDefault(d => !string.IsNullOrEmpty(d));
                    return Tuple.Create(nonEmptyDescription, validFiles);
                }
            }
            return null;
        }
        //private async Task<Tuple<string, List<string>>> GetCategorySlider(string category)
        //{
        //    var filesAndDescriptions = await _dbContext.Products
        //                                        .Where(p => p.Category == category && p.ProductType == 3 && p.CatSlider == true)
        //                                        .Select(p => new
        //                                        {
        //                                            Description = p.Description,
        //                                            FileName = p.FileName
        //                                        })
        //                                        .ToListAsync();

        //    if (filesAndDescriptions.Any())
        //    {
        //        string baseUrl = "https://zovprofil.by/Images/ClientsCatalogImages/";

        //        var validFiles = new List<string>();
        //        foreach (var file in filesAndDescriptions)
        //        {
        //            string fileUrl = $"{baseUrl}{file.FileName}";

        //            if (await FileExistsOnHttpAsync(fileUrl))
        //            {
        //                validFiles.Add(file.FileName);
        //            }
        //        }

        //        if (validFiles.Any())
        //        {
        //            var nonEmptyDescription = filesAndDescriptions
        //                                        .Select(f => f.Description)
        //                                        .FirstOrDefault(d => !string.IsNullOrEmpty(d));
        //            return Tuple.Create(nonEmptyDescription, validFiles);
        //        }
        //    }
        //    return null;
        //}

        //private async Task<bool> FileExistsOnHttpAsync(string fileUrl)
        //{
        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, fileUrl));
        //            return response.StatusCode == System.Net.HttpStatusCode.OK;
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        return false;
        //    }
        //}



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
