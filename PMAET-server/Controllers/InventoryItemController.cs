using Microsoft.AspNetCore.Mvc;
using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;
using PMAET_server.Services.InventoryItems;

namespace PMAET_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;
        public InventoryItemController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }

        // POST: api/InventoryItem/Add
        [HttpPost("Add")]
        public async Task<ActionResult<Response<ItemResponseDto>>> AddItem([FromBody] CreateItemDto request)
        {
            var response = await _inventoryItemService.AddItemAsync(request);
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

        // GET: api/InventoryItem/GetByInventory/
        [HttpGet("GetByInventory/{inventoryId}")]
        public async Task<ActionResult<Response<List<ItemResponseDto>>>> GetItems(int inventoryId)
        {
            var response = await _inventoryItemService.GetItemsByInventoryAsync(inventoryId);
            if (response.Message?.Status == 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // GET: api/InventoryItem/Expired/
        [HttpGet("Expired")]
        public async Task<ActionResult<Response<List<ItemResponseDto>>>> GetExpiredItems()
        {
            var response = await _inventoryItemService.GetExpiredItemsAsync();
            if (response.Message?.Status == 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


                // GET: api/InventoryItem/ExpiringSoon?days=7
        [HttpGet("ExpiringSoon")]
        public async Task<ActionResult<Response<List<ItemResponseDto>>>> GetExpiringSoonItems([FromQuery] int days = 7)
        {
            var response = await _inventoryItemService.GetExpiringSoonItemsAsync(days);
            if (response.Message?.Status == 0)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }




        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Response<ItemResponseDto>>> UpdateItem(int id, [FromBody] UpdateItemDto request)
        {
            var response = await _inventoryItemService.UpdateItemAsync(id, request);
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

        [HttpPost("Consume/{id}")]
        public async Task<ActionResult<Response<decimal>>> ConsumeItem(int id, [FromBody] ConsumeItemDto request)
        {
            var response = await _inventoryItemService.ConsumeItemAsync(id,request);
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

        [HttpPost("Purchase/{id}")]
        public async Task<ActionResult<Response<decimal>>> PurchaseItem(int id, [FromBody] PurchaseItemDto request)
        {
            var response = await _inventoryItemService.PurchaseItemAsync(id,request);
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

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteItem(int id)
        {
            var response = await _inventoryItemService.DeleteItemAsync(id);
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
        //GET /api/InventoryItem/{id}/Transactions
        [HttpGet("{id}/Transactions")]
        public async Task<ActionResult<Response<List<StockTransactionResponseDto>>>> GetItemTransactions(int id)
        {
            var response = await _inventoryItemService.GetItemTransactionsAsync(id);
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