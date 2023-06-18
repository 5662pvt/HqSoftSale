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
        OrderDetail, //The Order entity
        OrdDetailDto, //Used to show Orders
        Guid, //Primary key of the Order entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateOrdDetailsDto>, //Used to create/update a Order
    IOrdDetailAppService //implement the IOrderAppService
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
            //Get the IQueryable<Order> from the repository
            var queryable = await Repository.GetQueryableAsync();

            //Prepare a query to join Orders and authors
            var query = from OrderDetail in queryable
                        where OrderDetail.Id == id
                        select new { OrderDetail };

            //Execute the query and get the Order with author
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
            //Get the IQueryable<Order> from the repository
            var queryable = await Repository.GetQueryableAsync();

            //Prepare a query to join Orders and authors
            var query = from OrderDetail in queryable
                        select new { OrderDetail };

            //Paging
            query = query
                .OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(query);

            //Convert the query result to a list of OrderDto objects
            var OrderDtos = queryResult.Select(x =>
            {
                var OrderDto = ObjectMapper.Map<OrderDetail, OrdDetailDto>(x.OrderDetail);
                return OrderDto;
            }).ToList();

            //Get the total count with another query
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


            // Vòng lặp để truy vấn danh sách sản phẩm tương ứng với mỗi chi tiết đơn hàng trong danh sách
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

        public async Task<List<OrdDetailDto>> GetProductsByOrderDetail(string orderId, string productId)
        {
            var orderDetail = await _orderDetailRepository.FirstOrDefaultAsync(od =>
                od.OrderID == orderId && od.ProductID == productId);

            if (orderDetail == null)
            {
                throw new UserFriendlyException("Order detail not found.");
            }

            var products = await _orderDetailRepository.GetListAsync(p =>
                p.ProductID == orderDetail.ProductID);

            return ObjectMapper.Map<List<OrderDetail>, List<OrdDetailDto>>(products);
        }
    }
}
