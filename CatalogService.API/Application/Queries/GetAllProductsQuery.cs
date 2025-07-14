using CatalogService.API.Application.DTOs;
using MediatR;
namespace CatalogService.API.Application.Queries;
public class GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;