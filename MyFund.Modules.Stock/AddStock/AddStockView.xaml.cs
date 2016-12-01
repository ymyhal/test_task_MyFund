using System.ComponentModel.Composition;
using System.Windows.Controls;
using MyFund.Infrastructure;
using MyFund.Infrastructure.Behaviors;

namespace MyFund.Modules.Stock.AddStock
{
    [ViewExport(RegionName = RegionNames.ToopPanelRegion)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AddUserView : UserControl
    {
        public AddUserView()
        {
            InitializeComponent();
        }

        [Import]
        public IAddStockViewModel Model
        {
            get
            {
                return DataContext as IAddStockViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
