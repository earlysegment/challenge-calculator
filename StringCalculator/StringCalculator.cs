using System;
using System.Linq;

public class StringCalculator
{
    public int Add(string? numbers)
    {
        // Handle null or empty input
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        // Replace the literal string "\n" with an actual newline character
        numbers = numbers.Replace(@"\n", "\n");

        // Define the delimiters: comma and newline
        var delimiters = new[] { ',', '\n' };

        // Split the numbers by both comma and newline delimiters
        var numberArray = numbers.Split(delimiters, StringSplitOptions.None);

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