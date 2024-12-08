using System.Net.Http;
using System.Text;
using System.Text.Json;
using LostItemUI.dto;
using LostItemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LostItemUI.Controllers
{
	public class UserController : Controller
	{private readonly IHttpClientFactory httpClient;
        public UserController(IHttpClientFactory _httpClient)
        {
            this.httpClient = _httpClient;	
        }
		[HttpGet]	
        public async Task< IActionResult> ViewUsers()
		{
			List<UserResponseDTO> users= new List<UserResponseDTO>();
			try
			{
				var client = httpClient.CreateClient();
				var response = await client.GetAsync("https://localhost:7031/api/User");
				response.EnsureSuccessStatusCode();
				users.AddRange(await response.Content.ReadFromJsonAsync<List<UserResponseDTO>>());
			}
			catch (Exception)
			{
			}
			
			return View(users);
		}

		[HttpGet]
		public IActionResult AddUser()
		{
			return View();	
		}

		[HttpPost]
        public async Task<IActionResult> AddUser(AddUserModel userModel)
		{

			var client = httpClient.CreateClient();
			var httpRequestMsg = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7031/api/Auth/registeradmin"),
				Content = new StringContent(JsonSerializer.Serialize(userModel),Encoding.UTF8,"application/json")  
			};
			var httpResponseMsg = await client.SendAsync(httpRequestMsg);
			if (!httpResponseMsg.IsSuccessStatusCode)
			{
                var responseContent = await httpResponseMsg.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw Error Response: {responseContent}");
                return View(); 
            }
			httpResponseMsg.EnsureSuccessStatusCode();
			var responseObj=await httpResponseMsg.Content.ReadFromJsonAsync<UserResponseDTO>();
			if(responseObj != null)
			{
				return RedirectToAction("ViewUsers", "User");
			}

			return View();
		}

		[HttpGet]

		public async Task< IActionResult> EditUser(Guid userId)
		{

			var client = httpClient.CreateClient();
			var responseObj = await client.GetFromJsonAsync<UserResponseDTO>($"https://localhost:7031/api/user/{userId}");
			if (responseObj is not null)
			{
				return View(responseObj);

			}
			return View(null);
			
			
		}

		[HttpPost]
		public async Task<IActionResult> EditUser(UserResponseDTO userResponseDTO)
		{
			var client = httpClient.CreateClient();

			var requestObj = new HttpRequestMessage()
			{

				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:7031/api/user/{userResponseDTO.Id}"),
				Content = new StringContent(JsonSerializer.Serialize(userResponseDTO), Encoding.UTF8, "application/json")

			};

			var responseObj = await client.SendAsync(requestObj);
			if (responseObj.IsSuccessStatusCode) {
				var updatedUser = await requestObj.Content.ReadFromJsonAsync<UserResponseDTO>();
				if (updatedUser != null)
				{
					//return RedirectToAction("EditUser", "User");
					return View(updatedUser);
				}
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteUser(UserResponseDTO userResponseDTO)
		{

			try
			{
				var client = httpClient.CreateClient();
				var responseObj = await client.DeleteAsync($"https://localhost:7031/api/user/{userResponseDTO.Id}");

				var response = responseObj.EnsureSuccessStatusCode();

				if (responseObj is not null)
				{
					return RedirectToAction("ViewUsers", "User");
				}

			}
			catch (Exception)
			{

				throw;
			}

			return View("EditUser");

		}
	}
}
