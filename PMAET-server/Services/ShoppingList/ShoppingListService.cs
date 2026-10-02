using Microsoft.EntityFrameworkCore;
using PMAET_server.Data;
using PMAET_server.Models;
using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;


namespace PMAET_server.Services.ShoppingList
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly AppDbContext _context;

        public ShoppingListService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Response<ShoppingListItemResponseDto>> CreateItemAsync(CreateShoppingListItemDto request)
        {
            try
            {
                var newShoppingListItem = new ShoppingListItem
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    Quantity = request.Quantity,
                    Unit = request.Unit,
                };

                _context.ShoppingListItems.Add(newShoppingListItem);

                await _context.SaveChangesAsync();

                var responseDto = new ShoppingListItemResponseDto
                {
                    Id = newShoppingListItem.Id,
                    UserId = newShoppingListItem.UserId,
                    Name = newShoppingListItem.Name,
                    Quantity = newShoppingListItem.Quantity,
                    Unit = newShoppingListItem.Unit,
                    IsPurchased = newShoppingListItem.IsPurchased,
                    CreatedAt = newShoppingListItem.CreatedAt,
                    PurchasedAt = newShoppingListItem.PurchasedAt
                };
                return new Response<ShoppingListItemResponseDto>(responseDto, new ResponseMessage(1, "Item added to shopping list successfully"));
            }
            catch (Exception)
            {
                return new Response<ShoppingListItemResponseDto>(null, new ResponseMessage(0, "Failed to add Item to shopping list", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<bool>> DeleteItemAsync(int id)
        {
            try
            {
                var item = await _context.ShoppingListItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                _context.ShoppingListItems.Remove(item);
                await _context.SaveChangesAsync();

                return new Response<bool>(true, new ResponseMessage(1, "Item deleted from shopping list successfully"));
            }
            catch (Exception)
            {
                return new Response<bool>(false, new ResponseMessage(0, "Failed to delete Item from shopping list", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<List<ShoppingListItemResponseDto>>> GetItemsByUserAsync(int userId)
        {
            try
            {
                var items = await _context.ShoppingListItems
                .AsNoTracking()
                .Where(i => i.UserId == userId)
                .OrderBy(i => i.CreatedAt) 
                .ToListAsync();

                var dtoList = new List<ShoppingListItemResponseDto>();
                dtoList.AddRange(items.Select(i => new ShoppingListItemResponseDto
                {
                    Id = i.Id,
                    UserId = i.UserId,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    IsPurchased = i.IsPurchased,
                    CreatedAt = i.CreatedAt,
                    PurchasedAt = i.PurchasedAt
                }));
                return new Response<List<ShoppingListItemResponseDto>>(dtoList, new ResponseMessage(1, "Get all Item in user shopping list done successfully"));
            }
            catch (Exception)
            {
                return new Response<List<ShoppingListItemResponseDto>>(null, new ResponseMessage(0, "Failed to get all Item in user shopping list", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<ShoppingListItemResponseDto>> MarkPurchasedAsync(int id)
        {
            try
            {
                var item = await _context.ShoppingListItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<ShoppingListItemResponseDto>(null, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                item.IsPurchased = true;
                item.PurchasedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var responseDto = new ShoppingListItemResponseDto
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    IsPurchased = item.IsPurchased,
                    CreatedAt = item.CreatedAt,
                    PurchasedAt = item.PurchasedAt
                };
                return new Response<ShoppingListItemResponseDto>(responseDto, new ResponseMessage(1, "Item mark as purchased successfully"));
            }
            catch (Exception)
            {
                return new Response<ShoppingListItemResponseDto>(null, new ResponseMessage(0, "Failed to mark as purchased", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<ShoppingListItemResponseDto>> UpdateItemAsync(int id, UpdateShoppingListItemDto request)
        {
            try
            {
                var item = await _context.ShoppingListItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<ShoppingListItemResponseDto>(null, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                item.Name = request.Name;
                item.Quantity = request.Quantity;
                item.Unit = request.Unit;
                await _context.SaveChangesAsync();
                var responseDto = new ShoppingListItemResponseDto
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    IsPurchased = item.IsPurchased,
                    CreatedAt = item.CreatedAt,
                    PurchasedAt = item.PurchasedAt
                };
                return new Response<ShoppingListItemResponseDto>(responseDto, new ResponseMessage(1, "Item update successfully"));
            }
            catch (Exception)
            {
                return new Response<ShoppingListItemResponseDto>(null, new ResponseMessage(0, "Failed to update item", ResponseCodes.ServerError));
            }
        }
    }
}