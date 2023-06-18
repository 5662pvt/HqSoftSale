using HqSoftSale.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace HqSoftSale;

[DependsOn(
    typeof(HqSoftSaleEntityFrameworkCoreTestModule)
    )]
public class HqSoftSaleDomainTestModule : AbpModule
{

}
