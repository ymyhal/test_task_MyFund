using System;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.ViewModels;
using Prism.Events;

namespace MyFund.Modules.Stock.Stocks
{
    public class StockItem : BaseViewModel, IStockItem
    {
        public StockItem(IEventAggregator eventAggregator, StockModel stock) 
            : base(eventAggregator)
        {
            if (stock == null)
            {
                throw new ArgumentNullException(nameof(stock));
            }
            Populate(stock);
        }

        public StockType StockType { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public decimal MarketValue { get; private set; }
        public decimal TransitionCost { get; private set; }
        public decimal StockWeight { get; private set; }
        public bool NameHighlighted { get; private set; }

        private void Populate(StockModel stock)
        {
            StockType = stock.Type;
            Name = stock.Name;
            Price = stock.Price;
            Quantity = stock.Quantity;
            MarketValue = stock.Price * stock.Quantity;
            switch (stock.Type)
            {
                case (StockType.Equity):
                    TransitionCost = MarketValue * 0.005m;
                    NameHighlighted = MarketValue < 0 || TransitionCost > 200000;
                    break;
                case (StockType.Bond):
                    TransitionCost = MarketValue * 0.02m;
                    NameHighlighted = MarketValue < 0 || TransitionCost > 100000;
                    break;
                default:
                    throw new InvalidOperationException($"Not supported Stock Type {stock.Type}.");
            }
            StockWeight = MarketValue * 100 / stock.TotalMarketValue;
        }
    }
}
