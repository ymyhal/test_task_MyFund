using MyFund.Infrastructure.Enums;

namespace MyFund.Infrastructure.Models
{
    public class StockModel : BaseStockModel
    {
        public StockModel() { }

        public StockModel(BaseStockModel baseStockModel) 
            : base(baseStockModel.Type, baseStockModel.Price, baseStockModel.Quantity)
        {
        }

        public string Name { get; set; }
        public decimal TotalMarketValue { get; set; }
    }
}
