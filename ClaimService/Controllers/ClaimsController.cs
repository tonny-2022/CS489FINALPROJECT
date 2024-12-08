using AutoMapper;
using ClaimService.dto;
using ClaimService.model;
using ClaimService.repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClaimService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClaimsController : ControllerBase
	{
		private readonly IClaimRepository _claimRepository;
		private readonly IMapper _mapper;

        public ClaimsController(IMapper mapper, IClaimRepository claimRepository)
        {
            _mapper = mapper;
			_claimRepository = claimRepository;	
        }

        [HttpGet]
		public async Task<IActionResult> GetAll() {
			var claims = await _claimRepository.GetAllClaimsAysnc();
			var claimDtos = _mapper.Map<List<ItemClaimResponseDTO>>(claims);
			return Ok(claimDtos);
		   
		}
		[HttpGet]
		[Route("claimid:Guid")]
		public async Task<IActionResult> GetById(Guid claimid)
		{
			var claim=await _claimRepository.GetClaimByIdAsync(claimid);
			if (claim == null)
			{
				return NotFound();
			}
			var claimDto=_mapper.Map<ItemClaimResponseDTO>(claim);
			return Ok(claimDto);
		}

		[HttpPost]	
		public async Task<IActionResult> Create([FromBody] ItemClaimRequestDTO itemClaimRequestDTO)
		{

			//itemmatchid:8366084d-a9e9-460a-8e85-080c10111aff
			var claim = _mapper.Map<ItemClaim>(itemClaimRequestDTO);
			await _claimRepository.CreateClaimAsync(claim);	
			return Ok(_mapper.Map<ItemClaimResponseDTO>(claim));

		}
		[HttpPut]
		public async Task<IActionResult> Update(Guid claimid,ItemClaimRequestDTO claimRequestDTO)

		{
			var claim=_mapper.Map<ItemClaim>(claimRequestDTO);
			var claimToUpdate = await _claimRepository.UpdateClaimAsync(claimid,claim);
			if (claimToUpdate == null)
			{
				return NotFound();
			}
			
			return Ok(_mapper.Map<ItemClaimResponseDTO>(claim));
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			var claim=await _claimRepository.DeleteClaimAsync(id);
			if (claim == null) { 
			  return NotFound();	
			}

			return Ok(_mapper.Map<ItemClaimResponseDTO>(claim));

		}
			
	}
}
