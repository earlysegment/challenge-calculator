using System;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new StringCalculator();
        
        while (true) // Continuous input (Optional Stretch Goal)
        {
            // Prompt the user for input
            Console.WriteLine("Enter a string of numbers separated by commas or newlines (or type 'exit' to quit):");

            // Read input from the command line
            string? input = Console.ReadLine(); // Allow for null

            // Check if input is null or exit command
            if (input == null || input.ToLower() == "exit")
                break;

            // Replace literal "\n" with the actual newline character for command line input
            input = input.Replace(@"\n", "\n");

            // Call the Add method and output the result
            try
            {
                int result = calculator.Add(input); // Safe because Add now accepts nullable string
                Console.WriteLine($"The result is: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}