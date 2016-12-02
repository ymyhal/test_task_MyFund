using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Events;

namespace MyFund.Modules.Stock.Stocks
{
    public interface IStocksViewModel
    {
        IEventAggregator EventAggregator { get; }
        IEnumerable<StockItem> Stocks { get; }
        void OnFundChanged();
    }
}
