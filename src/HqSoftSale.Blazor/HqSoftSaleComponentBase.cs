using HqSoftSale.Localization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components;

namespace HqSoftSale.Blazor;

public abstract class HqSoftSaleComponentBase : AbpComponentBase
{
    protected HqSoftSaleComponentBase()
    {
        LocalizationResource = typeof(HqSoftSaleResource);
    }

        protected override async Task OnInitializedAsync()
        {
         
        }
}
