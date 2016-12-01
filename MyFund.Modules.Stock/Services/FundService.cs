using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.Services;
using Prism.Events;

namespace MyFund.Modules.Stock.Services
{
    /// <summary>
    /// Service which is holding all information for fund.
    /// </summary>
    [Export(typeof(IFundService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FundService : BaseService, IFundService
    {
        private static object obj1 = new object();

        private readonly ConcurrentBag<StockModel> _stocks = new ConcurrentBag<StockModel>();
        private readonly ConcurrentBag<TotalStocksModel> _totals = new ConcurrentBag<TotalStocksModel>()
        {
            new TotalStocksModel() {Type = null },
            new TotalStocksModel() {Type = StockType.Equity},
            new TotalStocksModel() {Type = StockType.Bond}
        };

        private int _numberEquities;
        private int _numberBonds;
        private int _numberTotal;
        private int _numberCurrent;

        private decimal _marketValueEquities;
        private decimal _marketValueBonds;
        private decimal _marketValueTotal;

        private decimal _stockWeightEquities;
        private decimal _stockWeightBonds;
        private const decimal StockWeightTotal = 100;

        [ImportingConstructor]
        public FundService(IEventAggregator eventAggregator) 
            : base(eventAggregator)
        {
        }

        public void AddStock(BaseStockModel baseStock)
        {
            UpdateFund(baseStock);
        }

        public IEnumerable<StockModel> AllStocks()
        {
            return _stocks;
        }

        public IEnumerable<TotalStocksModel> AllTotals()
        {
            return _totals;
        }

        private TotalStocksModel GetTotal(StockType? type = null)
        {
            decimal marketValue;
            int number;
            decimal stockWeight;

            if (type == null)
            {
                marketValue = _marketValueTotal;
                number = _numberTotal;
                stockWeight = StockWeightTotal;
            }
            else
            {
                switch (type.Value)
                {
                    case (StockType.Equity):
                        marketValue = _marketValueEquities;
                        number = _numberEquities;
                        stockWeight = _stockWeightEquities;
                        break;
                    case (StockType.Bond):
                        marketValue = _marketValueBonds;
                        number = _numberBonds;
                        stockWeight = _stockWeightBonds;
                        break;
                    default:
                        throw new InvalidOperationException($"Not supported Stock Type {type.Value}.");
                }
            }

            return new TotalStocksModel()
            {
                Type = type,
                MarketValue = marketValue,
                Number = number,
                StockWeight = stockWeight
            };
        }

        private void UpdateFund(BaseStockModel baseStockModel)
        {
            if (baseStockModel == null)
            {
                return;
            }

            // Creating new stock
            var stock = new StockModel(baseStockModel);

            // Adding it to collection
            _stocks.Add(stock);

            // increasinfg number of specific stocks

            lock (obj1) // using lock there due to Interlocked doesn't support decimals
            {
                switch (stock.Type)
                {
                    case (StockType.Equity):
                        _numberEquities++;
                        _numberCurrent = _numberEquities;
                        _marketValueEquities = _marketValueEquities + stock.Price*stock.Quantity;
                        break;
                    case (StockType.Bond):
                        _numberBonds++;
                        _numberCurrent = _numberBonds;
                        _marketValueBonds = _marketValueBonds + stock.Price*stock.Quantity;
                        break;
                    default:
                        throw new InvalidOperationException($"Not supported Stock Type {stock.Type}.");
                }

                _numberTotal = _numberEquities + _numberBonds;
                _marketValueTotal = _marketValueEquities + _marketValueBonds;
                _stockWeightEquities = _marketValueEquities * 100 / _marketValueTotal;
                _stockWeightBonds = _marketValueBonds * 100 / _marketValueTotal;
            }

            // Updating stocks
            foreach (var stockModel in _stocks)
            {
                if (stockModel.Type == stock.Type)
                {
                    stockModel.Name = stock.Type.ToString() + _numberCurrent;
                }
                stockModel.TotalMarketValue = _marketValueTotal;
            }

            // Updating totals
            foreach (var totalStocksModel in _totals)
            {
                var totalNew = GetTotal(totalStocksModel.Type);
                totalStocksModel.MarketValue = totalNew.MarketValue;
                totalStocksModel.Number = totalNew.Number;
                totalStocksModel.StockWeight = totalNew.StockWeight;
            }

            EventAggregator.GetEvent<FundChangedEvent>().Publish();
        }
    }
}
