namespace Balalayka.Domain.Dtos;

public record BalalaykasFilter(int? CodeUpperLimit, int? CodeLowerLimit, string? ValueMask);