using MyFund.Infrastructure.Enums;
using Prism.Events;

namespace MyFund.Modules.Stock.Fund
{
    public interface IFundTotalItem
    {
        IEventAggregator EventAggregator { get; }

        StockType? Type { get; }
        int Number { get; }
        decimal StockWeight { get; }
        decimal MarketValue { get; }
    }
}
