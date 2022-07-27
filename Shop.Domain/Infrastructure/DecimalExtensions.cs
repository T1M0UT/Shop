namespace Shop.Domain.Infrastructure;

public static class DecimalExtensions
{
    public static string GetPriceString(this decimal value)
    {
        return $"${value:N2}";
    }
}