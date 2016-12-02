using System.Collections.Generic;
using Prism.Events;

namespace MyFund.Modules.Stock.Fund
{
    public interface IFundViewModel
    {
        IEventAggregator EventAggregator { get; }
        IEnumerable<IFundTotalItem> FundSummaryItems { get; }
        void OnFundChanged();
    }
}
