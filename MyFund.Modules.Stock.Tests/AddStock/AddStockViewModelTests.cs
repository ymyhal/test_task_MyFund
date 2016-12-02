using System;
using System.Linq;
using Moq;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.Tests.TestHeppers;
using MyFund.Modules.Stock.AddStock;
using NUnit.Framework;
using Prism.Events;

namespace MyFund.Modules.Stock.Tests.AddStock
{
    [TestFixture]
    public class AddStockViewModelTests
    {
        private Mock<IFundService> _fundServiceMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<FundChangedEvent> _fundChangedEventMock;
        private IAddStockViewModel _vModel;

        [SetUp]
        public void SetUp()
        {
            _fundChangedEventMock = TestsHelper.SetUpFundEventMock();
            _eventAggregatorMock = _fundChangedEventMock.SetUpEventAggregatorMock();
            _fundServiceMock = new Mock<IFundService>();
        }

        [Test]
        public void ProperInitialization()
        {
            _vModel = new AddStockViewModel(_eventAggregatorMock.Object, _fundServiceMock.Object);

            Assert.That(_vModel.Price, Is.EqualTo(0));
            Assert.That(_vModel.Quantity, Is.EqualTo(0));
            Assert.That(_vModel.StockTypes, Is.EqualTo(Enum.GetValues(typeof(StockType)).Cast<StockType>().ToList()));
            Assert.That(_vModel.HasErrors, Is.True);
            Assert.That(_vModel.Errors, Is.Not.Empty);
            Assert.That(_vModel.Errors, Has.Exactly(1).EqualTo(AddStockViewModel.ErrorPriceIsLessOrEquallThanZerro));
            Assert.That(_vModel.Errors, Has.Exactly(1).EqualTo(AddStockViewModel.ErrorQuantityIsLessOrEquallThanZerro));
            Assert.That(_vModel.AddStockCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void ProperValidation()
        {
            _vModel = new AddStockViewModel(_eventAggregatorMock.Object, _fundServiceMock.Object);

            _vModel.StockTypeSelected = StockType.Bond;
            _vModel.Price = 1;
            _vModel.Quantity = 1;

            Assert.That(_vModel.Price, Is.EqualTo(1));
            Assert.That(_vModel.Quantity, Is.EqualTo(1));
            Assert.That(_vModel.StockTypeSelected, Is.EqualTo(StockType.Bond));
            Assert.That(_vModel.HasErrors, Is.False);
            Assert.That(_vModel.Errors == null || !_vModel.Errors.Any(), Is.True);
            Assert.That(_vModel.AddStockCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void ProperAddButtonHandler()
        {
            _vModel = new AddStockViewModel(_eventAggregatorMock.Object, _fundServiceMock.Object);

            _vModel.StockTypeSelected = StockType.Bond;
            _vModel.Price = 10;
            _vModel.Quantity = 10;

            Assert.That(_vModel.AddStockCommand.CanExecute(null), Is.True);

            _vModel.AddStockCommand.Execute(null);

            _fundServiceMock.Setup(f => f.AddStock(It.IsAny<BaseStockModel>())).Callback((BaseStockModel model) =>
            {
                Assert.That(model.Type, Is.EqualTo(_vModel.StockTypeSelected));
                Assert.That(model.Price, Is.EqualTo(_vModel.Price));
                Assert.That(model.Quantity, Is.EqualTo(_vModel.Quantity));
            });
        }
    }
}
