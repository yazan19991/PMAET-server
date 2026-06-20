using Microsoft.AspNetCore.Mvc;
using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;
using PMAET_server.Services.Inventories;
namespace PMAET_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        // GET: api/Inventory/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<Response<List<InventoryResponseDto>>>> GetAllInventories()
        {
            var response = await _inventoryService.GetAllInventoriesAsync();
            if (response.Message?.Status == 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        // POST: api/Inventory/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Response<InventoryResponseDto>>> CreateInventory([FromBody] CreateInventoryDto request)
        {
            var response = await _inventoryService.CreateInventoryAsync(request);
            if (response.Message?.Status == 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        // PUT: PUT api/Inventory/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Response<InventoryResponseDto>>> UpdateInventory(int id, [FromBody] InventoryUpdateDto request)
        {
            var response = await _inventoryService.UpdateInventoryAsync(id, request);
            if (response.Message?.Code == ResponseCodes.NotFound)
            {
                return NotFound(response);
            }
            if (response.Message?.Status == 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        // Delete: Delete api/Inventory/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteInventory(int id)
        {
            var response = await _inventoryService.DeleteInventoryAsync(id);
            if (response.Message?.Code == ResponseCodes.NotFound)
            {
                return NotFound(response);
            }
            if (response.Message?.Status == 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}