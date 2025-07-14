using CatalogService.API.Application.DTOs;
using CatalogService.API.Application.Queries;
using CatalogService.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace CatalogService.API.Application.Handlers;
public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly CatalogDbContext _db;
    public GetAllProductsHandler(CatalogDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await _db.Products
        .Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
        }).ToListAsync(cancellationToken);
    }
}