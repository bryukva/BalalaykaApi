namespace Balalayka.Data;

public interface IBalalaykaStore
{
    Task<string?> Get(int code, CancellationToken ctx);
}