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
        // Use numbers less than or equal to 1000 to match the behavior after Requirement #5
        Assert.That(calculator.Add("1,5"), Is.EqualTo(6));  // Now using 1 and 5
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

    // Requirement 3: Newline characters as delimiters
    [Test]
    public void Add_NewlineAsDelimiter_ReturnsSum()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("1\n2,3"), Is.EqualTo(6)); // 1 + 2 + 3 = 6
    }

    [Test]
    public void Add_NewlineOnlyAsDelimiter_ReturnsSum()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("1\n2\n3"), Is.EqualTo(6)); // 1 + 2 + 3 = 6
    }

    // Requirement 4: Throw exception on negative numbers
    [Test]
    public void Add_NegativeNumbers_ThrowsException()
    {
        var calculator = new StringCalculator();
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("1,-2,3,-4"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2, -4"));
    }

    [Test]
    public void Add_SingleNegativeNumber_ThrowsException()
    {
        var calculator = new StringCalculator();
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("-5"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -5"));
    }

    // Requirement 5: Ignored values greater than 1000 in the calculation
    [Test]
    public void Add_NumbersGreaterThan1000_AreIgnored()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("2,1001,6"), Is.EqualTo(8));  // 1001 is ignored
    }

    // Requirement 6: Custom delimiter should work
    [Test]
    public void Add_CustomDelimiter_ReturnsSum()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("//#\n2#5"), Is.EqualTo(7));  // Custom delimiter '#'
    }

    // Requirement 6: Custom delimiter with invalid numbers
    [Test]
    public void Add_CustomDelimiter_InvalidNumbers_ReturnsSumTreatingInvalidAsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("//;\n3;tytyt"), Is.EqualTo(3));  // Custom delimiter ';', invalid number "tytyt"
    }

    // Requirement 6: Custom delimiter with negative numbers should throw exception
    [Test]
    public void Add_CustomDelimiter_NegativeNumbers_ThrowsException()
    {
        var calculator = new StringCalculator();
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("//;\n3;-2;4;-5"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2, -5"));
    }

    // Requirement 7: Custom delimiter of any length should work
    [Test]
    public void Add_CustomDelimiterOfAnyLength_ReturnsSum()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("//[***]\n11***22***33"), Is.EqualTo(66));  // Custom delimiter "***"
    }

    // Requirement 7: Custom delimiter with invalid numbers
    [Test]
    public void Add_CustomDelimiterOfAnyLength_InvalidNumbers_ReturnsSumTreatingInvalidAsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("//[##]\n3##tytyt##5"), Is.EqualTo(8));  // Custom delimiter "##"
    }

    // Requirement 7: Custom delimiter with negative numbers should throw exception
    [Test]
    public void Add_CustomDelimiterOfAnyLength_NegativeNumbers_ThrowsException()
    {
        var calculator = new StringCalculator();
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("//[###]\n3###-2###4###-5"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2, -5"));
    }

    
}
