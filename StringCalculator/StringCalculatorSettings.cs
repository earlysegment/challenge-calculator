public class StringCalculatorSettings : IStringCalculatorSettings
{
    public bool DenyNegativeNumbers { get; set; }
    public int UpperBound { get; set; }
    public string[] CustomDelimiters { get; set; }  

    public StringCalculatorSettings(bool denyNegativeNumbers = true, int upperBound = 1000, string[]? customDelimiters = null)
    {
        DenyNegativeNumbers = denyNegativeNumbers;
        UpperBound = upperBound;
        CustomDelimiters = customDelimiters ?? new[] { ",", "\n" };  
    }
}