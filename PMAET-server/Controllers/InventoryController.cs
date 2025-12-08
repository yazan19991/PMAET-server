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
                var dtoList = inventories.Select(inv => new InventoryResponseDto
                {
                    Id = inv.Id,
                    Name = inv.Name,
                    Description = inv.Description,
                    CreatedAt = inv.CreatedAt,
                    ItemCount = 0
                }).ToList();

                // WRAP IT in your custom Response object
                var response = new Response<List<InventoryResponseDto>>(dtoList, new ResponseMessage(1, "Success"));

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return a clean error response instead of crashing
                return BadRequest(new Response<List<InventoryResponseDto>>(null, new ResponseMessage(0, ex.Message)));
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
            catch (Exception ex)
            {
                return BadRequest(new Response<InventoryResponseDto>(null, new ResponseMessage(0, "Failed to create inventory: " + ex.Message)));
            }
        }
    }
}