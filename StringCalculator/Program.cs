using System;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new StringCalculator();

        // Handle Ctrl+C to exit the loop gracefully
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("\nCtrl+C detected. Exiting the program...");
            Environment.Exit(0); // Immediately terminate the program
        };

        Console.WriteLine("String Calculator");
        Console.WriteLine("Enter a string of numbers separated by commas or newlines.");
        Console.WriteLine("You can also use custom delimiters in the format: //[delimiter]\\n[numbers].");
        Console.WriteLine("Type 'exit' or press Ctrl+C to quit.");

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
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}