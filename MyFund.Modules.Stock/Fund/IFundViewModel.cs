using System.Collections.ObjectModel;
using Prism.Events;

namespace MyFund.Modules.Stock.Fund
{
    public interface IFundViewModel
    {
        IEventAggregator EventAggregator { get; }
        ObservableCollection<IFundTotalItem> FundSummaryItems { get; }
        void OnFundChanged();
    }
}
