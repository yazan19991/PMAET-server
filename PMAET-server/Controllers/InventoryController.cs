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
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventory/GetAll
        [HttpGet("GetAll")]
        // Return type is now wrapped in Response<T>
        public async Task<ActionResult<Response<List<InventoryResponseDto>>>> GetAllInventories()
        {
            try
            {
                var inventories = await _context.Inventories
                    .OrderByDescending(i => i.CreatedAt)
                    .ToListAsync();

                // Map to DTO
                var dtoList = new List<InventoryResponseDto>();
                foreach (var inv in inventories)
                {
                    var itemCount = await _context.InventoryItems
                        .CountAsync(item => item.InventoryId == inv.Id);

                    dtoList.Add(new InventoryResponseDto
                    {
                        Id = inv.Id,
                        Name = inv.Name,
                        Description = inv.Description,
                        CreatedAt = inv.CreatedAt,
                        ItemCount = itemCount
                    });
                }

                // WRAP IT in your custom Response object
                var response = new Response<List<InventoryResponseDto>>(dtoList, new ResponseMessage(1, "Success"));

                return Ok(response);
            }
            catch (Exception)
            {
                // Return a clean error response instead of crashing
                return BadRequest(new Response<List<InventoryResponseDto>>(null, new ResponseMessage(0,"Failed to get inventories")));
            }
        }

        // POST: api/Inventory/Create
        [HttpPost("Create")]
        public async Task<ActionResult<Response<InventoryResponseDto>>> CreateInventory([FromBody] CreateInventoryDto request)
        {
            try
            {
                var newInventory = new Inventory
                {
                    Name = request.Name,
                    Description = request.Description,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Inventories.Add(newInventory);
                await _context.SaveChangesAsync();

                var responseDto = new InventoryResponseDto
                {
                    Id = newInventory.Id,
                    Name = newInventory.Name,
                    Description = newInventory.Description,
                    CreatedAt = newInventory.CreatedAt
                };

                // WRAP IT
                var response = new Response<InventoryResponseDto>(responseDto,new ResponseMessage(1,"Inventory created successfully"));

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new Response<InventoryResponseDto>(null, new ResponseMessage(0, "Failed to create inventory")));
            }
        }


        // PUT: PUT api/Inventory/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Response<InventoryResponseDto>>> UpdateInventory(int id,[FromBody] InventoryUpdateDto request)
        {
            try   
            {

                var inventory  = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    return NotFound(new Response<InventoryResponseDto>(null,new ResponseMessage(0,"Inventory ID does not exist")));
                }

                inventory .Name = request.Name;
                inventory .Description = request.Description;
                await _context.SaveChangesAsync();

                var responseDto = new InventoryResponseDto
                {
                    Id = inventory .Id,
                    Name =  inventory .Name,
                    Description = inventory .Description,
                    CreatedAt = inventory .CreatedAt
                };

                
                var response = new Response<InventoryResponseDto>(responseDto,new ResponseMessage(1,"Inventory Updated successfully"));

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new Response<InventoryResponseDto>(null, new ResponseMessage(0, "Failed to Update inventory")));
            }
        }

        // Delete: Delete api/Inventory/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteInventory(int id)
        {
            try   
            {

                var inventory  = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    return NotFound(new Response<bool>(false,new ResponseMessage(0,"Inventory ID does not exist.")));
                }

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();

                var response = new Response<bool>(true,new ResponseMessage(1,"Inventory Deleted successfully"));

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new Response<bool>(false, new ResponseMessage(0, "Failed to Delete inventory")));
            }
        }


    }
}