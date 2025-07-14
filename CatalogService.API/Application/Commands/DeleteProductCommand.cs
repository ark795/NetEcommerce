using MediatR;
namespace CatalogService.API.Application.Commands;
public record DeleteProductCommand(Guid Id) : IRequest<Unit>; 
