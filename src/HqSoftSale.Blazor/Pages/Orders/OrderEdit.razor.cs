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
using Volo.Abp.BlazoriseUI.Components;
using System.Linq;

namespace HqSoftSale.Blazor.Pages.Orders
{
    public partial class OrderEdit
    {
        [Parameter]
        public string Id { get; set; }
        public Guid EditingEntityId { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }

        protected CreateUpdateOrderDto EditingEntity = new();
        protected CreateUpdateProductDto NewEntityProduct = new();
        protected CreateUpdateOrdDetailsDto EditingDetailEntity = new();
        protected CreateUpdateOrdDetailsDto CreateDetailEntity = new();
        protected Validations CreateValationRef;
        protected Validations EditValidationsRef;

        private IReadOnlyList<OrderDto> orders { get; set; }
        private IReadOnlyList<OrdDetailDto> orderDetails { get; set; }
        private IReadOnlyList<ProductDto> products { get; set; }


        protected CreateUpdateOrderDto NewEntity = new();
        protected CreateUpdateOrdDetailsDto NewDetailEntity = new();
        private IReadOnlyList<OrdDetailDto> OrdDetailList { get; set; }
        protected DataGridEntityActionsColumn<OrdDetailDto> EntityActionsColumn;

        private List<OrdDetailDto> selectedRows = new List<OrdDetailDto>();


        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
            if (CreateValationRef != null)
            {
                await CreateValationRef.ClearAll();
            }

            await CalculatePrice();
            await GetProductAsync();
            await GetOrderDettailAsync();

            await base.OnInitializedAsync();

            EditingEntityId = Guid.Parse(Id);

            var entityDto = await OrderAppService.GetAsync(EditingEntityId);

            EditingEntity = ObjectMapper.Map<OrderDto, CreateUpdateOrderDto>(entityDto);


            if (EditValidationsRef != null)
            {
                await EditValidationsRef.ClearAll();

            }
            var orderId = EditingEntity.OrderNumber;
            OrdDetailList = await OrderDetailAppService.GetProducts(orderId);
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


        //protected virtual async Task OpenEditDetailModalAsync(OrdDetailDto entity)
        //{
        //    try
        //    {
        //        if (EditValidationsRef != null)
        //        {
        //            await EditValidationsRef.ClearAll();
        //        }

        //        await CheckUpdatePolicyAsync();

        //        var entityDto = await AppService.GetAsync(entity.Id);

        //        EditingEntityId = entity.Id;
        //        EditingEntity = MapToEditingEntity(entityDto);

        //        await InvokeAsync(async () =>
        //        {
        //            StateHasChanged();
        //            if (EditModal != null)
        //            {
        //                await EditModal.Show();
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await HandleErrorAsync(ex);
        //    }
        //}
        protected virtual async Task OpenEditDetailModalAsync()
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
                    await OrderDetailAppService.UpdateAsync(EditingEntityId, EditingDetailEntity);

                    NavigationManager.NavigateTo("orders");
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
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

        async Task DeleteDetailEntityAsync(Guid Id)
        {
      
            await OrderDetailAppService.DeleteAsync(Id);
            NavigationManager.NavigateTo("orders");
        }

        async Task DeleteSelectedRows()
        {
            foreach (var item in selectedRows)
            {
                await AppService.DeleteAsync(item.Id);
            }
            await GetOrderDettailAsync();
            selectedRows = new List<OrdDetailDto>();
        }

        private void GoToEditPage(OrdDetailDto order)
        {
            NavigationManager.NavigateTo($"order/edit/editdettail/{order.Id}");
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
                    
                    OrderDetailAppService.CreateAsync(CreateDetailEntity);
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

        private void UpdateTotal(int quantity)
        {
            CreateDetailEntity.Quantity = quantity;
            CreateDetailEntity.ExtenedAmount = CreateDetailEntity.Quantity * CreateDetailEntity.Price;
        }

        protected virtual async Task CalculatePrice()
        {
            if (!string.IsNullOrEmpty(CreateDetailEntity.ProductID))
            {
                var product = products.FirstOrDefault(p => p.ProductID == CreateDetailEntity.ProductID);
                if (product != null)
                {
                    CreateDetailEntity.Price = CreateDetailEntity.Quantity * product.Price;
                }
            }
        }
    }
}
