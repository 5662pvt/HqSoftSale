using AutoMapper.Internal.Mappers;
using Blazorise;
using HqSoftSale.Orders;
using HqSoftSale.Products;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using HqSoftSale.Blazor.Pages.Products;
using HqSoftSale.OrderDetails;

namespace HqSoftSale.Blazor.Pages.Orders
{
    public partial class OrderEdit
    {
        protected CreateUpdateOrderDto EditingEntity = new();
        protected CreateUpdateProductDto NewEntityProduct = new();
        protected Validations CreateValationRef;
        protected Validations EditValidationsRef;
        private IReadOnlyList<OrdDetailDto> OrdDetailList { get; set; }

        [Parameter]
        public string Id { get; set; }
        public Guid EditingEntityId { get; set; }

        protected override async Task OnInitializedAsync()
        {


            await base.OnInitializedAsync();

            EditingEntityId = Guid.Parse(Id);

            var entityDto = await OrderAppService.GetAsync(EditingEntityId);

            EditingEntity = ObjectMapper.Map<OrderDto, CreateUpdateOrderDto>(entityDto);


            if (EditValidationsRef != null)
            {
                await EditValidationsRef.ClearAll();

            }
            var orderId = EditingEntity.OrderNumber;
            OrdDetailList = await OrderDetailAppService.GetProductsByOrderDetail(orderId);
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

        protected virtual async Task UpdateProductEntityAsync()
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
                    await ProductAppService.UpdateAsync(EditingEntityId, NewEntityProduct);

                    NavigationManager.NavigateTo("orders");
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        protected virtual async Task DeleteEntityAsync(Guid Id)
        {
            await OrderAppService.DeleteAsync(Id);
            NavigationManager.NavigateTo("orders");
        }

        private void GoToOrderPage()
        {
            NavigationManager.NavigateTo("/orders");
        }      

        protected virtual async Task CreateProductEntityAsync()
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
                    await ProductAppService.CreateAsync(NewEntityProduct);
                    //NavigationManager.NavigateTo("products");
                    NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private bool _hideItem = false;

        private void HideFilterBy()
        {
            _hideItem = !_hideItem;
        }
    }
}
