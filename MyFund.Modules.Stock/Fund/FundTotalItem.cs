using System;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.ViewModels;
using Prism.Events;

namespace MyFund.Modules.Stock.Fund
{
    public class FundTotalItem : BaseViewModel, IFundTotalItem
    {
        public FundTotalItem(IEventAggregator aggregator, TotalStocksModel totalStocksModel)
            : base(aggregator)
        {
            if (totalStocksModel == null)
            {
                throw new ArgumentNullException(nameof(totalStocksModel));
            }

            Populate(totalStocksModel);
        }

        public StockType? Type { get; private set; }
        public int Number { get; private set; }
        public decimal StockWeight { get; private set; }
        public decimal MarketValue { get; private set; }

        private void Populate(TotalStocksModel totalStocksModel)
        {
            Type = totalStocksModel.Type;
            Number = totalStocksModel.Number;
            StockWeight = totalStocksModel.StockWeight;
            MarketValue = totalStocksModel.MarketValue;
        }
    }
}
