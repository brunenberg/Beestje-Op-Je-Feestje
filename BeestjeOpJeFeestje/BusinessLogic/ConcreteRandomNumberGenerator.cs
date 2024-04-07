using BusinessLogic.Interfaces;

public class ConcreteRandomNumberGenerator : IRandomNumberGenerator {
    private Random _random = new Random();

    public int Next(int minValue, int maxValue) {
        return _random.Next(minValue, maxValue);
    }
}
