using System;
using System.Collections.Generic;
using System.Linq;

public class NumberOperations
{
    public static void Main(string[] args)
    {
        List<int> numbers = new List<int>();
        int inputNumber;

        Console.WriteLine("Enter a list of numbers, type 0 when finished.");

        do
        {
            Console.Write("Enter number: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out inputNumber))
            {
                if (inputNumber != 0)
                {
                    numbers.Add(inputNumber);
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter an integer.");
            }
        } while (inputNumber != 0);

        if (numbers.Any())
        {
            // 1. Compute the sum
            int sum = numbers.Sum();

            // 2. Compute the average
            double average = numbers.Average();

            // 3. Find the maximum number
            int largestNumber = numbers.Max();

            Console.WriteLine($"The sum is: {sum}");
            Console.WriteLine($"The average is: {average}");
            Console.WriteLine($"The largest number is: {largestNumber}");
        }
        else
        {
            Console.WriteLine("No numbers were entered.");
        }
    }
}
