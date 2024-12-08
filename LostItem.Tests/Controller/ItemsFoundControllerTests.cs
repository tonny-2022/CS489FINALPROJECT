using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FoundItemService.Controllers;
using FoundItemService.dto;
using FoundItemService.model;
using FoundItemService.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostItem.Tests.Controller
{
	public class ItemsFoundControllerTests
	{
		
			private readonly ItemFoundRepository itemFoundRepository;
			private readonly IMapper mapper;
			private readonly ItemsFoundController itemsFoundController;

			public ItemsFoundControllerTests()
			{
				// Initialize mocks
				itemFoundRepository = A.Fake<ItemFoundRepository>();
				mapper = A.Fake<IMapper>();

				// Initialize the controller with mocks
				itemsFoundController = new ItemsFoundController(itemFoundRepository, mapper);
			}

			[Fact]
			public async Task Create_ShouldReturnOkWithCreatedItem()
			{
				// Arrange
				var itemFoundRequest = new ItemFoundRequestDTO
				{
					Description = "Found Bag",
					DatetimeFound = DateTime.UtcNow,
					LocationFound = "Train Station",
					File = A.Fake<IFormFile>() // Mock file upload
				};

				var uploadedImage = new Image
				{
					FileName = "image",
					FileExtension = ".jpg",
					FilePath = "https://example.com/images/image.jpg"
				};

				var itemToSave = new ItemFound
				{
					FoundId = Guid.NewGuid(),
					Description = "Found Bag",
					DatetimeFound = DateTime.UtcNow,
					LocationFound = "Train Station",
					ImageUrl = uploadedImage.FilePath,
					CreateAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow,
					UserId = Guid.NewGuid()
				};

				var itemResponse = new ItemFoundResponseDTO
				{
					FoundId = itemToSave.FoundId,
					Description = itemToSave.Description,
					DatetimeFound = itemToSave.DatetimeFound,
					LocationFound = itemToSave.LocationFound,
					ImageUrl = itemToSave.ImageUrl,
					CreateAt = itemToSave.CreateAt,
					UpdatedAt = itemToSave.UpdatedAt,
					UserId = itemToSave.UserId
				};

				// Mock behavior
				A.CallTo(() => itemFoundRepository.UploadImage(A<Image>.Ignored)).Returns(uploadedImage);
				A.CallTo(() => mapper.Map<ItemFound>(itemFoundRequest)).Returns(itemToSave);
				A.CallTo(() => itemFoundRepository.CreateItemFoundAsync(itemToSave)).Returns(itemToSave);
				A.CallTo(() => mapper.Map<ItemFoundResponseDTO>(itemToSave)).Returns(itemResponse);

				// Act
				var result = await itemsFoundController.Create(itemFoundRequest);

				// Assert
				result.Should().NotBeNull();
				result.Should().BeOfType<OkObjectResult>();

				var okResult = result as OkObjectResult;
				okResult!.Value.Should().BeEquivalentTo(itemResponse);
			}


		[Fact]
		public async Task Update_ShouldReturnOkWithUpdatedItem()
		{
			// Arrange
			var itemFoundId = Guid.NewGuid();
			var itemFoundRequest = new ItemFoundRequestDTO
			{
				Description = "Updated Description",
				DatetimeFound = DateTime.UtcNow,
				LocationFound = "Office",
				File = A.Fake<IFormFile>() // Mock file upload
			};

			var uploadedImage = new Image
			{
				FileName = "updated_image",
				FileExtension = ".jpg",
				FilePath = "https://example.com/images/updated_image.jpg"
			};

			var itemToUpdate = new ItemFound
			{
				FoundId = itemFoundId,
				Description = "Updated Description",
				DatetimeFound = DateTime.UtcNow,
				LocationFound = "Office",
				ImageUrl = uploadedImage.FilePath,
				CreateAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				UserId = Guid.NewGuid()
			};

			var itemResponse = new ItemFoundResponseDTO
			{
				FoundId = itemToUpdate.FoundId,
				Description = itemToUpdate.Description,
				DatetimeFound = itemToUpdate.DatetimeFound,
				LocationFound = itemToUpdate.LocationFound,
				ImageUrl = itemToUpdate.ImageUrl,
				CreateAt = itemToUpdate.CreateAt,
				UpdatedAt = itemToUpdate.UpdatedAt,
				UserId = itemToUpdate.UserId
			};

			// Mock dependencies
			A.CallTo(() => itemFoundRepository.UploadImage(A<Image>.Ignored)).Returns(uploadedImage);
			A.CallTo(() => mapper.Map<ItemFound>(itemFoundRequest)).Returns(itemToUpdate);
			A.CallTo(() => itemFoundRepository.UpdateItemFoundAsync(itemFoundId, itemToUpdate)).Returns(itemToUpdate);
			A.CallTo(() => mapper.Map<ItemFoundResponseDTO>(itemToUpdate)).Returns(itemResponse);

			var controller = new ItemsFoundController(itemFoundRepository, mapper);

			// Act
			var result = await controller.Update(itemFoundId, itemFoundRequest);

			// Assert
			result.Should().NotBeNull();
			result.Should().BeOfType<OkObjectResult>();

			var okResult = result as OkObjectResult;
			okResult!.Value.Should().BeEquivalentTo(itemResponse);
		}



	}





}
