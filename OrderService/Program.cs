var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

List<Order> orders = new();

app.MapGet("/", () =>
{
    return orders;
})
.WithName("GetOrders").WithOpenApi();

app.MapPost("/", (Order order) =>
{
    orders.Add(order);
})
.WithName("PostOrder").WithOpenApi();

app.Run();

internal record Order(string CustomerName, string CustomerAddress, string ItemNo, int Qty, DateTime CreatedDate);