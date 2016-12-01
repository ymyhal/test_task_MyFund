using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.Tests.TestHeppers;
using MyFund.Modules.Stock.Stocks;
using NUnit.Framework;
using Prism.Events;

namespace MyFund.Modules.Stock.Tests.Stocks
{
    [TestFixture]
    public class StockItemTests
    {
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<FundChangedEvent> _fundChangedEventMock;
        private StockItemTests _stockItem;

        [SetUp]
        public void SetUp()
        {
            _fundChangedEventMock = TestsHelper.SetUpFundEventMock();
            _eventAggregatorMock = _fundChangedEventMock.SetUpEventAggregatorMock();
        }

        [Test]
        public void EmptyStockModelFailed()
        {
            Assert.That(() => new StockItem(_eventAggregatorMock.Object, null), Throws.TypeOf<ArgumentNullException>());
        }

        [TestCase(StockType.Equity, 1, 1, "name1", 1, StockType.Equity, "name1", 1, 1, 1, 0.005, 100, false)]
        [TestCase(StockType.Bond, 1, 1, "name2", 1, StockType.Bond, "name2", 1, 1, 1, 0.02, 100, false)]
        [TestCase(StockType.Equity, 100000000, 3, "name2", 100, StockType.Equity, "name2", 100000000, 3, 300000000, 1500000, 300000000, true)]
        [TestCase(StockType.Bond, 100000000, 3, "name2", 100, StockType.Bond, "name2", 100000000, 3, 300000000, 6000000, 300000000, true)]
        public void CreateNewStockSucess(
            StockType typeIn,
            decimal priceIn,
            int quantityIn,
            string nameIn,
            decimal totalMarketValueIn,
            StockType typeOut,
            string nameOut,
            decimal priceOut,
            int quantityOut,
            decimal marketValueOut,
            decimal transitionCostOut,
            decimal stockWeightOut,
            bool nameHighlightedOut)
        {
            var stock = new StockModel()
            {
                Type = typeIn,
                Price = priceIn,
                Quantity = quantityIn,
                Name = nameIn,
                TotalMarketValue = totalMarketValueIn
            };

            var result = new StockItem(_eventAggregatorMock.Object, stock);

            Assert.That(result.Type, Is.EqualTo(typeOut));
            Assert.That(result.Price, Is.EqualTo(priceOut));
            Assert.That(result.Quantity, Is.EqualTo(quantityOut));
            Assert.That(result.Name, Is.EqualTo(nameOut));
            Assert.That(result.MarketValue, Is.EqualTo(marketValueOut));
            Assert.That(result.TransitionCost, Is.EqualTo(transitionCostOut));
            Assert.That(Math.Round(result.StockWeight, 2), Is.EqualTo(stockWeightOut));
            Assert.That(result.NameHighlighted, Is.EqualTo(nameHighlightedOut));
        }
    }
}
