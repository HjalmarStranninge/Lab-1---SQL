namespace Lab_1___SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool runMenu = true;
            while (runMenu)
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

                switch(menuChoice )
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "8":
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input. Try again.");
                        Thread.Sleep(2000);
                        break;
                }
            }
            
        }
    }
}