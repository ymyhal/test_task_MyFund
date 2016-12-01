using MyFund.Infrastructure.Enums;

namespace MyFund.Infrastructure.Models
{
    public class BaseStockModel
    {
        public BaseStockModel() { }

        public BaseStockModel(StockType type, decimal price, int quantity)
        {
            Type = type;
            Price = price;
            Quantity = quantity;
        }

        public StockType Type { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
