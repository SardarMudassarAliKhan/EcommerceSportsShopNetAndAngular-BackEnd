namespace Core.Entities.OrderAggregate
{
    public class OrderDeliveryMethod : BaseEntity
    {
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}