using NUnit.Framework;
using System;
using System.IO;

[TestFixture]
public class StringCalculatorTests
{
    // Requirement 1: Empty or missing numbers should return 0
    [Test]
    public void Add_EmptyString_ReturnsZero()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add(""), Is.EqualTo(0));
    }

    // Requirement 1: Null input should return 0
    [Test]
    public void Add_NullInput_ReturnsZero()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add(null), Is.EqualTo(0));
    }

    // Requirement 1: Sum of two valid numbers
    [Test]
    public void Add_TwoValidNumbers_ReturnsSum()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("1,5"), Is.EqualTo(6));  // Now using 1 and 5
    }

    // Requirement 2: Sum of multiple numbers (no limit)
    [Test]
    public void Add_MultipleNumbers_ReturnsSum()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("1,2,3,4,5,6"), Is.EqualTo(21));
    }

    // Requirement 1: Invalid number should be treated as 0
    [Test]
    public void Add_InvalidNumber_ReturnsSumWithInvalidAsZero()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("5,tytyt"), Is.EqualTo(5));
    }

    // Requirement 1: Consecutive commas (missing numbers) treated as 0
    [Test]
    public void Add_ConsecutiveCommas_ReturnsSumTreatingMissingNumbersAsZero()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("1,,2"), Is.EqualTo(3));
    }

    // Requirement 3: Newline characters as delimiters
    [Test]
    public void Add_NewlineAsDelimiter_ReturnsSum()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("1\n2,3"), Is.EqualTo(6)); // 1 + 2 + 3 = 6
    }

    [Test]
    public void Add_NewlineOnlyAsDelimiter_ReturnsSum()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("1\n2\n3"), Is.EqualTo(6)); // 1 + 2 + 3 = 6
    }

    // Requirement 4: Throw exception on negative numbers
    [Test]
    public void Add_NegativeNumbers_ThrowsException()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("1,-2,3,-4"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2, -4"));
    }

    [Test]
    public void Add_SingleNegativeNumber_ThrowsException()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("-5"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -5"));
    }

    // Requirement 5: Ignored values greater than 1000 in the calculation
    [Test]
    public void Add_NumbersGreaterThan1000_AreIgnored()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("2,1001,6"), Is.EqualTo(8));  // 1001 is ignored
    }

    // Requirement 6: Custom delimiter should work
    [Test]
    public void Add_CustomDelimiter_ReturnsSum()
    {
        var settings = new StringCalculatorSettings(customDelimiters: new[] { "#" });
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("//#\n2#5"), Is.EqualTo(7));  // Custom delimiter '#'
    }

    // Requirement 6: Custom delimiter with invalid numbers
    [Test]
    public void Add_CustomDelimiter_InvalidNumbers_ReturnsSumTreatingInvalidAsZero()
    {
        var settings = new StringCalculatorSettings(customDelimiters: new[] { ";" });
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("//;\n3;tytyt"), Is.EqualTo(3));  // Custom delimiter ';', invalid number "tytyt"
    }

    // Requirement 6: Custom delimiter with negative numbers should throw exception
    [Test]
    public void Add_CustomDelimiter_NegativeNumbers_ThrowsException()
    {
        var settings = new StringCalculatorSettings(customDelimiters: new[] { ";" });
        var calculator = new StringCalculator(settings);
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("//;\n3;-2;4;-5"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2, -5"));
    }

    // Requirement 7: Custom delimiter of any length should work
    [Test]
    public void Add_CustomDelimiterOfAnyLength_ReturnsSum()
    {
        var settings = new StringCalculatorSettings(customDelimiters: new[] { "***" });
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("//[***]\n11***22***33"), Is.EqualTo(66));  // Custom delimiter "***"
    }

    // Requirement 7: Custom delimiter with invalid numbers
    [Test]
    public void Add_CustomDelimiterOfAnyLength_InvalidNumbers_ReturnsSumTreatingInvalidAsZero()
    {
        var settings = new StringCalculatorSettings(customDelimiters: new[] { "##" });
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("//[##]\n3##tytyt##5"), Is.EqualTo(8));  // Custom delimiter "##"
    }

    // Requirement 7: Custom delimiter with negative numbers should throw exception
    [Test]
    public void Add_CustomDelimiterOfAnyLength_NegativeNumbers_ThrowsException()
    {
        var settings = new StringCalculatorSettings(customDelimiters: new[] { "###" });
        var calculator = new StringCalculator(settings);
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("//[###]\n3###-2###4###-5"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2, -5"));
    }

    // Requirement 8: Multiple custom delimiters of any length
    [Test]
    public void Add_MultipleCustomDelimitersOfAnyLength_ReturnsSum()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("//[*][!!][r9r]\n11r9r22*hh*33!!44"), Is.EqualTo(110));  // Custom delimiters "*", "!!", "r9r"
    }

    // Requirement 8: Custom delimiters with invalid numbers
    [Test]
    public void Add_MultipleCustomDelimiters_InvalidNumbers_ReturnsSumTreatingInvalidAsZero()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("//[*][!!]\n3*tytyt!!5"), Is.EqualTo(8));  // Custom delimiters "*" and "!!"
    }

    // Requirement 8: Custom delimiters with negative numbers should throw exception
    [Test]
    public void Add_MultipleCustomDelimiters_NegativeNumbers_ThrowsException()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        var ex = Assert.Throws<ArgumentException>(() => calculator.Add("//[***][#]\n3***-2#4"));
        Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2"));
    }

    // Stretch Goal 1: Display the formula used for calculation
    private StringWriter _stringWriter;

    [SetUp]
    public void Setup()
    {
        _stringWriter = new StringWriter();
        Console.SetOut(_stringWriter);  // Redirect console output to the StringWriter
    }

    [TearDown]
    public void TearDown()
    {
        _stringWriter.Dispose();  // Dispose of the StringWriter
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });  // Restore console output
    }

    [Test]
    public void Add_DisplayFormulaForValidInput_ShowsCorrectFormula()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        calculator.Add("2,,4,rrrr,1001,6");

        // Get the output from the console
        var result = _stringWriter.ToString().Trim();

        // Assert the expected formula output
        Assert.That(result, Is.EqualTo("Formula: 2 + 0 + 4 + 0 + 0 + 6 = 12"));
    }

    [Test]
    public void Add_DisplayFormulaForCustomDelimiter_ShowsCorrectFormula()
    {
        var settings = new StringCalculatorSettings();
        var calculator = new StringCalculator(settings);
        calculator.Add("//#\n2#5");

        // Get the output from the console
        var result = _stringWriter.ToString().Trim();

        // Assert the expected formula output
        Assert.That(result, Is.EqualTo("Formula: 2 + 5 = 7"));
    }

    // Stretch Goal #3: Test if custom upper bound, delimiter, and allowing negative numbers work correctly
    [Test]
    public void Add_WithCustomSettings_RespectsUpperBoundAndNegativeNumbers()
    {
        var settings = new StringCalculatorSettings(denyNegativeNumbers: false, upperBound: 500, customDelimiters: new[] { "#" });
        var calculator = new StringCalculator(settings);
        Assert.That(calculator.Add("100#-200#600"), Is.EqualTo(-100));  // 600 is ignored (greater than upper bound of 500)
    }

    // Stretch Goal #4: Test case to verify DI with injected settings
    [Test]
    public void Add_WithInjectedSettings_ReturnsCorrectSum()
    {
        // Arrange: Create settings to be injected
        var settings = new StringCalculatorSettings
        {
            DenyNegativeNumbers = true,
            UpperBound = 1000,
            CustomDelimiters = new[] { ";" }
        };

        // Inject the settings using DI
        var calculator = new StringCalculator(settings);

        // Act: Use the calculator with the injected settings
        int result = calculator.Add("1;2;3");

        // Assert: Check that the result matches expected sum
        Assert.That(result, Is.EqualTo(6));  // 1 + 2 + 3 = 6
    }

    // Stretch Goal #4: Test case to verify DI with upper bound setting
    [Test]
    public void Add_WithUpperBoundSetting_IgnoresNumbersAboveUpperBound()
    {
        // Arrange: Inject settings with custom upper bound
        var settings = new StringCalculatorSettings
        {
            DenyNegativeNumbers = true,
            UpperBound = 100,
            CustomDelimiters = new[] { "," }
        };

        var calculator = new StringCalculator(settings);

        // Act: Add numbers with one value exceeding the upper bound
        int result = calculator.Add("10,200,5");

        // Assert: Check that numbers above the upper bound are ignored
        Assert.That(result, Is.EqualTo(15));  // 10 + 5 = 15, 200 is ignored
    }
}