using System.Collections.Generic;
using System.Diagnostics;
using LostItemUI.dto;
using LostItemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostItemUI.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory _httpClient)
		{
			_logger = logger;
			httpClient= _httpClient;	
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
            List<LostItemDTO> items = new List<LostItemDTO>();
            try
            {
                var client = httpClient.CreateClient();
                var response = await client.GetAsync("https://localhost:7008/api/LostItem");
                response.EnsureSuccessStatusCode();
                items.AddRange(await response.Content.ReadFromJsonAsync<List<LostItemDTO>>());
            }
            catch (Exception)
            {
            }

            return View(items);
            
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
