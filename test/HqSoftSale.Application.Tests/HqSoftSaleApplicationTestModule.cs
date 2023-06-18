using Volo.Abp.Modularity;

namespace HqSoftSale;

[DependsOn(
    typeof(HqSoftSaleApplicationModule),
    typeof(HqSoftSaleDomainTestModule)
    )]
public class HqSoftSaleApplicationTestModule : AbpModule
{

}
