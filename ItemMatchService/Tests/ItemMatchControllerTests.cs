using AutoMapper;
using ItemMatchService.Controllers;
using ItemMatchService.dto;
using ItemMatchService.model;
using ItemMatchService.repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ItemMatchService.Tests
{
	public class ItemMatchControllerTests
	{
		
			private readonly Mock<ItemMatchRepository> mockRepository;
			private readonly Mock<IMapper> mockMapper;
			private readonly ItemMatchController controller;

			public ItemMatchControllerTests()
			{
				mockRepository = new Mock<ItemMatchRepository>();
				mockMapper = new Mock<IMapper>();
				controller = new ItemMatchController(mockRepository.Object, mockMapper.Object);
			}

			[Fact]
			public async Task GetAll_ReturnsOkResult_WhenItemsExist()
			{
				// Arrange
				var itemMatchList = new List<ItemMatch>
			{
				new ItemMatch { MatchId = Guid.NewGuid(), LostId = Guid.NewGuid(), FoundId = Guid.NewGuid() },
				new ItemMatch { MatchId = Guid.NewGuid(), LostId = Guid.NewGuid(), FoundId = Guid.NewGuid() }
			};

				var itemMatchListDto = new List<ItemMatchResponseDTO>
			{
				new ItemMatchResponseDTO { MatchId = itemMatchList[0].MatchId, LostId = itemMatchList[0].LostId, FoundId = itemMatchList[0].FoundId },
				new ItemMatchResponseDTO { MatchId = itemMatchList[1].MatchId, LostId = itemMatchList[1].LostId, FoundId = itemMatchList[1].FoundId }
			};

				mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(itemMatchList);
				mockMapper.Setup(mapper => mapper.Map<List<ItemMatchResponseDTO>>(itemMatchList)).Returns(itemMatchListDto);

				// Act
				var result = await controller.GetAll();

				// Assert
				var actionResult = Assert.IsType<OkObjectResult>(result);
				var returnValue = Assert.IsType<List<ItemMatchResponseDTO>>(actionResult.Value);
				Assert.Equal(itemMatchList.Count, returnValue.Count);  // Verify that the number of items is correct
			}

			[Fact]
			public async Task GetById_ReturnsOkResult_WhenItemExists()
			{
				// Arrange
				var matchId = Guid.NewGuid();
				var itemMatch = new ItemMatch { MatchId = matchId, LostId = Guid.NewGuid(), FoundId = Guid.NewGuid() };
				var itemMatchDto = new ItemMatchResponseDTO { MatchId = itemMatch.MatchId, LostId = itemMatch.LostId, FoundId = itemMatch.FoundId };

				mockRepository.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(itemMatch);
				mockMapper.Setup(mapper => mapper.Map<ItemMatchResponseDTO>(itemMatch)).Returns(itemMatchDto);

				// Act
				var result = await controller.GetById(matchId);

				// Assert
				var actionResult = Assert.IsType<OkObjectResult>(result);
				var returnValue = Assert.IsType<ItemMatchResponseDTO>(actionResult.Value);
				Assert.Equal(itemMatch.MatchId, returnValue.MatchId);
			}

			[Fact]
			public async Task Create_ReturnsOkResult_WhenItemIsCreated()
			{
				// Arrange
				var itemMatchRequestDto = new dto.ItemMatchRequestDTO { LostId = Guid.NewGuid(), FoundId = Guid.NewGuid() };
				var itemMatch = new ItemMatch { MatchId = Guid.NewGuid(), LostId = itemMatchRequestDto.LostId, FoundId = itemMatchRequestDto.FoundId };
				var itemMatchDto = new dto.ItemMatchResponseDTO { MatchId = itemMatch.MatchId, LostId = itemMatch.LostId, FoundId = itemMatch.FoundId };

				mockMapper.Setup(mapper => mapper.Map<ItemMatch>(itemMatchRequestDto)).Returns(itemMatch);
				mockRepository.Setup(repo => repo.CreateAsync(itemMatch)).Returns((Task<ItemMatch>)Task.CompletedTask);
				mockMapper.Setup(mapper => mapper.Map<ItemMatchResponseDTO>(itemMatch)).Returns(itemMatchDto);

				// Act
				var result = await controller.Create(itemMatchRequestDto);

				// Assert
				var actionResult = Assert.IsType<OkObjectResult>(result);
				var returnValue = Assert.IsType<ItemMatchResponseDTO>(actionResult.Value);
				Assert.Equal(itemMatch.MatchId, returnValue.MatchId);
			}

			[Fact]
			public async Task Update_ReturnsOkResult_WhenItemIsUpdated()
			{
				// Arrange
				var matchId = Guid.NewGuid();
				var itemMatchRequestDto = new ItemMatchRequestDTO { LostId = Guid.NewGuid(), FoundId = Guid.NewGuid() };
				var itemMatch = new ItemMatch { MatchId = matchId, LostId = itemMatchRequestDto.LostId, FoundId = itemMatchRequestDto.FoundId };
				var itemMatchDto = new ItemMatchResponseDTO { MatchId = itemMatch.MatchId, LostId = itemMatch.LostId, FoundId = itemMatch.FoundId };

				mockMapper.Setup(mapper => mapper.Map<ItemMatch>(itemMatchRequestDto)).Returns(itemMatch);
				mockRepository.Setup(repo => repo.UpdateAsync(matchId, itemMatch)).Returns((Task<ItemMatch>)Task.CompletedTask);
				mockMapper.Setup(mapper => mapper.Map<ItemMatchResponseDTO>(itemMatch)).Returns(itemMatchDto);

				// Act
				var result = await controller.Update(matchId, itemMatchRequestDto);

				// Assert
				var actionResult = Assert.IsType<OkObjectResult>(result);
				var returnValue = Assert.IsType<ItemMatchResponseDTO>(actionResult.Value);
				Assert.Equal(itemMatch.MatchId, returnValue.MatchId);
			}

			[Fact]
			public async Task Delete_ReturnsNoContent_WhenItemIsDeleted()
			{
				// Arrange
				var matchId = Guid.NewGuid();
				mockRepository.Setup(repo => repo.DeleteAsync(matchId)).Returns(Task.CompletedTask);

				// Act
				var result = await controller.Delete(matchId);

				// Assert
				Assert.IsType<NoContentResult>(result);
			}
		}
}
