using Carter;
using CatalogService.Database;
using CatalogService.Entities;
using MediatR;

namespace CatalogService.Features.Catalog;

public static class CreateItem
{
    public record Command(string Name,
                          string Description
                          ) : IRequest<Guid>;

    public class Handler(CatalogDbContext _dbContext) : IRequestHandler<Command, Guid>
    {
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = new Item() { Name = request.Name, Description = request.Description, Inventory = 0 };
            _dbContext.Set<Item>().Add(item);
            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
    }

    public class CreateItemEndpoint : CarterModule
    {
        public CreateItemEndpoint() {}

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/create", async (Command command, ISender sender) =>
            {
                var result = await sender.Send(command);

                return Results.Ok(result);

            }).WithOpenApi();
        }
    }
}
