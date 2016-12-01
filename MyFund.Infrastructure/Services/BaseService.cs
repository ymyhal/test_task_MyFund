using Prism.Events;

namespace MyFund.Infrastructure.Services
{
    public abstract class BaseService
    {
        public IEventAggregator EventAggregator { get; }

        protected BaseService(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }
    }
}
