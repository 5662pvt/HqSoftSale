using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HqSoftSale.Data;

/* This is used if database provider does't define
 * IHqSoftSaleDbSchemaMigrator implementation.
 */
public class NullHqSoftSaleDbSchemaMigrator : IHqSoftSaleDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
