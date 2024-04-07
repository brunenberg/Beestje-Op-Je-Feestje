using BusinessLogic;

public interface IDiscountRule {
    (int discountPercentage, List<string>? discountMessage) GetDiscount(DiscountContext context);
}