using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Balalayka.Data.Models;

public class BalalaykaEntity
{
    public long Id { get; set; }
    public int Code { get; set; }
    public string Value { get; set; }

    public Domain.Models.Balalayka ToDomain() => new (Id, Code, Value);
    
    private class Config : IEntityTypeConfiguration<BalalaykaEntity>
    {
        public void Configure(EntityTypeBuilder<BalalaykaEntity> b)
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Code);
        }
    }
}
