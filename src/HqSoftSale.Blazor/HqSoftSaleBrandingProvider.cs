using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace HqSoftSale.Blazor;

[Dependency(ReplaceServices = true)]
public class HqSoftSaleBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "HqSoftSale";
}
