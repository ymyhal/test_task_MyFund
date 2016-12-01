using MyFund.Infrastructure.Enums;
using Prism.Events;

namespace MyFund.Modules.Stock.Stocks
{
    public interface IStockItem
    {
        IEventAggregator EventAggregator { get; }

        StockType StockType { get; }
        string Name { get; }
        decimal Price { get; }
        int Quantity { get; }
        decimal MarketValue { get; }
        decimal TransitionCost { get; }
        decimal StockWeight { get; }
        bool NameHighlighted { get; }
    }
}
