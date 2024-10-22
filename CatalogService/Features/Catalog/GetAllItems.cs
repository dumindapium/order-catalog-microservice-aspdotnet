using Carter;
using CatalogService.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Catalog;

public static class GetAllItems
{
    public record Query():IRequest<IEnumerable<Response>>;
    public record Response(Guid Id, string Name, string Description, int Inventory);

    internal class Handler(CatalogDbContext _dbContext) : IRequestHandler<Query, IEnumerable<Response>>
    {
        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var items = _dbContext.Items;

            var result = items.Select(i =>
                            new Response(
                                i.Id,
                                i.Name,
                                i.Description,
                                i.Inventory
                                ));
            return await result.ToListAsync(cancellationToken);
        }
    }

    public class GetAllItemsEndpoint : CarterModule
    {
        public GetAllItemsEndpoint() : base() { }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/getall", async (ISender sender) =>
            {
                var result = await sender.Send(new Query());

                return Results.Ok(result);

            }).WithOpenApi();
        }
    }
}
