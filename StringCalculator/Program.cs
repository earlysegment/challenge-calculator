using System;

class Program
{
    static void Main(string[] args)
    {
        // Default settings for command-line arguments
        bool denyNegativeNumbers = true;
        int upperBound = 1000;
        var defaultDelimiters = new[] { ",", "\n" };
        string[] customDelimiters = defaultDelimiters;

        // Parse command-line arguments
        foreach (var arg in args)
        {
            if (arg.StartsWith("--delimiter="))
            {
                var delimiter = arg.Substring("--delimiter=".Length);
                customDelimiters = new[] { delimiter };
            }
            else if (arg == "--allow-negative")
            {
                denyNegativeNumbers = false;
            }
            else if (arg.StartsWith("--upper-bound="))
            {
                upperBound = int.Parse(arg.Substring("--upper-bound=".Length));
            }
        }

        // Merge custom delimiters with the default delimiters
        customDelimiters = MergeDelimiters(customDelimiters, defaultDelimiters);

        // Create a settings object
        var settings = new StringCalculatorSettings(denyNegativeNumbers, upperBound, customDelimiters);

        // Instantiate the calculator with the settings
        var calculator = new StringCalculator(settings);

        // Handle Ctrl+C to exit the loop gracefully
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nCtrl+C detected. Exiting the program...");
            Environment.Exit(0); // Immediately terminate the program
        };

        // Display information for the user
        Console.WriteLine("String Calculator");
        Console.WriteLine("Enter a mathematical expression using numbers, operators (+, -, *, /), and delimiters.");
        Console.WriteLine("You can also use custom delimiters in the format: //[delimiter]\\n[expression].");
        Console.WriteLine("Available command-line options:");
        Console.WriteLine("--delimiter=\"[delimiter]\" to set a custom delimiter.");
        Console.WriteLine("--allow-negative to allow negative numbers.");
        Console.WriteLine("--upper-bound=[number] to set the upper bound for numbers.");
        Console.WriteLine("Type 'exit' or press Ctrl+C to quit.");

        // Main input loop
        while (true)
        {
            try
            {
                // Read input from the user
                Console.Write("\nInput: ");
                string? input = Console.ReadLine();

                // Check if input is null or 'exit'
                if (input == null || input.ToLower() == "exit")
                {
                    Console.WriteLine("Exiting the program...");
                    break;
                }

                // Calculate the result using the StringCalculator
                int result = calculator.Add(input);

                // Display the result
                Console.WriteLine($"Result: {result}");
            }
            catch (ArgumentException ex)
            {
                // Handle exceptions such as negative numbers
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (DivideByZeroException ex)
            {
                // Handle division by zero
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }

    // Helper method to merge custom delimiters with default delimiters
    static string[] MergeDelimiters(string[] customDelimiters, string[] defaultDelimiters)
    {
        var mergedDelimiters = new string[customDelimiters.Length + defaultDelimiters.Length];
        customDelimiters.CopyTo(mergedDelimiters, 0);
        defaultDelimiters.CopyTo(mergedDelimiters, customDelimiters.Length);
        return mergedDelimiters;
    }
}