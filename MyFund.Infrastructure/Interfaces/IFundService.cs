using System.Collections.Generic;
using MyFund.Infrastructure.Models;

namespace MyFund.Infrastructure.Interfaces
{
    public interface IFundService
    {
        void AddStock(BaseStockModel baseStock);
        IEnumerable<StockModel> AllStocks();
        IEnumerable<TotalStocksModel> AllTotals();
    }
}
