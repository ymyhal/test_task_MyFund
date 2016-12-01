using System.Collections.Generic;
using System.Linq;
using Moq;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.Tests.TestHeppers;
using MyFund.Modules.Stock.Services;
using NUnit.Framework;
using Prism.Events;

namespace MyFund.Modules.Stock.Tests.Services
{
    [TestFixture]
    public class FundServiceTests
    {
        private IFundService _fundService;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<FundChangedEvent> _fundChangedEventMock;

        [SetUp]
        public void SetUp()
        {
            _fundChangedEventMock = TestsHelper.SetUpFundEventMock();
            _eventAggregatorMock = _fundChangedEventMock.SetUpEventAggregatorMock();
            _fundService = new FundService(_eventAggregatorMock.Object);
        }

        [Test]
        public void AddFirstEquityStockSuccess()
        {
            var request = new BaseStockModel()
            {
                Price = 1,
                Quantity = 1,
                Type = StockType.Equity
            };

            _fundService.AddStock(request);

            var allStocks = _fundService.AllStocks().ToArray();

            _eventAggregatorMock.VerifyGetEvent();
            _fundChangedEventMock.VerifyPublishEvent();

            Assert.That(allStocks.Count(), Is.EqualTo(1));
            Assert.That(allStocks[0].Type, Is.EqualTo(request.Type));
            Assert.That(allStocks[0].Price, Is.EqualTo(request.Price));
            Assert.That(allStocks[0].Quantity, Is.EqualTo(request.Quantity));
            Assert.That(allStocks[0].TotalMarketValue, Is.EqualTo(request.Price * request.Quantity));
        }

        [Test]
        public void AddTwoStocksSuccess()
        {
            var request0 = new BaseStockModel()
            {
                Price = 1,
                Quantity = 1,
                Type = StockType.Equity
            };
            var request1 = new BaseStockModel()
            {
                Price = 1,
                Quantity = 1,
                Type = StockType.Bond
            };

            _fundService.AddStock(request0);
            _fundService.AddStock(request1);

            var allStocks = _fundService.AllStocks().ToArray();

            AssertEvents(2);

            AssertStocksLength(allStocks, 2);
            Assert.That(allStocks.Count(s => s.Type == request0.Type && s.Quantity == request0.Quantity && s.Price == request0.Price), Is.EqualTo(1));
            Assert.That(allStocks.Count(s => s.Type == request1.Type && s.Quantity == request1.Quantity && s.Price == request1.Price), Is.EqualTo(1));
            AssertTotalMarketValueInStock(allStocks[0], new[] { request0, request1 });
        }

        [Test]
        public void AddThreeStocksCheckTotalsNumberSuccess()
        {
            var request0 = new BaseStockModel()
            {
                Price = 1,
                Quantity = 1,
                Type = StockType.Equity
            };
            var request1 = new BaseStockModel()
            {
                Price = 1,
                Quantity = 1,
                Type = StockType.Bond
            };

            var request2 = new BaseStockModel()
            {
                Price = 1,
                Quantity = 1,
                Type = StockType.Bond
            };

            _fundService.AddStock(request0);
            _fundService.AddStock(request1);
            _fundService.AddStock(request2);

            var allStocks = _fundService.AllStocks().ToArray();

            AssertEvents(3);

            AssertStocksLength(allStocks, 3);
            AssertTotalMarketValueInStock(allStocks[0], new[] { request0, request1, request2 });

            var allTotals = _fundService.AllTotals().ToArray();

            Assert.That(allTotals.Length, Is.EqualTo(3));
            Assert.That(allTotals.Count(t => t.Type == null), Is.EqualTo(1));
            Assert.That(allTotals.Count(t => t.Type == StockType.Bond), Is.EqualTo(1));
            Assert.That(allTotals.Count(t => t.Type == StockType.Equity), Is.EqualTo(1));
        }

        [Test]
        public void AddThreeStocksCheckTotalsSuccess()
        {
            var request0 = new BaseStockModel()
            {
                Price = 1,
                Quantity = 100,
                Type = StockType.Equity
            };
            var request1 = new BaseStockModel()
            {
                Price = 10,
                Quantity = 10,
                Type = StockType.Bond
            };

            var request2 = new BaseStockModel()
            {
                Price = 1000,
                Quantity = 1,
                Type = StockType.Bond
            };

            _fundService.AddStock(request0);
            _fundService.AddStock(request1);
            _fundService.AddStock(request2);

            var allStocks = _fundService.AllStocks().ToArray();

            AssertEvents(3);

            AssertStocksLength(allStocks, 3);
            AssertTotalMarketValueInStock(allStocks[0], new[] {request0, request1, request2});
           
            var allTotals = _fundService.AllTotals().ToArray();

            AssertTotalsNumber(allTotals);
        }

        private void AssertEvents(int expectedNumberOfEvents)
        {
            _eventAggregatorMock.VerifyGetEvent(expectedNumberOfEvents);
            _fundChangedEventMock.VerifyPublishEvent(expectedNumberOfEvents);
        }

        private void AssertStocksLength(IEnumerable<StockModel> stocks, int expectedLength)
        {
            Assert.That(stocks.Count(), Is.EqualTo(expectedLength));
        }

        private void AssertTotalMarketValueInStock(StockModel stock, IEnumerable<BaseStockModel> requests)
        {
            Assert.That(stock.TotalMarketValue, Is.EqualTo(requests.Sum(s => s.Price * s.Quantity)));
        }

        private void AssertTotalsNumber(IEnumerable<TotalStocksModel> totals)
        {
            Assert.That(totals.Count(), Is.EqualTo(3));
            Assert.That(totals.Count(t => t.Type == null), Is.EqualTo(1));
            Assert.That(totals.Count(t => t.Type == StockType.Bond), Is.EqualTo(1));
            Assert.That(totals.Count(t => t.Type == StockType.Equity), Is.EqualTo(1));
        }
    }
}
