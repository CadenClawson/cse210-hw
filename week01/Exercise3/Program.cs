using System;

class Program
{
    static void Main(string[] args)
    {
        Random numberGenerator = new Random();
        int magic = numberGenerator.Next(1, 101);

        int guess = -1;

        while (guess != magic)
        {
            Console.Write("What is your guess? ");
            guess = int.Parse(Console.ReadLine());

            if (magic > guess)
            {
                Console.WriteLine("Higher");
            }
            else if (magic < guess)
            {
                Console.WriteLine("Lower");
            }
            else
            {
                Console.WriteLine("Congratulations you guessed it!");
            }
    }
}
