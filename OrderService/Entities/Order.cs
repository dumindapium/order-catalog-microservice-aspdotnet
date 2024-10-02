namespace OrderService.Entities;

public class Order
{
    public  Guid Id { get; set; }
    public string  CustomerName { get; set; }
    public string  CustomerAddress { get; set; }
    public string CustomerCity { get; set; }

    public IList<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

}
