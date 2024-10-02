namespace OrderService.Entities;

public class OrderLine
{
    public Guid Id { get; set; }
    public int SeqNo { get; set; }
    public Product  Product { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
}
