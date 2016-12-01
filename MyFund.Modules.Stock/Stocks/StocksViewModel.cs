using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.ViewModels;
using MyFund.Modules.Stock.Converters;
using Prism.Events;

namespace MyFund.Modules.Stock.Stocks
{
    [Export(typeof(IStocksViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class StocksViewModel : BaseViewModel, IStocksViewModel
    {
        private readonly IFundService _fundService;
        private readonly IStockConverter _stockConverter;

        [ImportingConstructor]
        public StocksViewModel(IEventAggregator eventAggregator, IFundService fundService, IStockConverter stockConverter) 
            : base(eventAggregator)
        {
            _fundService = fundService;
            _stockConverter = stockConverter;
            GetAllStocks();
            EventAggregator.GetEvent<FundChangedEvent>().Subscribe(OnFundChanged, false);
        }

        public ObservableCollection<StockItem> Stocks { get; private set; }

        public void OnFundChanged()
        {
            GetAllStocks();
        }

        private void GetAllStocks()
        {
            var stocks = _fundService.AllStocks().Select(s => _stockConverter.Convert(EventAggregator, s));
            Stocks = new ObservableCollection<StockItem>(stocks);
            OnPropertyChanged(() => Stocks);
        }
    }
}
