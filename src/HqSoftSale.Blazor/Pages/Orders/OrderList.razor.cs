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

        private string _searchString;
        private DataGrid<OrderDto> dataGrid;
        private List<string> _events = new();

        private Task _quickFilter(string e)
        {
            _searchString = e;
            return dataGrid.Reload();
        }


        //Xóa nhiều dòng trong Datagrid 
        async Task DeleteSelectedRows()
        {
            foreach (var item in selectedRows)
            {
                await AppService.DeleteAsync(item.Id);
            }
            // Refresh lại danh sách sau khi xóa
            await GetOrderAsync();
            selectedRows = new List<OrderDto>();
        }

        private void NavigateToOrderDetailPage(Guid orderId)
        {
            var orderDetailUrl = $"/orders/{orderId}";

            NavigationManager.NavigateTo(orderDetailUrl);
        }



        private void GoToEditPage(OrderDto order)
        {
            NavigationManager.NavigateTo($"order/edit/{order.Id}");
        }
        private void GoToCreatePage()
        {
            NavigationManager.NavigateTo("order/new");
        }

        private bool _hideItem = false;
    }
}
