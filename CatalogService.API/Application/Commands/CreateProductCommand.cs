using CatalogService.API.Application.DTOs;
using MediatR;
namespace CatalogService.API.Application.Commands;
public record CreateProductCommand(string Name, string Description, decimal Price, int StockQuantity, string ImageUrl, string Brand, string Category) : IRequest<ProductDto>;


