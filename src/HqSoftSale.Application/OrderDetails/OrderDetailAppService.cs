using AutoMapper.Internal.Mappers;
using HqSoftSale.Orders;
using HqSoftSale.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace HqSoftSale.OrderDetails
{
    public class OrderDetailAppService :
    CrudAppService<
        OrderDetail, 
        OrdDetailDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateOrdDetailsDto>,
    IOrdDetailAppService 
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;

        public OrderDetailAppService(
            IRepository<OrderDetail, Guid> repository,
            IRepository<Order> orderRepository,
            IRepository<Product> productRepository,
            IRepository<OrderDetail> orderDetailRepository)
            : base(repository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public override async Task<OrdDetailDto> GetAsync(Guid id)
        {         
            var queryable = await Repository.GetQueryableAsync();
         
            var query = from OrderDetail in queryable
                        where OrderDetail.Id == id
                        select new { OrderDetail };

            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(OrderDetail), id);
            }

            var OrderDto = ObjectMapper.Map<OrderDetail, OrdDetailDto>(queryResult.OrderDetail);
            return OrderDto;
        }

        public override async Task<PagedResultDto<OrdDetailDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await Repository.GetQueryableAsync();

            var query = from OrderDetail in queryable
                        select new { OrderDetail };
            query = query
                .OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var queryResult = await AsyncExecuter.ToListAsync(query);

            var OrderDtos = queryResult.Select(x =>
            {
                var OrderDto = ObjectMapper.Map<OrderDetail, OrdDetailDto>(x.OrderDetail);
                return OrderDto;
            }).ToList();

            var totalCount = await Repository.GetCountAsync();

            return new PagedResultDto<OrdDetailDto>(
                totalCount,
                OrderDtos
            );
        }

        private static string NormalizeSorting(string sorting)
        {
            if (sorting.IsNullOrEmpty())
            {
                return $"OrderDetail.{nameof(OrderDetail.OrderID)}";
            }
            return $"OrderDetail.{sorting}";
        }

        public async Task<List<OrdDetailDto>> GetProductsByOrderDetail(string orderId)
        {
            var orderDetails = await _orderDetailRepository.GetListAsync(od => od.OrderID == orderId);

            var products = new List<OrdDetailDto>();

            foreach (var orderDetail in orderDetails)
            {
                var product = await _orderDetailRepository.FirstOrDefaultAsync(p => p.ProductID == orderDetail.ProductID);

                if (product != null)
                {
                    products.Add(ObjectMapper.Map<OrderDetail, OrdDetailDto>(product));
                }
            }
            return products;
        }

        //public async Task<List<OrdDetailDto>> GetProductsByOrderDetail(string orderId, string productId)
        //{
        //    var orderDetail = await _orderDetailRepository.FirstOrDefaultAsync(od =>
        //        od.OrderID == orderId && od.ProductID == productId);

        //    if (orderDetail == null)
        //    {
        //        throw new UserFriendlyException("Order detail not found.");
        //    }

        //    var products = await _orderDetailRepository.GetListAsync(p =>
        //        p.ProductID == orderDetail.ProductID);

        //    return ObjectMapper.Map<List<OrderDetail>, List<OrdDetailDto>>(products);
        //}
    }
}
