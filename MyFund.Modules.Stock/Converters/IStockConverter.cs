using MyFund.Infrastructure.Models;
using MyFund.Modules.Stock.Fund;
using MyFund.Modules.Stock.Stocks;
using Prism.Events;

namespace MyFund.Modules.Stock.Converters
{
    public interface IStockConverter
    {
        StockItem Convert(IEventAggregator aggregator, StockModel stockModel);
        IFundTotalItem Convert(IEventAggregator aggregator, TotalStocksModel totalStocksModel);
    }
}
