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
        public void AddFirstEquityStock()
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
        public void AddTwoStocks()
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

            _eventAggregatorMock.VerifyGetEvent(2);
            _fundChangedEventMock.VerifyPublishEvent(2);

            Assert.That(allStocks.Count(), Is.EqualTo(2));
            Assert.That(allStocks.Count(s => s.Type == request0.Type && s.Quantity == request0.Quantity && s.Price == request0.Price), Is.EqualTo(1));
            Assert.That(allStocks.Count(s => s.Type == request1.Type && s.Quantity == request1.Quantity && s.Price == request1.Price), Is.EqualTo(1));
            Assert.That(allStocks[0].TotalMarketValue, Is.EqualTo(request0.Price * request0.Quantity + request1.Price * request1.Quantity));
        }
    }
}
