using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter your grade percentage: ");
        int gradePercentage = int.Parse(Console.ReadLine());

        char letterGrade;

        if(gradePercentage >= 90)
        {
            letterGrade = 'A';
        }
        else if (gradePercentage >= 80);
        {
            letterGrade = 'B';
        }
        else if (gradePercentage >= 70);
        {
            letterGrade = 'C';
        }
        else if (gradePercentage >= 60);
        {
            letterGrade = 'D';
        }
        else
        {
            letterGrade = 'F';
        }

        Console.Writeline("Your letter grade is: {letterGrade}");

        if (gradePercentage >= 70)
        {
            Console.Writeline("Congratulations! You passed the course.");
        }
        else
        {
            Console.Writeline("Don't worry, you can do better next time. Keep practicing!");
        }
    }
}
