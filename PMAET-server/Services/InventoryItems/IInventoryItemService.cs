using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;

namespace PMAET_server.Services.InventoryItems
{
    public interface IInventoryItemService
    {
        Task<Response<ItemResponseDto>> AddItemAsync(CreateItemDto request);
        Task<Response<List<ItemResponseDto>>> GetItemsByInventoryAsync(int inventoryId);
        Task<Response<ItemResponseDto>> UpdateItemAsync(int id, UpdateItemDto request);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<List<ItemResponseDto>>> GetExpiredItemsAsync();
        Task<Response<List<ItemResponseDto>>> GetExpiringSoonItemsAsync(int days);
        Task<Response<decimal>> ConsumeItemAsync(int id, ConsumeItemDto request);
        Task<Response<decimal>> PurchaseItemAsync(int id, PurchaseItemDto request);
    }
}