using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
                    return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Inventory ID does not exist.", ResponseCodes.NotFound));
                }

                var newItem = new InventoryItem
                {
                    InventoryId = request.InventoryId,
                    Name = request.Name,
                    Quantity = request.Quantity,
                    Unit = request.Unit,
                    ExpiryDate = request.ExpiryDate,
                    NeverExpires = request.NeverExpires,
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
                    NeverExpires = request.NeverExpires,
                    PurchaseDate = newItem.PurchaseDate
                };

                return new Response<ItemResponseDto>(responseDto, new ResponseMessage(1, "Item added successfully"));
            }
            catch (Exception)
            {
                return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Failed to add Item", ResponseCodes.ServerError));
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
                    NeverExpires = i.NeverExpires,
                    PurchaseDate = i.PurchaseDate
                }).ToList();

                return new Response<List<ItemResponseDto>>(dtoList, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<List<ItemResponseDto>>(null, new ResponseMessage(0, "Failed to get Items", ResponseCodes.ServerError));
            }
        }
        public async Task<Response<ItemResponseDto>> UpdateItemAsync(int id, UpdateItemDto request)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                var inventoryExists = await _context.Inventories.AnyAsync(i => i.Id == request.InventoryId);
                if (!inventoryExists)
                {
                    return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Inventory ID does not exist.", ResponseCodes.NotFound));
                }

                item.InventoryId = request.InventoryId;
                item.Name = request.Name;
                item.Quantity = request.Quantity;
                item.Unit = request.Unit;
                item.ExpiryDate = request.ExpiryDate;
                item.NeverExpires = request.NeverExpires;


                await _context.SaveChangesAsync();

                var responseDto = new ItemResponseDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    ExpiryDate = item.ExpiryDate,
                    NeverExpires = item.NeverExpires,
                    PurchaseDate = item.PurchaseDate
                };
                return new Response<ItemResponseDto>(responseDto, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<ItemResponseDto>(null, new ResponseMessage(0, "Failed to Update Item", ResponseCodes.ServerError));
            }


        }
        public async Task<Response<bool>> DeleteItemAsync(int id)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                _context.InventoryItems.Remove(item);
                await _context.SaveChangesAsync();

                return new Response<bool>(true, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<bool>(false, new ResponseMessage(0, "Failed to Delete Item", ResponseCodes.ServerError));
            }

        }

        public async Task<Response<List<ItemResponseDto>>> GetExpiredItemsAsync()
        {
            try
            {
                var items = await _context.InventoryItems
                    .Where(i => i.ExpiryDate.HasValue && !i.NeverExpires && i.ExpiryDate.Value < DateTime.UtcNow)
                    .OrderBy(i => i.ExpiryDate)
                    .ToListAsync();
                var dtoList = items.Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    ExpiryDate = i.ExpiryDate,
                    NeverExpires = i.NeverExpires,
                    PurchaseDate = i.PurchaseDate
                }).ToList();
                return new Response<List<ItemResponseDto>>(dtoList, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<List<ItemResponseDto>>(null, new ResponseMessage(0, "Failed to get expired items", ResponseCodes.ServerError));
            }
        }


        public async Task<Response<List<ItemResponseDto>>> GetExpiringSoonItemsAsync(int days)
        {
            try
            {
                if (days <= 0)
                {
                    days = 7;
                }
                var now = DateTime.UtcNow;
                var limitDate = now.AddDays(days);

                var items = await _context.InventoryItems
                    .Where(i => i.ExpiryDate.HasValue &&
                                !i.NeverExpires &&
                                i.ExpiryDate.Value >= now &&
                                i.ExpiryDate.Value <= limitDate)
                    .OrderBy(i => i.ExpiryDate)
                    .ToListAsync();
                var dtoList = items.Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    ExpiryDate = i.ExpiryDate,
                    NeverExpires = i.NeverExpires,
                    PurchaseDate = i.PurchaseDate
                }).ToList();
                return new Response<List<ItemResponseDto>>(dtoList, new ResponseMessage(1, "Success"));
            }
            catch (Exception)
            {
                return new Response<List<ItemResponseDto>>(null, new ResponseMessage(0, "Failed to get expiring soon items", ResponseCodes.ServerError));
            }
        }
        public async Task<Response<decimal>> ConsumeItemAsync(int id, ConsumeItemDto request)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<decimal>(0, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                if (request.Amount <= 0)
                {
                    return new Response<decimal>(0, new ResponseMessage(0, "Amount must be greater than zero", ResponseCodes.ValidationError));
                }
                if (item.Quantity < request.Amount)
                {
                    return new Response<decimal>(0, new ResponseMessage(0, "Not enough stock to consume the requested amount", ResponseCodes.ValidationError));
                }
                item.Quantity -= request.Amount;
                var newTransaction = new StockTransaction
                {
                    InventoryItemId = item.Id,
                    Type = "Consume",
                    QuantityChanged = -request.Amount,
                    CreatedAt = DateTime.UtcNow
                };
                _context.StockTransactions.Add(newTransaction);
                await _context.SaveChangesAsync();
                return new Response<decimal>(item.Quantity, new ResponseMessage(1, "Item consumed successfully"));
            }
            catch (Exception)
            {

                return new Response<decimal>(0, new ResponseMessage(0, "Failed to consume item", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<decimal>> PurchaseItemAsync(int id, PurchaseItemDto request)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item == null)
                {
                    return new Response<decimal>(0, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                if (request.Amount <= 0)
                {
                    return new Response<decimal>(0, new ResponseMessage(0, "Amount must be greater than zero", ResponseCodes.ValidationError));
                }
                item.Quantity += request.Amount;

                var newTransaction = new StockTransaction
                {
                    InventoryItemId = item.Id,
                    Type = "Purchase",
                    QuantityChanged = request.Amount,
                    CreatedAt = DateTime.UtcNow
                };
                _context.StockTransactions.Add(newTransaction);
                await _context.SaveChangesAsync();
                return new Response<decimal>(item.Quantity, new ResponseMessage(1, "Item purchased successfully"));
            }
            catch (Exception)
            {

                return new Response<decimal>(0, new ResponseMessage(0, "Failed to purchase item", ResponseCodes.ServerError));
            }
        }
        public async Task<Response<List<StockTransactionResponseDto>>> GetItemTransactionsAsync(int itemId)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(itemId);
                if (item == null)
                {
                    return new Response<List<StockTransactionResponseDto>>(null, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                var items = await _context.StockTransactions
                    .Where(i => i.InventoryItemId == itemId)
                    .OrderByDescending(i => i.CreatedAt)
                    .ToListAsync();
                var dtoList = items.Select(i => new StockTransactionResponseDto
                {
                    Id = i.Id,
                    InventoryItemId = i.InventoryItemId,
                    Type = i.Type,
                    QuantityChanged = i.QuantityChanged,
                    CreatedAt = i.CreatedAt,
                }).ToList();
                return new Response<List<StockTransactionResponseDto>>(dtoList, new ResponseMessage(1, "Transactions retrieved successfully"));
            }
            catch (Exception)
            {
                return new Response<List<StockTransactionResponseDto>>(null, new ResponseMessage(0, "Failed to retrieve transactions", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<bool>> CreateAndAddLabelAsync(CreateAndAddLabelDto request, int itemId)
        {
            try
            {
                var labelName = request.Name.Trim();
                if (string.IsNullOrWhiteSpace(labelName))
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Label name is required", ResponseCodes.ValidationError));
                }
                var label = await _context.Labels 
                    .FirstOrDefaultAsync(l => l.Name.ToLower() == labelName.ToLower());
                if (label == null)
                {
                    var newLabel = new Label
                    {
                        Name = labelName,
                    };
                    _context.Labels.Add(newLabel);
                    await _context.SaveChangesAsync();
                    label = newLabel;
                }
                var item = await _context.InventoryItems
                    .Include(i => i.Labels )
                    .SingleOrDefaultAsync(i => i.Id == itemId);
                if (item == null)
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }
                if (item.Labels!.Any(l => l.Name == label.Name))
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Label already assigned to this item",ResponseCodes.ValidationError));
                }
                item.Labels.Add(label);
                await _context.SaveChangesAsync();
                return new Response<bool>(true, new ResponseMessage(1, "label added successfully"));
            }
            catch (Exception)
            {
                return new Response<bool>(false, new ResponseMessage(0, "Failed to add label", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<bool>> RemoveAssignedLabelAsync(int itemId,int labelId)
        {
            try
            {
                var item = await _context.InventoryItems
                    .Include(i => i.Labels)
                    .SingleOrDefaultAsync(i => i.Id == itemId);
                if (item == null)
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }

                var label = item.Labels!.SingleOrDefault(l => l.Id == labelId);
                if (label == null)
                {
                    return new Response<bool>(false, new ResponseMessage(0, "Label is not assigned to this item", ResponseCodes.NotFound));
                }

                item.Labels.Remove(label);
                await _context.SaveChangesAsync();

                return new Response<bool>(true, new ResponseMessage(1, "Label removed successfully"));
            }
            catch (Exception)
            {
                return new Response<bool>(false, new ResponseMessage(0, "Failed to remove label", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<List<LabelResponseDto>>> GetItemLabelsAsync(int itemId)
        {
            try
            {
                var item = await _context.InventoryItems
                    .AsNoTracking()
                    .Include(i => i.Labels)
                    .SingleOrDefaultAsync(i => i.Id == itemId);
                if (item == null)
                {
                    return new Response<List<LabelResponseDto>>(null, new ResponseMessage(0, "Item not found", ResponseCodes.NotFound));
                }

                var labels = item.Labels!
                    .Select(label => new LabelResponseDto
                    {
                        Id = label.Id,
                        Name = label.Name,
                    })
                    .ToList();

                return new Response<List<LabelResponseDto>>(labels, new ResponseMessage(1, "Labels retrieved successfully"));
            }
            catch (Exception)
            {
                return new Response<List<LabelResponseDto>>(null, new ResponseMessage(0, "Failed to retrieve labels", ResponseCodes.ServerError));
            }
        }

        public async Task<Response<List<ItemResponseDto>>> GetItemsByLabelAsync(GetItemByLabelDto request)
        {
            try
            {
                var items = await _context.InventoryItems
                    .AsNoTracking()
                    .Where(item => item.Inventory != null
                        && item.Inventory.UserId == request.Userid
                        && item.Labels!.Any(label => label.Id == request.LabelId))
                    .Select(item => new ItemResponseDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Unit = item.Unit,
                        ExpiryDate = item.ExpiryDate,
                        PurchaseDate = item.PurchaseDate,
                        NeverExpires = item.NeverExpires
                    })
                    .ToListAsync();

                return new Response<List<ItemResponseDto>>(items, new ResponseMessage(1, "Items retrieved successfully"));
            }
            catch (Exception)
            {
                return new Response<List<ItemResponseDto>>(null, new ResponseMessage(0, "Failed to retrieve items", ResponseCodes.ServerError));
            }
        }
    }

}
