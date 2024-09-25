using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class StringCalculator
{
    public int Add(string? numbers)
    {
        // Handle null or empty input
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        string[] delimiters = { ",", "\n" }; // Default delimiters

        // Check if a custom delimiter is provided
        if (numbers.StartsWith("//"))
        {
            int newlineIndex = numbers.IndexOf('\n');
            if (newlineIndex == -1)
            {
                throw new ArgumentException("Input format is incorrect. Missing newline after custom delimiter.");
            }

            // Check for a custom delimiter of any length in the format //[{delimiter}]\n
            if (numbers[2] == '[' && numbers.Contains("]\n"))
            {
                int endBracketIndex = numbers.IndexOf("]\n");
                var delimiterSection = numbers.Substring(3, endBracketIndex - 3);
                delimiters = new[] { delimiterSection };  // Set the custom delimiter
                numbers = numbers.Substring(endBracketIndex + 2);  // Remove delimiter part
            }
            else
            {
                // Handle the single-character custom delimiter format
                var delimiterSection = numbers.Substring(2, newlineIndex - 2);
                delimiters = new[] { delimiterSection };
                numbers = numbers.Substring(newlineIndex + 1);  // Remove delimiter part
            }
        }

        // Split the numbers using the delimiters
        var numberArray = numbers.Split(delimiters, StringSplitOptions.None);

        // List to track negative numbers
        var negativeNumbers = new List<int>();

        // Sum the valid numbers and collect negative numbers
        int sum = 0;
        foreach (var num in numberArray)
        {
            if (int.TryParse(num, out int result))
            {
                if (result < 0)
                {
                    // Collect negative numbers
                    negativeNumbers.Add(result);
                }
                else if (result <= 1000)
                {
                    // Sum numbers less than or equal to 1000
                    sum += result;
                }
            }
        }

        // If there are negative numbers, throw an exception
        if (negativeNumbers.Any())
        {
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negativeNumbers)}");
        }

        return sum;
    }
}