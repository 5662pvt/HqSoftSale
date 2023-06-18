using Volo.Abp.Settings;

namespace HqSoftSale.Settings;

public class HqSoftSaleSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(HqSoftSaleSettings.MySetting1));
    }
}
