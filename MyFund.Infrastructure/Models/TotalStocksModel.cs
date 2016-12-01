using MyFund.Infrastructure.Enums;

namespace MyFund.Infrastructure.Models
{
    public class TotalStocksModel
    {
        public StockType? Type { get; set; }
        public int Number { get; set; }
        public decimal StockWeight { get; set; }
        public decimal MarketValue { get; set; }
    }
}
