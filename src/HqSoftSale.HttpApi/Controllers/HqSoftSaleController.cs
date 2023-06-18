using HqSoftSale.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace HqSoftSale.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class HqSoftSaleController : AbpControllerBase
{
    protected HqSoftSaleController()
    {
        LocalizationResource = typeof(HqSoftSaleResource);
    }
}
