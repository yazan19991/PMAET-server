using Microsoft.EntityFrameworkCore;
using PMAET_server.Data;
using PMAET_server.Models;
using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;


namespace PMAET_server.Services.InventoryItems
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly AppDbContext _context;

        public InventoryItemService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Response<ItemResponseDto>> AddItemAsync(CreateItemDto request)

        {
            try
            {
                var inventoryExists = await _context.Inventories.AnyAsync(i => i.Id == request.InventoryId);
                if (!inventoryExists)
                {
                    return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Inventory ID does not exist."));
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

                return new Response<ItemResponseDto>(responseDto, new ResponseMessage(1, "Item added successfully"));
            }
            catch (Exception)
            {
                return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Failed to add Item"));
            }
        }


        public async Task<Response<List<ItemResponseDto>>> GetItemsByInventoryAsync(int inventoryId)
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

                return new Response<List<ItemResponseDto>>(dtoList, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<List<ItemResponseDto>>(null, new ResponseMessage(0, "Failed to get Items"));
            }
        }
        public async Task<Response<ItemResponseDto>> UpdateItemAsync(int id, UpdateItemDto request)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Item not found"));
                }
                var inventoryExists = await _context.Inventories.AnyAsync(i => i.Id == request.InventoryId);
                if (!inventoryExists)
                {
                    return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Inventory ID does not exist."));
                }

                item.InventoryId = request.InventoryId;
                item.Name = request.Name;
                item.Quantity = request.Quantity;
                item.Unit = request.Unit;
                item.ExpiryDate = request.ExpiryDate;


                await _context.SaveChangesAsync();

                var responseDto = new ItemResponseDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    ExpiryDate = item.ExpiryDate,
                    PurchaseDate = item.PurchaseDate
                };
                return new Response<ItemResponseDto>(responseDto, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Failed to Update Item"));
            }


        }
        public async Task<Response<bool>> DeleteItemAsync(int id)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Item not found"));
                }
                _context.InventoryItems.Remove(item);
                await _context.SaveChangesAsync();

                return new Response<bool>(true, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<bool>(false, new ResponseMessage(0, "Failed to Delete Item"));
            }

        }
    }

}