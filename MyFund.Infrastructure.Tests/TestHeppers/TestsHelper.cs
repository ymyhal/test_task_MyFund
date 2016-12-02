using System;
using Moq;
using MyFund.Infrastructure.Events;
using Prism.Events;

namespace MyFund.Infrastructure.Tests.TestHeppers
{
    public static class TestsHelper
    {
        public static Mock<IEventAggregator> GetEventAggregatorMock()
        {
            return new Mock<IEventAggregator>();
        }

        public static Mock<FundChangedEvent> SetUpFundEventMock()
        {
            var fundEventMock = new Mock<FundChangedEvent>();
            fundEventMock.Setup(m => m.Publish());
            //fundEventMock.Setup(m => m.Subscribe(It.IsAny<Action>(), It.IsAny<bool>()));
            return fundEventMock;
        }

        public static Mock<IEventAggregator> SetUpEventAggregatorMock(this Mock<FundChangedEvent> fundChangedEventMock)
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(m => m.GetEvent<FundChangedEvent>()).Returns(fundChangedEventMock.Object);

            return eventAggregatorMock;
        }

        public static void VerifyPublishEvent(this Mock<FundChangedEvent> fundChangedEventMock, int expectedNumberOfCalls = 1)
        {
            fundChangedEventMock.Verify(m => m.Publish(), Times.Exactly(expectedNumberOfCalls));
        }

        public static void VerifySubscribeEvent(this Mock<FundChangedEvent> fundChangedEventMock, int expectedNumberOfCalls = 1)
        {
            fundChangedEventMock.Verify(m => m.Subscribe(It.IsAny<Action>(), It.IsAny<bool>()), Times.Exactly(expectedNumberOfCalls));
        }

        public static void VerifyGetEvent(this Mock<IEventAggregator> eventAggregatorMock, int expectedNumberOfCals = 1)
        {
            eventAggregatorMock.Verify(m => m.GetEvent<FundChangedEvent>(), Times.Exactly(expectedNumberOfCals));
        }
    }
}
