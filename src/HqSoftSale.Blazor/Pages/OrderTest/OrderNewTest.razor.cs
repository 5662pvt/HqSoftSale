using Blazorise;
using HqSoftSale.OrderDetails;
using HqSoftSale.Orders;
using HqSoftSale.Products;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using DevExpress.Blazor.Base;

namespace HqSoftSale.Blazor.Pages.OrderTest
{
    public partial class OrderNewTest
    {
        private IReadOnlyList<OrderDto> orders { get; set; }
        private IReadOnlyList<OrdDetailDto> orderDetails { get; set; }
        private IReadOnlyList<ProductDto> products { get; set; }

        protected Validations CreateValationRef;
        protected CreateUpdateOrderDto NewEntity = new();
        protected CreateUpdateOrdDetailsDto NewDetailEntity = new();
        private int PageSize { get; set; } = 1000;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (CreateValationRef != null)
            {
                await CreateValationRef.ClearAll();
            }

            await CalculatePrice();
            await GetProductAsync();
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
                    OrderAppService.CreateAsync(NewEntity);
                    OrderDetailAppService.CreateAsync(NewDetailEntity);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task GetProductAsync()
        {
            var result = await ProductAppService.GetListAsync(
                new GetProductListDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            products = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private void GoToEditPage(OrderDto order)
        {
            NavigationManager.NavigateTo($"order/edit/{order.Id}");
        }

        private void UpdateTotal(int quantity)
        {
            NewDetailEntity.Quantity = quantity;
            NewDetailEntity.ExtenedAmount = NewDetailEntity.Quantity * NewDetailEntity.Price;
        }

        protected virtual async Task CalculatePrice()
        {
            if (!string.IsNullOrEmpty(NewDetailEntity.ProductID))
            {
                var product = products.FirstOrDefault(p => p.ProductID == NewDetailEntity.ProductID);
                if (product != null)
                {
                    NewDetailEntity.Price = NewDetailEntity.Quantity * product.Price;
                }
            }
        }
    }
}
