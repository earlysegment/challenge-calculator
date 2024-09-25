using NUnit.Framework;

[TestFixture]
public class StringCalculatorTests
{
    [Test]
    public void Add_EmptyString_ReturnsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add(""), Is.EqualTo(0));
    }

    [Test]
    public void Add_TwoValidNumbers_ReturnsSum()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("1,5000"), Is.EqualTo(5001));
    }

    [Test]
    public void Add_InvalidNumber_ReturnsSumWithInvalidAsZero()
    {
        var calculator = new StringCalculator();
        Assert.That(calculator.Add("5,tytyt"), Is.EqualTo(5));
    }

    [Test]
    public void Add_ThreeNumbers_ThrowsException()
    {
        var calculator = new StringCalculator();
        Assert.Throws<ArgumentException>(() => calculator.Add("1,2,3"));
    }
}