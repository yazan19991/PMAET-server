using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;

namespace PMAET_server.Services.ShoppingList
{



    public interface IShoppingListService
    {
        Task<Response<ShoppingListItemResponseDto>> CreateItemAsync(CreateShoppingListItemDto request);
        Task<Response<List<ShoppingListItemResponseDto>>> GetItemsByUserAsync(int userId);
        Task<Response<ShoppingListItemResponseDto>> UpdateItemAsync(int id, UpdateShoppingListItemDto request);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<ShoppingListItemResponseDto>> MarkPurchasedAsync(int id);
    }

}