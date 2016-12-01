using System.Collections.ObjectModel;
using Prism.Events;

namespace MyFund.Modules.Stock.Stocks
{
    public interface IStocksViewModel
    {
        IEventAggregator EventAggregator { get; }
        ObservableCollection<StockItem> Stocks { get; }
        void OnFundChanged();
    }
}
