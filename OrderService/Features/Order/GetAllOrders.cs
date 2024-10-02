using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using static OrderService.Features.Order.CreateOrder;

namespace OrderService.Features.Order;

public static class GetAllOrders
{
    public record Command():IRequest<IEnumerable<OrderResponse>>;

    public record OrderResponse(Guid Id,
        string CustomerName,
        string CustomerAddress,
        string CustomerCity,
        IEnumerable<Line> Lines);
    public record Line(int SeqNo, int ProductId, decimal Price);

    internal class Handler(OrderDbContext _dbContext) : IRequestHandler<Command, IEnumerable<OrderResponse>>
    {

        public async Task<IEnumerable<OrderResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var orders = _dbContext.Orders.Include(a=>a.OrderLines);

            var results = orders.Select(o =>
                            new OrderResponse(
                                o.Id,
                                o.CustomerName,
                                o.CustomerAddress,
                                o.CustomerCity,
                                o.OrderLines.Select(l =>
                                    new Line(l.SeqNo, l.ProductId, l.Price))
                                )
                            );

            return await results.ToListAsync(cancellationToken);
        }
    }

    public class GetAllOrdersEndpoint : CarterModule
    {
        public GetAllOrdersEndpoint() : base() { }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/getall", async (ISender sender) =>
            {
                var result = await sender.Send(new Command());

                return Results.Ok(result);

            }).WithOpenApi();
        }
    }
}
