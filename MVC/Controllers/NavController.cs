using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace MVC.Controllers
{
	public class NavController : Controller
	{
        private readonly ILogger<NavController> _logger;


        private readonly MarketingDbContext _dbContext;
		private readonly ProductDbContext _dbContextProduct;
		public NavController(MarketingDbContext dbContext, ProductDbContext dbProdContext, ILogger<NavController> logger)
		{
			_dbContext = dbContext;
			_dbContextProduct = dbProdContext;
            _logger = logger;
            _logger.LogInformation("YourController is being initialized.");
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

		public async Task<IActionResult> Downloads()
		{
            var documents = await _dbContext.Documents.ToListAsync();

            var fileIcons = new Dictionary<string, string>
            {
                { "pdf", "/img/PDFFile.png" },
                { "doc", "/img/WordFile.png" },
                { "docx", "/img/WordFile.png" },
                { "zip", "/img/ArchiveFile.png" },
                { "rar", "/img/ArchiveFile.png" },
                { "jpg", "/img/ImageFile.png" },
                { "png", "/img/ImageFile.png" }
            };
            ViewBag.FileIcons = fileIcons;

            return View(documents);
        }

		public IActionResult About()
		{
			return View();
		}

		public async Task<IActionResult> Contacts()
		{
            var sectionsWithContacts = await _dbContext.Sections
														.Include(s => s.Contacts)
														.ToListAsync();

            return View(sectionsWithContacts);
        }
        public async Task<IActionResult> Main()
        {
			var latestList = await _dbContextProduct.Products
				.Where(p => p.Latest==true)
				.ToListAsync();

			return View(latestList);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendEmail([FromBody] EmailModel model)
        {
            // Проверяем, валидны ли данные модели
            if (ModelState.IsValid)
            {
                try
                {
                    // Настройка SMTP-клиента
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("notify.infinium@gmail.com", "infinium1q2w3e4r"),
                        EnableSsl = true,
                    };

                    // Создаем сообщение электронной почты
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("notify.infinium@gmail.com"),
                        Subject = $"zovprofil.by. Сообщение от {model.Name}",
                        Body = "Посетитель сайта zovprofil.by написал сообщение для отдела маркетинга." +
                               $"\nEmail для ответа: {model.Email}" +
                               $"\nФИО: {model.Name}" +
                               $"\nКомпания: {model.Company}" +
                               $"\nТелефон: {model.Phone}" +
                               $"\nТип компании: {GetTypeDescription(model.Type)}" +
                               $"\nИнтересующая продукция: {GetProductDescription(model.Product)}" +
                               $"\nТекст сообщения: {model.Text}",
                        IsBodyHtml = false,
                    };

                    // Добавляем получателя
                    mailMessage.To.Add("marketing@omcprofil.by");

                    // Отправляем письмо
                    smtpClient.Send(mailMessage);

                    _logger.LogInformation("Email sent successfully from {EmailFrom} to {EmailTo}. Subject: {Subject}",
                        mailMessage.From.Address,
                        mailMessage.To.First().Address,
                        mailMessage.Subject);

                    return Ok();
                }
                catch (SmtpException smtpEx)
                {
                    // Логируем ошибку SMTP
                    _logger.LogError(smtpEx, "SMTP error occurred while sending email.");
                    return StatusCode(500, "Ошибка SMTP при отправке письма: " + smtpEx.Message);
                }
                catch (Exception ex)
                {
                    // Логируем любую другую ошибку
                    _logger.LogError(ex, "Unexpected error occurred while sending email.");
                    return StatusCode(500, "Ошибка при отправке письма: " + ex.Message);
                }
            }

            // Логируем сообщение об ошибке валидации
            _logger.LogWarning("Invalid model state: {ModelState}", ModelState);
            return BadRequest("Неверные данные");
        }

        private string GetTypeDescription(string type)
        {
            return type switch
            {
                "F" => "Физ. лицо",
                "R" => "Розница",
                "M" => "Мелкий опт",
                "K" => "Крупный опт",
                "D" => "Диз. студия",
                _ => "Неизвестно"
            };
        }

        private string GetProductDescription(string product)
        {
            return product switch
            {
                "P" => "Профиль",
                "Fr" => "Фасады",
                "Me" => "Мебель",
                "I" => "Интерьерные панели",
                _ => "Неизвестно"
            };
        }
    }

    public class EmailModel
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Product { get; set; }
        public string Text { get; set; }
    }
}