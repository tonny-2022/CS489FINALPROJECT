using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using LostItemRegistrationService.Controllers;
using LostItemRegistrationService.dto;
using LostItemRegistrationService.model;
using LostItemRegistrationService.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostItem.Tests.Controller
{
	public class LostItemControllerTests
	{
		private readonly ILostItemRegistrationRepository lostItemRepostoiry;
		private readonly IMapper mapper;
		private readonly IMageUploadRepository imageUploadRepository;
		private readonly LostItemController lostItemController;

		public LostItemControllerTests()
        {
            lostItemRepostoiry=A.Fake<ILostItemRegistrationRepository>();
			imageUploadRepository = A.Fake<IMageUploadRepository>();
			mapper = A.Fake<IMapper>();
        }
		


		[Fact]
		public async Task LostItemController_GetAllLostItems_ReturnOk()
		{
			var lostItemEntities = new List<LostItemRegistration>
			{
				new() { LostId = Guid.NewGuid(), Description = "Lost Wallet", LocationLost = "Park", UserId = "User123" },
				new() { LostId = Guid.NewGuid(), Description = "Lost Keys", LocationLost = "Office", UserId = "User456" }
			};

			var lostItemsDto = new List<LostItemResponseDTO>
			{
			new() { LostId = lostItemEntities[0].LostId, Description = "Lost Wallet", LocationLost = "Park", UserId = "User123" },
			new() { LostId = lostItemEntities[1].LostId, Description = "Lost Keys", LocationLost = "Office", UserId = "User456" }
			};

			// Mock repository behavior to return the list of entities
			A.CallTo(() => lostItemRepostoiry.GetAllAsync()).Returns(Task.FromResult(lostItemEntities));

			// Mock mapping behavior to map entities to DTOs
			A.CallTo(() => mapper.Map<List<LostItemResponseDTO>>(lostItemEntities)).Returns(lostItemsDto);

			// Instantiate the controller
			var controller = new LostItemController(lostItemRepostoiry, mapper, imageUploadRepository);

			// Act
			var result = await controller.GetAll();

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<OkObjectResult>();

			var okResult = result as OkObjectResult;
			okResult!.Value.Should().BeEquivalentTo(lostItemsDto); // Validate the mapped response


		}

		[Fact]
		public async Task LostItemController_Create_ReturnsOk()
		{
			// Arrange
			var lostItemRequest = new LostItemRequestDTO
			{
				Description = "Lost Phone",
				LocationLost = "Cafe",
				DatetimeLost = DateTime.UtcNow,
				File = A.Fake<IFormFile>() // Mock file upload
			};

			var uploadedImage = new Image
			{
				FileName = "image",
				FileExtension = ".jpg",
				FilePath = "https://example.com/images/image.jpg"
			};

			var lostItem = new LostItemRegistration
			{
				LostId = Guid.NewGuid(),
				Description = "Lost Phone",
				LocationLost = "Cafe",
				DatetimeLost = DateTime.UtcNow,
				PhotoURL = uploadedImage.FilePath,
				UserId = "User123"
			};

			var lostItemResponse = new LostItemResponseDTO
			{
				LostId = lostItem.LostId,
				Description = lostItem.Description,
				LocationLost = lostItem.LocationLost,
				DatetimeLost = lostItem.DatetimeLost,
				PhotoURL = lostItem.PhotoURL,
				UserId = lostItem.UserId
			};

			// Mock file validation behavior
			A.CallTo(() => imageUploadRepository.UploadImage(A<Image>.Ignored)).Returns(Task.FromResult(uploadedImage));

			// Mock mapping behavior
			A.CallTo(() => mapper.Map<LostItemRegistration>(lostItemRequest)).Returns(lostItem);
			A.CallTo(() => mapper.Map<LostItemResponseDTO>(lostItem)).Returns(lostItemResponse);

			// Mock repository behavior
			A.CallTo(() => lostItemRepostoiry.CreateAsync(A<LostItemRegistration>.Ignored)).Returns(Task.FromResult(lostItem));

			var controller = new LostItemController(lostItemRepostoiry, mapper, imageUploadRepository);

			// Act
			var result = await controller.Create(lostItemRequest);

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<OkObjectResult>();

			var okResult = result as OkObjectResult;
			okResult!.Value.Should().BeEquivalentTo(lostItemResponse);
		}


	}
}
