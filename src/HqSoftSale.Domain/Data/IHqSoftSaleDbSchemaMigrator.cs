using System.Threading.Tasks;

namespace HqSoftSale.Data;

public interface IHqSoftSaleDbSchemaMigrator
{
    Task MigrateAsync();
}
