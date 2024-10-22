using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using OrderService.Entities;

namespace OrderService.Features.Order;

public static class CreateOrder
{
    public record Command(
        string CustomerName,
        string CustomerAddress,
        string CustomerCity,
        IEnumerable<Line> Lines
        ):IRequest<Guid>;
    public record Line(int SeqNo, Guid ItemId, decimal Price);


    public class Handler(OrderDbContext _dbContext) : IRequestHandler<Command, Guid>
    {
        //private readonly OrderDbContext _dbContext;
        //public Handler(OrderDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var order = new Entities.Order()
            {
                CustomerName = request.CustomerName,
                CustomerAddress = request.CustomerAddress,
                CustomerCity = request.CustomerCity,
                OrderLines = request.Lines.Select(l => new OrderLine() { Price = l.Price, ItemId = l.ItemId, SeqNo = l.SeqNo }).ToList()
            };

            _dbContext.Set<Entities.Order>().Add(order);
            await _dbContext.SaveChangesAsync();

            return order.Id;
        }


        public class CreateOrderEndpoint : CarterModule 
        {
            public CreateOrderEndpoint(): base() { }

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
}
