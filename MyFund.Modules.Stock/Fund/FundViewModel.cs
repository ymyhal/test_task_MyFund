using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.ViewModels;
using MyFund.Modules.Stock.Converters;
using Prism.Events;

namespace MyFund.Modules.Stock.Fund
{
    [Export(typeof(IFundViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FundViewModel : BaseViewModel, IFundViewModel
    {
        private readonly IFundService _fundService;
        private readonly IStockConverter _stockConverter;

        [ImportingConstructor]
        public FundViewModel(IEventAggregator aggregator, IFundService fundService, IStockConverter stockConverter)
            : base(aggregator)
        {
            _fundService = fundService;
            _stockConverter = stockConverter;
            EventAggregator.GetEvent<FundChangedEvent>().Subscribe(OnFundChanged, false);
        }

        public IEnumerable<IFundTotalItem> FundSummaryItems { get; private set; }

        public void OnFundChanged()
        {
            var fundTotalItems = _fundService.AllTotals().Select(s => _stockConverter.Convert(EventAggregator, s));
            FundSummaryItems = new List<IFundTotalItem>(fundTotalItems);
            OnPropertyChanged(() => FundSummaryItems);
        }
    }
}
