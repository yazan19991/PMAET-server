using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMAET_server.Data;
using PMAET_server.Models;
using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;

namespace PMAET_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryItemController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/InventoryItem/Add
        [HttpPost("Add")]
        public async Task<ActionResult<Response<ItemResponseDto>>> AddItem([FromBody] CreateItemDto request)
        {
            try
            {
                var inventoryExists = await _context.Inventories.AnyAsync(i => i.Id == request.InventoryId);
                if (!inventoryExists)
                {
                    return BadRequest(new Response<ItemResponseDto>(null,new ResponseMessage(0,"Inventory ID does not exist.")));
                }

                var newItem = new InventoryItem
                {
                    InventoryId = request.InventoryId,
                    Name = request.Name,
                    Quantity = request.Quantity,
                    Unit = request.Unit,
                    ExpiryDate = request.ExpiryDate,
                    PurchaseDate = DateTime.UtcNow
                };

                _context.InventoryItems.Add(newItem);
                await _context.SaveChangesAsync();

                var responseDto = new ItemResponseDto
                {
                    Id = newItem.Id,
                    Name = newItem.Name,
                    Quantity = newItem.Quantity,
                    Unit = newItem.Unit,
                    ExpiryDate = newItem.ExpiryDate,
                    PurchaseDate = newItem.PurchaseDate
                };

                return Ok(new Response<ItemResponseDto>(responseDto,new ResponseMessage(1,"Item added successfully")));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<ItemResponseDto>(null,new ResponseMessage(0,ex.Message)));
            }
        }

        // GET: api/InventoryItem/GetByInventory/
        [HttpGet("GetByInventory/{inventoryId}")]
        public async Task<ActionResult<Response<List<ItemResponseDto>>>> GetItems(int inventoryId)
        {
            try
            {
                var items = await _context.InventoryItems
                    .Where(i => i.InventoryId == inventoryId) // Filter by Pantry
                    .OrderBy(i => i.ExpiryDate) // Show expiring soon first!
                    .ToListAsync();

                var dtoList = items.Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    ExpiryDate = i.ExpiryDate,
                    PurchaseDate = i.PurchaseDate
                }).ToList();

                return Ok(new Response<List<ItemResponseDto>>(dtoList,new ResponseMessage(1,"Success")));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<List<ItemResponseDto>>(null,new ResponseMessage(0,ex.Message)));
            }
        }
    }
}