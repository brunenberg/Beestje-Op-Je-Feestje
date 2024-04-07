namespace BusinessLogic.Rules.PricingRules
{
    public class DayDiscountRule : IDiscountRule
    {
        public (int discountPercentage, List<string>? discountMessage) GetDiscount(DiscountContext context)
        {
            if (context.BookingDate.DayOfWeek == DayOfWeek.Monday || context.BookingDate.DayOfWeek == DayOfWeek.Tuesday) {
                return (15, new List<string> { "Maandag of dinsdag: 15%" });
            }
            return (0, null);
        }
    }
}