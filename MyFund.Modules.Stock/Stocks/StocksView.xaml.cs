using System.ComponentModel.Composition;
using System.Windows.Controls;
using MyFund.Infrastructure;
using MyFund.Infrastructure.Behaviors;

namespace MyFund.Modules.Stock.Stocks
{
    [ViewExport(RegionName = RegionNames.MainPanelRegion)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class StocksView : UserControl
    {
        public StocksView()
        {
            InitializeComponent();
        }

        [Import]
        public IStocksViewModel Model
        {
            get
            {
                return DataContext as IStocksViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
