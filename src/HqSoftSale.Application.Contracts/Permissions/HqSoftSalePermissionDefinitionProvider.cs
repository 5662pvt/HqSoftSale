using HqSoftSale.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace HqSoftSale.Permissions;

public class HqSoftSalePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HqSoftSalePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(HqSoftSalePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HqSoftSaleResource>(name);
    }
}
