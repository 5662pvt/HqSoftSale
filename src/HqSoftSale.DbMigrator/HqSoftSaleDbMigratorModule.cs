using HqSoftSale.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace HqSoftSale.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(HqSoftSaleEntityFrameworkCoreModule),
    typeof(HqSoftSaleApplicationContractsModule)
    )]
public class HqSoftSaleDbMigratorModule : AbpModule
{

}
