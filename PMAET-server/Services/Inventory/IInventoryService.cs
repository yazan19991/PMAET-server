using PMAET_server.Models.DTOs;
using PMAET_server.Models.Responses;

namespace PMAET_server.Services.Inventories
{
    public interface IInventoryService
    {
        Task<Response<List<InventoryResponseDto>>> GetAllInventoriesAsync();
        Task<Response<InventoryResponseDto>> CreateInventoryAsync(CreateInventoryDto request);
        Task<Response<InventoryResponseDto>> UpdateInventoryAsync(int id, InventoryUpdateDto request);
        Task<Response<bool>> DeleteInventoryAsync(int id);
    }
}