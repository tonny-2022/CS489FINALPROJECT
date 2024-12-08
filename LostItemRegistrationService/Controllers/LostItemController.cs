using AutoMapper;
using LostItemRegistrationService.dto;
using LostItemRegistrationService.model;
using LostItemRegistrationService.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostItemRegistrationService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	
	public class LostItemController : ControllerBase
	{
		private readonly ILostItemRegistrationRepository lostItemRegistrationRepository;
		private readonly IMapper mapper;
		private readonly IMageUploadRepository imageUploadRepository;
        public LostItemController(ILostItemRegistrationRepository _lostItemRegistrationRepository,
			                       IMapper _mapper,IMageUploadRepository _imageUploadRepository)
        {
            this.lostItemRegistrationRepository = _lostItemRegistrationRepository;	
			this.mapper = _mapper;
			this.imageUploadRepository = _imageUploadRepository;
        }

		[HttpGet]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll()
		{
			var itemsLost = await lostItemRegistrationRepository.GetAllAsync();
			var itemsLostDto = mapper.Map<List<LostItemResponseDTO>>(itemsLost);
			return Ok(itemsLostDto);

		}

		[HttpGet("{itemId}")]
		public  async Task<IActionResult> GetById([FromRoute]Guid itemId)
		{
			var item= await lostItemRegistrationRepository.GetByIdAsync(itemId);
			if (item is null)
			{
				return BadRequest();
			}
			return Ok(mapper.Map<LostItemResponseDTO>(item));	
		}

		
		[HttpPost]
		[Route("Create")]
		public async Task<IActionResult> Create([FromForm] LostItemRequestDTO lostItemRequestDTO)
		{
			ValidateFileUpload(new ImageUploadRequestDto { File = lostItemRequestDTO.File });
			var image = new Image()

			{
				File = lostItemRequestDTO.File,
				FileName = Path.GetFileNameWithoutExtension(lostItemRequestDTO.File.FileName),
				FileExtension = Path.GetExtension(lostItemRequestDTO.File.FileName)
			};
			var uploadedImg = await imageUploadRepository.UploadImage(image);
			
			if (lostItemRequestDTO.DatetimeLost.Kind != DateTimeKind.Utc)
			{
				lostItemRequestDTO.DatetimeLost = DateTime.SpecifyKind(lostItemRequestDTO.DatetimeLost, DateTimeKind.Utc);
			}
			var lostItem = mapper.Map<LostItemRegistration>(lostItemRequestDTO);
			lostItem.PhotoURL = uploadedImg.FilePath;
			await lostItemRegistrationRepository.CreateAsync(lostItem);
			var itemDto=mapper.Map<LostItemResponseDTO>(lostItem);	

			return Ok(itemDto);

		}

		[HttpPut("{itemid}")]	
		public async Task<IActionResult> Update([FromRoute] Guid itemid, [FromForm] LostItemRequestDTO lostItemRequestDTO)
		{
			ValidateFileUpload(new ImageUploadRequestDto { File = lostItemRequestDTO.File });
			var image = new Image()

			{
				File = lostItemRequestDTO.File,
				FileName = Path.GetFileNameWithoutExtension(lostItemRequestDTO.File.FileName),
				FileExtension = Path.GetExtension(lostItemRequestDTO.File.FileName)
			};
			var uploadedImg = await imageUploadRepository.UploadImage(image);
			if (lostItemRequestDTO.DatetimeLost.Kind != DateTimeKind.Utc)
			{
				lostItemRequestDTO.DatetimeLost = DateTime.SpecifyKind(lostItemRequestDTO.DatetimeLost, DateTimeKind.Utc);
			}

			var itemLost = mapper.Map<LostItemRegistration>(lostItemRequestDTO);
			itemLost.PhotoURL = uploadedImg.FilePath;

			await lostItemRegistrationRepository.UpdateAsync(itemid,itemLost);
			var itemLostDto = mapper.Map<LostItemResponseDTO>(itemLost);
			if (itemLostDto==null)
			{
				return NotFound(ModelState);
			}
			return Ok(itemLostDto);	
		}


	

		[HttpDelete("{itemid}")]
		public async Task<IActionResult> Delete(Guid itemid)
		{
			var item =await lostItemRegistrationRepository.DeleteAsync(itemid);
			if (item == null)
			{
				NotFound(ModelState);	
			}
			return Ok(mapper.Map<LostItemResponseDTO>(item));
				
		}

		private void ValidateFileUpload(ImageUploadRequestDto requestDto)

		{

			string[] filesExatension = new string[] { ".jpg", ".jpeg", ".png" };

			if (!filesExatension.Contains(Path.GetExtension(requestDto.File.FileName)))
			{
				ModelState.AddModelError("file", "File upload is not of supported format");
			}

			if (requestDto.File.Length > 10485760)
			{
				ModelState.AddModelError("file", "File size must not exceed 10 MB");
			}


		}

		[HttpGet]
		[Route("GetAllByUser")]

		public async Task<IActionResult> GetAllByUser()
		{
			var itemsLost = await lostItemRegistrationRepository.GetAllByUserAsync();
			var itemsLostDto = mapper.Map<List<LostItemResponseDTO>>(itemsLost);
			return Ok(itemsLostDto);

		}
		[HttpPatch("{lostId}/status")]
		public async Task<IActionResult> UpdateStatus([FromRoute] Guid lostId, [FromBody] UpdateStatusRequestDTO updateStatusRequest)
		{
			if (string.IsNullOrWhiteSpace(updateStatusRequest.Status))
			{
				return BadRequest("Status cannot be empty.");
			}
			try
			{
				var updatedStatus = await lostItemRegistrationRepository.UpdateStatus(lostId, updateStatusRequest.Status);
				return Ok(new { lostId = lostId, Status = updatedStatus });
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
		}

	}
}
