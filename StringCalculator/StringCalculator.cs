using System;
using System.Linq;

public class StringCalculator
{
    public int Add(string? numbers)
    {
        // Handle null or empty input
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        // Split the numbers by comma
        var numberArray = numbers.Split(',');

        // Convert each valid number, invalid numbers are treated as 0
        return numberArray.Sum(x =>
        {
            if (int.TryParse(x, out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine($"Warning: '{x}' is not a valid number. Treated as 0.");
                return 0;
            }
        });
    }
}