using Blazorise;
using HqSoftSale.OrderDetails;
using HqSoftSale.Orders;
using HqSoftSale.Products;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Users;

namespace HqSoftSale.Blazor.Pages.Orders
{
    public partial class OrderNew
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
                    NavigationManager.NavigateTo("orders");
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

        //protected virtual async Task CreateProductEntityAsync()
        //{
        //    try
        //    {
        //        var validate = true;
        //        if (CreateValationRef != null)
        //        {
        //            validate = await CreateValationRef.ValidateAll();
        //        }
        //        if (validate)
        //        {
        //            await ProductAppService.CreateAsync(NewEntityProduct);
        //            //NavigationManager.NavigateTo("products");
        //            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await HandleErrorAsync(ex);
        //    }
        //}

        private void UpdateTotal(int quantity)
        {
            NewDetailEntity.Quantity = quantity;
            NewDetailEntity.ExtenedAmount = NewDetailEntity.Quantity * NewDetailEntity.Price;
        }

        protected virtual async Task CalculatePrice()
        {
            if (!string.IsNullOrEmpty(NewDetailEntity.ProductID))
            {
                // Tìm sản phẩm trong productList dựa trên mã sản phẩm
                var product = products.FirstOrDefault(p => p.ProductID == NewDetailEntity.ProductID);
                if (product != null)
                {
                    // Tính toán giá tiền và đưa vào cột Price
                    NewDetailEntity.Price = NewDetailEntity.Quantity * product.Price;
                }
            }
        }
    }
}

