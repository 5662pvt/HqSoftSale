using Blazorise;
using HqSoftSale.Products;
using System.Threading.Tasks;
using System;

namespace HqSoftSale.Blazor.Pages.Products
{
    public partial class ProductNew
    {
        protected Validations CreateValationRef;
        protected CreateUpdateProductDto NewEntity = new();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (CreateValationRef != null)
            {
                await CreateValationRef.ClearAll();
            }
        }

        protected virtual async Task CreateEntityAsync()
        {
            try
            {
                var validate = true;
                if (CreateValationRef != null)
                {
                    validate = await CreateValationRef.ValidateAll();
                }
                if (validate)
                {
                    await ProductAppService.CreateAsync(NewEntity);
                    NavigationManager.NavigateTo("products");
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
    }
}
