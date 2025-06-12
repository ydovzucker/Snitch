using System;

class Intel
{
    //private string firstName;
    //private string lastName;

    public static string GetFirstName()
    {
        Console.Write("Enter your first name: ");
        string firstName = Console.ReadLine()?.Trim();
        //Console.Write("Enter your last name: ");
        //lastName = Console.ReadLine()?.Trim();
        return firstName;
        
    }

    public static string GetLastName()
    {
        Console.Write("Enter your last name: ");
        string lastName = Console.ReadLine()?.Trim();
        return lastName;
    }

    //public static string GetFullName()
    //{
    //    return $"{firstName} {lastName}";
    //}

    public static string AskForIntel()
    {
        Console.Write("What intel would you like to share? ");
        string intel = Console.ReadLine();
        return intel;
    }
}
