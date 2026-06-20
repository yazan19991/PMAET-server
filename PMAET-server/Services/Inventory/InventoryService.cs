using Microsoft.EntityFrameworkCore;
using PMAET_server.Data;
using PMAET_server.Models;
using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;

namespace PMAET_server.Services.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<InventoryResponseDto>>> GetAllInventoriesAsync()
        {
            try
            {
                var inventories = await _context.Inventories
                    .OrderByDescending(i => i.CreatedAt)
                    .ToListAsync();
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
                return new Response<List<InventoryResponseDto>>(dtoList, new ResponseMessage(1, "Success"));
            }
            catch(Exception)
            {
              return new Response<List<InventoryResponseDto>>(null, new ResponseMessage(0,"Failed to get inventories",ResponseCodes.ServerError));
            }
        }

        public async Task<Response<InventoryResponseDto>> CreateInventoryAsync(CreateInventoryDto request)
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

                return new Response<InventoryResponseDto>(responseDto,new ResponseMessage(1,"Inventory created successfully"));


            }
            catch(Exception)
            {
               return new Response<InventoryResponseDto>(null, new ResponseMessage(0, "Failed to create inventory",ResponseCodes.ServerError));
            }
        }

        public async Task<Response<InventoryResponseDto>> UpdateInventoryAsync(int id, InventoryUpdateDto request)
        {
            try
            {
                var inventory  = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    return new Response<InventoryResponseDto>(null,new ResponseMessage(0,"Inventory ID does not exist",ResponseCodes.NotFound));
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

                
                return new Response<InventoryResponseDto>(responseDto,new ResponseMessage(1,"Inventory Updated successfully"));


            }
            catch(Exception)
            {
                return new Response<InventoryResponseDto>(null, new ResponseMessage(0, "Failed to Update inventory",ResponseCodes.ServerError));
            }
        }

        public async Task<Response<bool>> DeleteInventoryAsync(int id)
        {
            try
            {


                var inventory  = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    return new Response<bool>(false,new ResponseMessage(0,"Inventory ID does not exist.", ResponseCodes.NotFound));
                }

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();

                return new Response<bool>(true,new ResponseMessage(1,"Inventory Deleted successfully"));

            }
            catch(Exception)
            {
               return new Response<bool>(false, new ResponseMessage(0, "Failed to Delete inventory",ResponseCodes.ServerError));
            }
        }
    }
}