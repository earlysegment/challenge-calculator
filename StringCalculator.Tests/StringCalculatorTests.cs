using NUnit.Framework;

[TestFixture]
public class StringCalculatorTests
{
    // Requirement 1: Empty or missing numbers should return 0
    [Test]
    public void Add_EmptyString_ReturnsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add(""), Is.EqualTo(0));
    }

    // Requirement 1: Null input should return 0
    [Test]
    public void Add_NullInput_ReturnsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add(null), Is.EqualTo(0));
    }

    // Requirement 1: Sum of two valid numbers
    [Test]
    public void Add_TwoValidNumbers_ReturnsSum()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("1,5000"), Is.EqualTo(5001));
    }

    // Requirement 2: Sum of multiple numbers (no limit)
    [Test]
    public void Add_MultipleNumbers_ReturnsSum()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("1,2,3,4,5,6"), Is.EqualTo(21));
    }

    // Requirement 1: Invalid number should be treated as 0
    [Test]
    public void Add_InvalidNumber_ReturnsSumWithInvalidAsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("5,tytyt"), Is.EqualTo(5));
    }

    // Requirement 1: Consecutive commas (missing numbers) treated as 0
    [Test]
    public void Add_ConsecutiveCommas_ReturnsSumTreatingMissingNumbersAsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("1,,2"), Is.EqualTo(3));
    }
}