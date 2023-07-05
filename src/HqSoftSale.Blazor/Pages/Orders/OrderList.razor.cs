using Blazorise.DataGrid;
using Blazorise;
using HqSoftSale.Orders;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HqSoftSale.Blazor.Pages.Orders
{
    public partial class OrderList
    {
        protected CreateUpdateOrderDto EditingEntity = new();
        private IReadOnlyList<OrderDto> ordersList { get; set; }
        private List<OrderDto> selectedRows = new List<OrderDto>();

        private int PageSize { get; set; } = 1000;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetOrderAsync();
        
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

       
        async Task DeleteSelectedRows()
        {
            foreach (var item in selectedRows)
            {
                await AppService.DeleteAsync(item.Id);
            }
            await GetOrderAsync();
            selectedRows = new List<OrderDto>();
        }

        private void GoToEditPage(OrderDto order)
        {
            NavigationManager.NavigateTo($"order/edit/{order.Id}");
        }

        private void GoToCreatePage()
        {
            NavigationManager.NavigateTo($"/order/new");
        }
        
       
    }
}
