using System.ComponentModel.Composition;
using MyFund.Infrastructure.Models;
using MyFund.Modules.Stock.Fund;
using MyFund.Modules.Stock.Stocks;
using Prism.Events;

namespace MyFund.Modules.Stock.Converters
{
    [Export(typeof(IStockConverter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public  class StockConverter : IStockConverter
    {
        public StockItem Convert(IEventAggregator aggregator, StockModel stockModel)
        {
            return new StockItem(aggregator, stockModel);
        }

        public IFundTotalItem Convert(IEventAggregator aggregator, TotalStocksModel totalStocksModel)
        {
            return new FundTotalItem(aggregator, totalStocksModel);
        }
    }
}
