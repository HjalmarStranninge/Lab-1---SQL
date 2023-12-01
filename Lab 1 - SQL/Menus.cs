using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1___SQL
{
    //Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True
    internal static class Menus
    {
        // Runs the main menu, returns false to exit app.
        internal static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("\tWelcome to SchoolNET");
            Console.WriteLine("     What would you like to do?");
            Console.WriteLine();

            Console.WriteLine($"[1]: List all students");
            Console.WriteLine($"[2]: List all students by specific class");
            Console.WriteLine($"[3]: Add new staff member");
            Console.WriteLine($"[4]: Show all staff members");
            Console.WriteLine($"[5]: Show all grades set last month");
            Console.WriteLine($"[6]: Show average grade per course");
            Console.WriteLine($"[7]: Add new student");
            Console.WriteLine($"[8]: Exit\n");

            Console.Write("Choose an option: ");
            string menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":

                    Console.Clear();
                    UserFunctions.ShowAllStudents();
                    return true;

                case "2":
                    return true;
                case "3":
                    return true;
                case "4":
                    return true;
                case "5":
                    return true;
                case "6":
                    return true;
                case "7":
                    return true;
                case "8":
                    Console.Clear();
                    Console.WriteLine("Exiting application...");
                    Thread.Sleep(2000);
                    return false;
                    
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input. Try again.");
                    Thread.Sleep(2000);
                    return true;
                   
            }
        }
    }
}
