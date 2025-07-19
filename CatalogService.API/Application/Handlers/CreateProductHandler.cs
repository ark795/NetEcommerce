using CatalogService.API.Application.Commands;
using CatalogService.API.Application.DTOs;
using CatalogService.API.Contracts.Events;
using CatalogService.API.Domain.Entities;
using CatalogService.API.Infrastructure.Data;
using MassTransit;
using MediatR;
namespace CatalogService.API.Application.Handlers;
public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly CatalogDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;
    public CreateProductHandler(CatalogDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.Description, request.Price, request.StockQuantity, request.ImageUrl, request.Brand, request.Category);
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        var @event = new ProductCreatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
        };
        await _publishEndpoint.Publish(@event, cancellationToken);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.StockQuantity,
        };
    }
}