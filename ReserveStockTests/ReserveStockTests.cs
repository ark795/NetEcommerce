using InventoryService.API.Domain.Entities;
using InventoryService.API.Infrastructure.Persistence;
using InventoryService.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace ReserveStockTests;

public class ReserveStockTests
{
    private readonly ApplicationDbContext _context;
    private readonly InventoryServiceImpl _inventoryService;

    public ReserveStockTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _context = new ApplicationDbContext(options);

        _context.Inventories.Add(new ProductInventory
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            AvailableStock = 10
        });
        _context.SaveChanges();

        _inventoryService = new InventoryServiceImpl(_context);
    }

    [Fact]
    public async Task ReserveStock_Should_Succeed_If_EnoughStock()
    {
        var result = await _inventoryService.ReserveStockAsync(new List<(Guid, int)>
        {
            (Guid.Parse("11111111-1111-1111-1111-111111111111"), 5)
        });

        result.ShouldBeTrue();
        _context.Inventories.First().AvailableStock.ShouldBe(5);
    }

    [Fact]
    public async Task ReserveStock_Should_Fail_If_NotEnoughStock()
    {
        var result = await _inventoryService.ReserveStockAsync(new List<(Guid, int)>
        {
            (Guid.Parse("11111111-1111-1111-1111-111111111111"), 20)
        });

        result.ShouldBeFalse();
        _context.Inventories.First().AvailableStock.ShouldBe(10);
    }
}
