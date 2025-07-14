using CatalogService.API.Application.DTOs;
using MediatR;
namespace CatalogService.API.Application.Queries;
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;