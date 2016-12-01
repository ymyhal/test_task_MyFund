using Moq;
using MyFund.Infrastructure.Events;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.Tests.TestHeppers;
using MyFund.Modules.Stock.Services;
using NUnit.Framework;
using Prism.Events;

namespace MyFund.Modules.Stock.Tests.AddStock
{
    [TestFixture]
    public class AddStockViewModelTests
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
        public void ProperInitializationSuccess()
        {
            
        }
    }
}
