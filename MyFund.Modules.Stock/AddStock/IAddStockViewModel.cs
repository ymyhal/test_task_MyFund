using System.Collections.Generic;
using System.Windows.Input;
using MyFund.Infrastructure.Enums;
using Prism.Commands;
using Prism.Events;

namespace MyFund.Modules.Stock.AddStock
{
    public interface IAddStockViewModel
    {
        IEventAggregator EventAggregator { get; }
        DelegateCommand<object> AddStockCommand { get; }
        IEnumerable<StockType> StockTypes { get; }
        StockType StockTypeSelected { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
    }
}
