using Blazorise;
using HqSoftSale.OrderDetails;
using HqSoftSale.Orders;
using HqSoftSale.Products;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlazoriseUI.Components;

namespace HqSoftSale.Blazor.Pages.ODDetail
{
    public  partial class ODDetailEdit
    {
        [Parameter]
        public Guid OrderId { get; set; }
        public string Id { get; set; }
        public Guid EditingEntityId { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }

           
        protected CreateUpdateOrdDetailsDto EditingDetailEntity = new();
      
        protected Validations EditValidationsRef;
        private IReadOnlyList<OrdDetailDto> OrdDetailList { get; set; }
        protected DataGridEntityActionsColumn<OrdDetailDto> EntityActionsColumn;

        private List<OrdDetailDto> selectedRows = new List<OrdDetailDto>();


        protected override async Task OnInitializedAsync()
        {
            await GetOrderDettailAsync();

            await base.OnInitializedAsync();

            EditingEntityId = Guid.Parse(Id);

            var entityDto = await OrderAppService.GetAsync(EditingEntityId);
            var entitydetailDto = await OrderDetailAppService.GetAsync(EditingEntityId);

            EditingEntity = ObjectMapper.Map<OrderDto, CreateUpdateOrderDto>(entityDto);
            EditingEntity = ObjectMapper.Map<OrdDetailDto, CreateUpdateOrderDto>(entitydetailDto);


            if (EditValidationsRef != null)
            {
                await EditValidationsRef.ClearAll();

            }
            var orderId = EditingEntity.OrderNumber;
            OrdDetailList = await OrderDetailAppService.GetProducts(orderId);
        }

        private async Task GetOrderDettailAsync()
        {
            var result = await OrderDetailAppService.GetListAsync(
                new GetOrdDetailListDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            OrdDetailList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task UpdateEntityAsync()
        {
            try
            {
                var validate = true;
                if (EditValidationsRef != null)
                {
                    validate = await EditValidationsRef.ValidateAll();
                }
                if (validate)
                {
                    await OrderAppService.UpdateAsync(EditingEntityId, EditingEntity);

                    NavigationManager.NavigateTo("orders");
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        
    }
}
