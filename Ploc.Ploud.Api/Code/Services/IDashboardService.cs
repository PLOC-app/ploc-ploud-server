using Ploc.Ploud.Library;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public interface IDashboardService
    {
        Task<Dashboard> GetDashboardAsync(RequestBase request, SyncSettings syncSettings);
    }
}
