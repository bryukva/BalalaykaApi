using Balalayka.Data.Models;
using Balalayka.Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpeciVacation;

namespace Balalayka.Data;

public class BalalaykaStore : IBalalaykaStore
{
    private readonly BalalaykaDbContext _context;
    private readonly ILogger<BalalaykaStore> _logger;
    public BalalaykaStore(BalalaykaDbContext context, ILogger<BalalaykaStore> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Domain.Models.Balalayka?> Get(int code, CancellationToken ctx)
    {
        var found = await _context.Balalaykas
            .Where(x => x.Code == code)
            .FirstOrDefaultAsync(ctx);
        return found == null ? null : new Domain.Models.Balalayka(found.Id, found.Code, found.Value);
    }
    
    public async Task<int> AddList(IReadOnlyCollection<BalalaykaCandidate> candidates, CancellationToken ctx)
    {
        try
        {
            foreach (var (code, value) in candidates.OrderBy(x => x.Code))
            {
                _context.Balalaykas.Add(new BalalaykaEntity
                {
                    Code = code, 
                    Value = value,
                });
            }
            
            return await _context.SaveChangesAsync(ctx);
        }
        catch (Exception ex)
        {
            _logger.LogError($@"Error inserting database: {ex.Message}");
            return -1;
        }
    }

    public async Task<IReadOnlyCollection<Domain.Models.Balalayka>> GetList(BalalaykasFilter? filter, CancellationToken ctx)
    {
        var spec = Specification<BalalaykaEntity>.All;
        if (filter != null)
        {
            if (filter.CodeLowerLimit != null || filter.CodeUpperLimit != null)
            {
                spec = spec.And(new Specifications.CodeSpecification(filter.CodeUpperLimit, filter.CodeLowerLimit));
            }
            if (!string.IsNullOrEmpty(filter.ValueMask))
            {
                spec = spec.And(new Specifications.ValueSpecification(filter.ValueMask));
            }
        }
        
        return await _context.Balalaykas
            .Where(spec.ToExpression())
            .Select(x => x.ToDomain())
            .ToListAsync(ctx);
    }

    public async Task DeleteAll(CancellationToken ctx)
    {
        //for better performance on large datasets use "truncate table" raw sql command
        await this.Delete(null, ctx);
    }

    public async Task Delete(long? id, CancellationToken ctx)
    {
        foreach (var item in _context.Balalaykas.Where(x => id == null || x.Id == id))
        {
            _context.Balalaykas.Remove(item);
        }
        await _context.SaveChangesAsync(ctx);
    }
}