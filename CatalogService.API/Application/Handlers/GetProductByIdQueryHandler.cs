using CatalogService.API.Application.DTOs;
using CatalogService.API.Application.Queries;
using CatalogService.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace CatalogService.API.Application.Handlers;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly CatalogDbContext _db;
    public GetProductByIdQueryHandler(CatalogDbContext db)
    {
        _db = db;
    }
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (product == null) return null;
        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
        return dto;
    }
}