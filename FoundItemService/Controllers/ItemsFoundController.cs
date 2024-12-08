using AutoMapper;
using FoundItemService.dto;
using FoundItemService.model;
using FoundItemService.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FoundItemService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemsFoundController : ControllerBase
	{
        private readonly IMapper mapper;
        private readonly ItemFoundRepository itemFoundRepository;
		
        public ItemsFoundController(ItemFoundRepository _itemFoundRepository,IMapper _mapper)
        {
            mapper = _mapper;
            itemFoundRepository = _itemFoundRepository; 
        }

        [HttpGet]   
        public  async Task<ActionResult> GetAll()
        {
            var itemsFoundList =await itemFoundRepository.GetAllItemFoundAsync();
            var itemFoundListDto = mapper.Map<IEnumerable<ItemFoundResponseDTO>>(itemsFoundList);
            return itemsFoundList == null ?NotFound(): Ok(itemFoundListDto); 
        }

        [HttpGet("{itemFoundId}")]
        public async Task<IActionResult> GetById([FromRoute]Guid itemFoundId)
        {
            var itemToDelete = await itemFoundRepository.GetItemFoundByIdAsync(itemFoundId);
            var itemToDeleteDto= mapper.Map<ItemFoundResponseDTO>(itemToDelete);
            return itemToDelete==null? NotFound(): Ok(mapper.Map<ItemFoundResponseDTO>(itemToDelete));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ItemFoundRequestDTO itemFoundRequestDTO)
        {
			ValidateFileUpload(new ImageUploadRequestDto { File = itemFoundRequestDTO.File });
			var image = new Image()

			{
				File = itemFoundRequestDTO.File,
				FileName = Path.GetFileNameWithoutExtension(itemFoundRequestDTO.File.FileName),
				FileExtension = Path.GetExtension(itemFoundRequestDTO.File.FileName)
			};
			var uploadedImg = await itemFoundRepository.UploadImage(image);

			if (itemFoundRequestDTO.DatetimeFound.Kind != DateTimeKind.Utc)
			{
				itemFoundRequestDTO.DatetimeFound = DateTime.SpecifyKind(itemFoundRequestDTO.DatetimeFound, DateTimeKind.Utc);
			}
			var itemToSave =  mapper.Map<ItemFound>(itemFoundRequestDTO);
			itemToSave.ImageUrl = uploadedImg.FilePath;
			await itemFoundRepository.CreateItemFoundAsync(itemToSave); 
            return itemToSave==null? NotFound() : Ok(mapper.Map<ItemFoundResponseDTO>(itemToSave));
        }

        [HttpPut]
        [Route("{itemFoundId}")]
        public async Task<IActionResult> Update([FromRoute]Guid itemFoundId, [FromBody] ItemFoundRequestDTO itemFoundRequestDTO)
        {


			ValidateFileUpload(new ImageUploadRequestDto { File = itemFoundRequestDTO.File });
			var image = new Image()

			{
				File = itemFoundRequestDTO.File,
				FileName = Path.GetFileNameWithoutExtension(itemFoundRequestDTO.File.FileName),
				FileExtension = Path.GetExtension(itemFoundRequestDTO.File.FileName)
			};
			var uploadedImg = await itemFoundRepository.UploadImage(image);

			var itemToUpdate = mapper.Map<ItemFound>(itemFoundRequestDTO);
			itemToUpdate.ImageUrl = uploadedImg.FilePath;


			await itemFoundRepository.UpdateItemFoundAsync(itemFoundId,itemToUpdate);
            return itemToUpdate == null ? NotFound(ModelState) : Ok(mapper.Map<ItemFoundResponseDTO>(itemToUpdate));
        }
        [HttpDelete("{itemFoundId}")]
        public  async Task<IActionResult> Delete([FromRoute] Guid itemFoundId)
        {
             await itemFoundRepository.DeleteItemFoundAsync(itemFoundId);
            return NoContent();
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
	}
}
