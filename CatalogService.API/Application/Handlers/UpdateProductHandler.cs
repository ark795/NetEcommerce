using CatalogService.API.Application.Commands;
using CatalogService.API.Application.DTOs;
using CatalogService.API.Contracts.Events;
using CatalogService.API.Infrastructure.Data;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace CatalogService.API.Application.Handlers;
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly CatalogDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;
    public UpdateProductHandler(CatalogDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product == null) throw new Exception("Product Not Found");
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        await _db.SaveChangesAsync(cancellationToken);
        var @event = new ProductUpdatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
        await _publishEndpoint.Publish(@event, cancellationToken);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
        };
    }
}