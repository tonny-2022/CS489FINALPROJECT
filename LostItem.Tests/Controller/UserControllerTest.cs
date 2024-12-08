using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using LoginService.Controllers;
using LoginService.dto;
using LoginService.model;
using LoginService.repository;
using LostItemRegistrationService.repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LostItem.Controllers
{
	public class UserControllerTest
	{
		private readonly IMapper mapper;
		private readonly IUserRepository userRepository;
		private readonly UserController userController;

		public UserControllerTest()
        {
			userRepository = A.Fake<IUserRepository>();	
			mapper = A.Fake<IMapper>();
			userController = new UserController(userRepository, mapper);
		}


		[Fact]
		public async Task GetAll_ShouldReturnOkWithUsers()
		{
			// Arrange
			var users = new List<ApplicationUser>
	{
		new() { Id = Guid.NewGuid().ToString(), FirstName = "John", LastName = "Doe", Location = "New York", UserName = "johndoe", Email = "johndoe@example.com", PhoneNumber = "1234567890" },
		new() { Id = Guid.NewGuid().ToString(), FirstName = "Jane", LastName = "Smith", Location = "Los Angeles", UserName = "janesmith", Email = "janesmith@example.com", PhoneNumber = "9876543210" }
	};

			var usersDto = new List<UserResponseDTO>
	{
		new() { Id = users[0].Id, FirstName = "John", LastName = "Doe", Location = "New York", UserName = "johndoe", PhoneNumber = "1234567890", Roles = new string[0] },
		new() { Id = users[1].Id, FirstName = "Jane", LastName = "Smith", Location = "Los Angeles", UserName = "janesmith", PhoneNumber = "9876543210", Roles = new string[0] }
	};

			A.CallTo(() => userRepository.GetAllUsersAsyc()).Returns(users);
			A.CallTo(() => mapper.Map<List<UserResponseDTO>>(users)).Returns(usersDto);

			// Act
			var result = await userController.GetAll();

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<OkObjectResult>();

			var okResult = result as OkObjectResult;
			okResult!.Value.Should().BeEquivalentTo(usersDto);
		}

		[Fact]
		public async Task GetById_ShouldReturnOkWithUser()
		{
			// Arrange
			var userId = Guid.NewGuid().ToString();
			var user = new ApplicationUser
			{
				Id = userId,
				FirstName = "John",
				LastName = "Doe",
				Location = "New York",
				UserName = "johndoe",
				Email = "johndoe@example.com",
				PhoneNumber = "1234567890"
			};

			var userDto = new UserResponseDTO
			{
				Id = user.Id,
				FirstName = "John",
				LastName = "Doe",
				Location = "New York",
				UserName = "johndoe",
				PhoneNumber = "1234567890",
				Roles = new string[0]
			};

			A.CallTo(() => userRepository.GetUserByIdAsync(Guid.Parse(userId))).Returns(user);
			A.CallTo(() => mapper.Map<UserResponseDTO>(user)).Returns(userDto);

			// Act
			var result = await userController.GetById(Guid.Parse(userId));

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<OkObjectResult>();

			var okResult = result as OkObjectResult;
			okResult!.Value.Should().BeEquivalentTo(userDto);

		}
		}
	}
