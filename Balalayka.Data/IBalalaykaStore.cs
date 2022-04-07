using Balalayka.Domain.Dtos;

namespace Balalayka.Data;

public interface IBalalaykaStore
{
    Task<Domain.Models.Balalayka?> Get(int code, CancellationToken ctx);
    Task<int> AddList(IReadOnlyCollection<BalalaykaCandidate> candidates, CancellationToken ctx);
    Task<IReadOnlyCollection<Domain.Models.Balalayka>> GetList(BalalaykasFilter? filter, CancellationToken ctx);
    Task DeleteAll(CancellationToken ctx);
    Task Delete(long? id, CancellationToken ctx);
}