namespace OrderService.Entities;

public class OrderLine
{
    public Guid Id { get; set; }
    public int SeqNo { get; set; }
    public Item  Item { get; set; }
    public Guid ItemId { get; set; }
    public decimal Price { get; set; }
}
