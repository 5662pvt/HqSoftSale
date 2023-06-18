using System;
using System.Collections.Generic;
using System.Text;
using HqSoftSale.Localization;
using Volo.Abp.Application.Services;

namespace HqSoftSale;

/* Inherit your application services from this class.
 */
public abstract class HqSoftSaleAppService : ApplicationService
{
    protected HqSoftSaleAppService()
    {
        LocalizationResource = typeof(HqSoftSaleResource);
    }
}
