public interface IStringCalculatorSettings
{
    bool DenyNegativeNumbers { get; }
    int UpperBound { get; }
    string[] CustomDelimiters { get; }  // Renamed from Delimiters to CustomDelimiters
}