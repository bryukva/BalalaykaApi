using Balalayka.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Balalayka.Data;

public class BalalaykaStore : IBalalaykaStore
{
    private readonly BalalaykaDbContext _context;
    public BalalaykaStore(BalalaykaDbContext context)
    {
        this._context = context;
    }

    public async Task<string?> Get(int code, CancellationToken ctx)
    {
        _context.Balalaykas.Add(new BalalaykaEntity {Code = 1, Value = "test1"});
        await _context.SaveChangesAsync(ctx);
        
        return await _context.Balalaykas
            .Where(x => x.Code == code)
            .Select(x => x.Value)
            .FirstOrDefaultAsync(ctx);
    }
}