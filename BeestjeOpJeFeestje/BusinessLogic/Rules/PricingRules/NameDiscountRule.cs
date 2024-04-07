namespace BusinessLogic.Rules.PricingRules
{
    public class NameDiscountRule : IDiscountRule
    {
        public (int discountPercentage, List<string>? discountMessage) GetDiscount(DiscountContext context)
        {
            HashSet<char> uniqueChars = new HashSet<char>(context.SelectedAnimals.SelectMany(a => a.Name.ToUpper()));
            int discount = 0;
            List<string> discountMessages = new List<string>();

            for (char c = 'A'; c <= 'Z'; c++) {
                if (uniqueChars.Contains(c)) {
                    discount += 2;
                    discountMessages.Add($"Beestje naam met '{c}' erin: 2%");
                } else {
                    break;
                }
            }

            if (discountMessages.Count > 0) {
                return (discount, discountMessages);
            }
            
            return (discount, null);
        }
    }
}