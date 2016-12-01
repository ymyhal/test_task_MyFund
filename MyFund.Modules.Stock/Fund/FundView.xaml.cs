using System.ComponentModel.Composition;
using System.Windows.Controls;
using MyFund.Infrastructure;
using MyFund.Infrastructure.Behaviors;

namespace MyFund.Modules.Stock.Fund
{
    [ViewExport(RegionName = RegionNames.RightPanelRegion)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FundView : UserControl
    {
        public FundView()
        {
            InitializeComponent();
        }

        [Import]
        public IFundViewModel Model
        {
            get
            {
                return DataContext as IFundViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
