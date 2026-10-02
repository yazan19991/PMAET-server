using Microsoft.AspNetCore.Mvc;
using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;
using PMAET_server.Services.ShoppingList;

namespace PMAET_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListController;
        public ShoppingListController(IShoppingListService shoppingListController)
        {
            _shoppingListController = shoppingListController;
        }

        // POST: /api/ShoppingList
        [HttpPost("")]
        public async Task<ActionResult<Response<ShoppingListItemResponseDto>>> NewShoppingList([FromBody] CreateShoppingListItemDto request)
        {
            var response = await _shoppingListController.CreateItemAsync(request);
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

        // Delete: /api/ShoppingList/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteItem(int Id)
        {
            var response = await _shoppingListController.DeleteItemAsync(Id);
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

        // Get: /api/ShoppingList/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<Response<List<ShoppingListItemResponseDto>>>> GetItemsByUser(int userId)
        {
            var response = await _shoppingListController.GetItemsByUserAsync(userId);
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

        // POST: /api/ShoppingList/{id}/MarkPurchased
        [HttpPost("{id}/MarkPurchased")]
        public async Task<ActionResult<Response<ShoppingListItemResponseDto>>> MarkPurchased(int id)
        {
            var response = await _shoppingListController.MarkPurchasedAsync(id);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<ShoppingListItemResponseDto>>> UpdateItem(int id, [FromBody] UpdateShoppingListItemDto request)
        {
            var response = await _shoppingListController.UpdateItemAsync(id, request);
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