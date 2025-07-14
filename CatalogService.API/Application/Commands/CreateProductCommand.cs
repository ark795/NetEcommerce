using CatalogService.API.Application.DTOs;
using MediatR;
namespace CatalogService.API.Application.Commands;
public record CreateProductCommand(string Name, string Description, decimal Price, int Stock)
: IRequest<ProductDto>;


