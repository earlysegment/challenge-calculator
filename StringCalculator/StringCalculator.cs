using System;
using System.Linq;
using System.Collections.Generic;

public class StringCalculator
{
    public int Add(string? numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        // Parse delimiters and get the updated number string
        string[] delimiters = { ",", "\n" };
        numbers = ParseDelimiters(numbers, ref delimiters);

        // Split and sum the numbers
        var numberArray = SplitNumbers(numbers, delimiters);
        return CalculateSum(numberArray);
    }

    // Parse the custom delimiters and update the number string
    private string ParseDelimiters(string numbers, ref string[] delimiters)
    {
        if (numbers.StartsWith("//"))
        {
            int newlineIndex = numbers.IndexOf('\n');
            if (newlineIndex == -1)
            {
                throw new ArgumentException("Input format is incorrect. Missing newline after custom delimiter.");
            }

            if (numbers[2] == '[' && numbers.Contains("]\n"))
            {
                int endBracketIndex = numbers.IndexOf("]\n");
                var delimiterSection = numbers.Substring(3, endBracketIndex - 3);
                delimiters = new[] { delimiterSection };
                return numbers.Substring(endBracketIndex + 2);
            }
            else
            {
                var delimiterSection = numbers.Substring(2, newlineIndex - 2);
                delimiters = new[] { delimiterSection };
                return numbers.Substring(newlineIndex + 1);
            }
        }
        return numbers;
    }

    // Split the numbers based on the delimiters
    private string[] SplitNumbers(string numbers, string[] delimiters)
    {
        return numbers.Split(delimiters, StringSplitOptions.None);
    }

    // Calculate the sum of the numbers and handle negatives and large numbers
    private int CalculateSum(string[] numberArray)
    {
        var negativeNumbers = new List<int>();
        int sum = 0;

        foreach (var num in numberArray)
        {
            if (int.TryParse(num, out int result))
            {
                if (result < 0)
                {
                    negativeNumbers.Add(result);
                }
                else if (result <= 1000)
                {
                    sum += result;
                }
            }
        }

        if (negativeNumbers.Any())
        {
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negativeNumbers)}");
        }

        return sum;
    }
}