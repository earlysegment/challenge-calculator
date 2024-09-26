public class StringCalculatorSettings : IStringCalculatorSettings
{
    public bool DenyNegativeNumbers { get; private set; }
    public int UpperBound { get; private set; }
    public string[] CustomDelimiters { get; private set; }

    public StringCalculatorSettings(bool denyNegativeNumbers = true, int upperBound = 1000, string[]? customDelimiters = null)
    {
        DenyNegativeNumbers = denyNegativeNumbers;
        UpperBound = upperBound;
        CustomDelimiters = customDelimiters ?? new[] { ",", "\n" };
    }
}