using CatalogService.API.Application.Commands;
using CatalogService.API.Contracts.Events;
using CatalogService.API.Infrastructure.Data;
using MassTransit;
using MediatR;
namespace CatalogService.API.Application.Handlers;
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly CatalogDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;
    public DeleteProductHandler(CatalogDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _db.Products.FindAsync(new object[] { request.Id }, cancellationToken);
        if (product == null) throw new Exception("Product not found");
        _db.Products.Remove(product);
        await _db.SaveChangesAsync(cancellationToken);
        var @event = new ProductDeletedEvent { Id = product.Id };
        await _publishEndpoint.Publish(@event, cancellationToken);
        return Unit.Value;
    }
}