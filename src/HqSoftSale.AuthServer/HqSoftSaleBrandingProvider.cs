using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace HqSoftSale;

[Dependency(ReplaceServices = true)]
public class HqSoftSaleBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "HqSoftSale";
}
