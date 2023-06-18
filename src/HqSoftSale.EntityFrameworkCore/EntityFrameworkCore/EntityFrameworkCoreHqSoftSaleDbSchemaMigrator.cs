using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HqSoftSale.Data;
using Volo.Abp.DependencyInjection;

namespace HqSoftSale.EntityFrameworkCore;

public class EntityFrameworkCoreHqSoftSaleDbSchemaMigrator
    : IHqSoftSaleDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreHqSoftSaleDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the HqSoftSaleDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<HqSoftSaleDbContext>()
            .Database
            .MigrateAsync();
    }
}
