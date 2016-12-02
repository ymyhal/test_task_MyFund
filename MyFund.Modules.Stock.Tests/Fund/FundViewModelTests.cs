using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.Tests.TestHeppers;
using MyFund.Modules.Stock.Converters;
using MyFund.Modules.Stock.Fund;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Prism.Events;

namespace MyFund.Modules.Stock.Tests.Fund
{
    [TestFixture]
    public class FundViewModelTests
    {
        private Mock<IFundService> _fundServiceMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<FundChangedEvent> _fundChangedEventMock;
        private IFundViewModel _model;

        [SetUp]
        public void SetUp()
        {
            _fundChangedEventMock = TestsHelper.SetUpFundEventMock();
            _eventAggregatorMock = _fundChangedEventMock.SetUpEventAggregatorMock();
        }

        public void ValidateOnFundChangedEventHandler()
        {
            _fundServiceMock.Setup(f => f.AllTotals()).Returns(new[]
            {
                new TotalStocksModel()
                {
                    Type = null,
                    Number = 100,
                    MarketValue = 200,
                    StockWeight = 300
                },
                new TotalStocksModel()
                {
                    Type = StockType.Bond,
                    Number = 40,
                    MarketValue = 200,
                    StockWeight = 300
                },
                new TotalStocksModel()
                {
                    Type = StockType.Equity,
                    Number = 60,
                    MarketValue = 200,
                    StockWeight = 300
                },
            });
            _fundChangedEventMock.Setup(e => e.Subscribe(It.IsAny<Action>(), It.IsAny<bool>())).Callback(
                (Action act, bool keep) =>
                {
                    _model.OnFundChanged();
                });
            _model = new FundViewModel(_eventAggregatorMock.Object, _fundServiceMock.Object, new StockConverter());

            Assert.That(_model.FundSummaryItems, Is.NaN);
            Assert.That(_model.FundSummaryItems.Count(), Is.EqualTo(3));
            Assert.That(_model.FundSummaryItems.Count(t => t.Type == null), Is.EqualTo(1));
            Assert.That(_model.FundSummaryItems.Count(t => t.Type == StockType.Bond), Is.EqualTo(1));
            Assert.That(_model.FundSummaryItems.Count(t => t.Type == StockType.Equity), Is.EqualTo(1));
        }
    }
}
