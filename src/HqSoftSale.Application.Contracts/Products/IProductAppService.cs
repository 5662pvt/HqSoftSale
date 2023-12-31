﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HqSoftSale.Products;
public interface IProductAppService :
ICrudAppService< //Defines CRUD methods
    ProductDto, //Used to show books
    Guid, //Primary key of the book entity
    PagedAndSortedResultRequestDto, //Used for paging/sorting
    CreateUpdateProductDto> //Used to create/update a book
{
}

