using CatalogService.API.Application.DTOs;
using MediatR;
namespace CatalogService.API.Application.Commands;
public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, int Stock)
: IRequest<ProductDto>;