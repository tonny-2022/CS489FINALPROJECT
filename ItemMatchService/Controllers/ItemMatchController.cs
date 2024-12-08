using AutoMapper;
using ItemMatchService.dto;
using ItemMatchService.model;
using ItemMatchService.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemMatchService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemMatchController(ItemMatchRepository _itemMatchRepository, IMapper _mapper) : ControllerBase
	{

		private readonly ItemMatchRepository itemMatchRepository = _itemMatchRepository;
		private readonly IMapper mapper = _mapper;

		[HttpGet]
		public  async Task<ActionResult> GetAll()
		{
          var itemsMatch= await itemMatchRepository.GetAllAsync();
		  var itemsMatchDto=mapper.Map<List<ItemMatchResponseDTO>>(itemsMatch);	
			return itemsMatch == null ? NotFound() : Ok(itemsMatchDto);
		}

		[HttpGet("{matchId}")]
		public async Task<ActionResult> GetById([FromRoute]Guid matchId)
		{
			var itemMatch = await itemMatchRepository.GetByIdAsync(matchId);

			var itemMatchDto = mapper.Map<ItemMatchResponseDTO>(itemMatch);
			return itemMatch==null?NotFound(): Ok(itemMatchDto);

		}
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ItemMatchRequestDTO itemMatchRequestDTO)
		{
			var itemMatch = mapper.Map<ItemMatch>(itemMatchRequestDTO);
			await itemMatchRepository.CreateAsync(itemMatch);
			return Ok(mapper.Map<ItemMatchResponseDTO>(itemMatch));

		}

		[HttpPut("{matchId}")]
		//[Route("{matchId}")]
		public async Task<IActionResult> Update([FromRoute] Guid matchId, [FromBody] ItemMatchRequestDTO itemMatchRequestDTO)
		{
			var matchItem = mapper.Map<ItemMatch>(itemMatchRequestDTO);
			await itemMatchRepository.UpdateAsync(matchId,matchItem);

			return matchItem==null?NotFound(ModelState) : Ok(mapper.Map<ItemMatchResponseDTO>(matchItem));
		}

		[HttpDelete("{matchId}")]	

		public async Task<IActionResult> Delete([FromRoute] Guid matchId)
		{
           await itemMatchRepository.DeleteAsync(matchId);	
			return NoContent();
		
		}
	}
}
