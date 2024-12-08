using AutoMapper;
using ClaimService.Controllers;
using ClaimService.dto;
using ClaimService.model;
using ClaimService.repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClaimService.Tests
{
	public class ClaimsControllerTests
	{
		private readonly Mock<IClaimRepository> mockClaimRepository;
		private readonly Mock<IMapper> mockMapper;
		private readonly ClaimsController controller;

		public ClaimsControllerTests()
		{
			mockClaimRepository = new Mock<IClaimRepository>();
			mockMapper = new Mock<IMapper>();
			controller = new ClaimsController(mockMapper.Object, mockClaimRepository.Object);
		}

		[Fact]
		public async Task Delete_ExistingClaim_ReturnsOkResult()
		{
			// Arrange
			var claimId = Guid.NewGuid();
			var claim = new ItemClaim
			{
				ItemClaimID = claimId,
				MatchId = Guid.NewGuid(),
				VerificationDetails = "Verified",
				Status = "Claimed",
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			var claimResponseDto = new ItemClaimResponseDTO(
				claimId,
				claim.VerificationDetails,
				claim.Status,
				claim.CreatedAt,
				claim.UpdatedAt,
				claim.MatchId
			);

			mockClaimRepository.Setup(repo => repo.DeleteClaimAsync(claimId))
				.ReturnsAsync(claim);

			mockMapper.Setup(mapper => mapper.Map<ItemClaimResponseDTO>(claim))
				.Returns(claimResponseDto);

			// Act
			var result = await controller.Delete(claimId);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var returnedDto = Assert.IsType<ItemClaimResponseDTO>(okResult.Value);

			Assert.Equal(claimId, returnedDto.ClaimID);
			Assert.Equal(claim.VerificationDetails, returnedDto.VerificationDetails);
			Assert.Equal(claim.Status, returnedDto.Status);
			Assert.Equal(claim.CreatedAt, returnedDto.CreatedAt);
			Assert.Equal(claim.UpdatedAt, returnedDto.UpdatedAt);
			Assert.Equal(claim.MatchId, returnedDto.MatchId);
		}

		[Fact]
		public async Task Delete_NonExistingClaim_ReturnsNotFoundResult()
		{
			// Arrange
			var claimId = Guid.NewGuid();

			mockClaimRepository.Setup(repo => repo.DeleteClaimAsync(claimId))
				.ReturnsAsync((ItemClaim)null);

			// Act
			var result = await controller.Delete(claimId);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}
	
}
}
