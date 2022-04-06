using System.Linq.Expressions;
using Balalayka.Data.Models;
using SpeciVacation;

namespace Balalayka.Data;

public class Specifications
{
    public sealed class CodeSpecification : Specification<BalalaykaEntity>
    {
        private int? _lower;
        private int? _upper;
        public CodeSpecification(int? upper, int? lower)
        {
            _lower = lower;
            _upper = upper;
        }

        public override Expression<Func<BalalaykaEntity, bool>> ToExpression()
        {
            return x => _lower == null 
                ? _upper == null || x.Code < _upper 
                : x.Code >= _lower && x.Code < _upper;
        }
    }
    
    public sealed class ValueSpecification : Specification<BalalaykaEntity>
    {
        private readonly string? _mask;
        public ValueSpecification(string? value)
        {
            _mask = value;
        }

        public override Expression<Func<BalalaykaEntity, bool>> ToExpression()
        {
            return x => string.IsNullOrEmpty(_mask) || x.Value.StartsWith(_mask) || x.Value.EndsWith(_mask);
        }
    }
}