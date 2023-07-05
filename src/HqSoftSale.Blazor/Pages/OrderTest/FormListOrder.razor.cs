using Blazorise;
using DevExpress.Blazor;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using HqSoftSale.Blazor.Pages.Orders;
using HqSoftSale.OrderDetails;
using HqSoftSale.Orders;
using HqSoftSale.Products;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace HqSoftSale.Blazor.Pages.OrderTest
{
    public partial class FormListOrder
    {
        DxWindow windowRef;
        IGrid Grid { get; set; }
        protected CreateUpdateOrderDto EditingEntity = new();
        private IReadOnlyList<OrderDto> ordersList { get; set; }    
        private List<OrderDto> selectedRows = new List<OrderDto>();
        private IReadOnlyList<ProductDto> products { get; set; }
        protected Validations CreateValationRef;
        protected CreateUpdateOrderDto NewEntity = new();
        protected CreateUpdateOrdDetailsDto NewDetailEntity = new();
        private int PageSize { get; set; } = 1000;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetOrderAsync();
            await base.OnInitializedAsync();
            if (CreateValationRef != null)
            {
                await CreateValationRef.ClearAll();
            }     
            await CalculatePrice();
            await GetProductAsync();
        }

        private async Task GetOrderAsync()
        {
            var result = await OrderAppService.GetListAsync(
                new GetOrderListDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );
            ordersList = result.Items;
            TotalCount = (int)result.TotalCount;
        }        

        private void GoToEditPage(OrderDto order)
        {
            NavigationManager.NavigateTo($"order/edit/{order.Id}");
        }

        void OnRowClick(GridRowClickEventArgs e)
        {
            string dateValue = e.Grid.GetRowValue(e.VisibleIndex, "Id").ToString();           
            NavigationManager.NavigateTo($"order/edit/{dateValue}");
        }

        //private void OnRowClick(GridRowClickEventArgs args)
        //{
        //    OrderDto clickedOrder = args.Grid.SelectRow;
        //    var orderId = clickedOrder.OrderNumber;
        //    NavigationManager.NavigateTo($"order/edit/{orderId}");
        //}

        public async void Refresh()
        {
           await OnInitializedAsync();
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

                    await OnInitializedAsync();
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
          
            await OnInitializedAsync();
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
