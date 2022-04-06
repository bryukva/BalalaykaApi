using Balalayka.Domain.Dtos;

namespace Balalayka.Data;

public interface IBalalaykaStore
{
    Task<string?> Get(int code, CancellationToken ctx);
    Task<int> AddList(IReadOnlyCollection<BalalaykaCandidate> candidates, CancellationToken ctx);
    Task<IReadOnlyCollection<Domain.Models.Balalayka>> GetList(BalalaykasFilter? filter, CancellationToken ctx);
    Task DeleteAll(CancellationToken ctx);
}