using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using MyFund.Infrastructure;
using MyFund.Infrastructure.Behaviors;
using MyFund.Modules.Stock.Services;
using Prism.Mef;
using Prism.Regions;

namespace MyFund
{
    public class MyFundBootstrapper : MefBootstrapper
    {
        protected override void ConfigureAggregateCatalog()
        {
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(MyFundBootstrapper).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(RegionNames).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(FundService).Assembly));
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Shell) this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();

            factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

            return factory;
        }

        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
        }

        // when using Unity
        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    return new AggregateModuleCatalog(); 
        //}
    }
}
