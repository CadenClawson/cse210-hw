using System;

public class Program
{
    // DisplayWelcome - Displays the message, "Welcome to the Program!"
    public static void DisplayWelcome()
    {
        Console.WriteLine("Welcome to the Program!");
    }

    // PromptUserName - Asks for and returns the user's name (as a string)
    public static string PromptUserName()
    {
        Console.Write("Please enter your name: ");
        return Console.ReadLine();
    }

    // PromptUserNumber - Asks for and returns the user's favorite number (as an integer)
    public static int PromptUserNumber()
    {
        Console.Write("Please enter your favorite number: ");
        return int.Parse(Console.ReadLine());
    }

    // SquareNumber - Accepts an integer as a parameter and returns that number squared (as an integer)
    public static int SquareNumber(int number)
    {
        return number * number;
    }

    // DisplayResult - Accepts the user's name and the squared number and displays them.
    public static void DisplayResult(string userName, int squaredNumber)
    {
        Console.WriteLine($"{userName}, the square of your number is {squaredNumber}");
    }

    // Main function - Calls each of the above functions
    public static void Main(string[] args)
    {
        DisplayWelcome();
        string name = PromptUserName();
        int favoriteNumber = PromptUserNumber();
        int squaredFavNumber = SquareNumber(favoriteNumber);
        DisplayResult(name, squaredFavNumber);
    }
}
