using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1___SQL
{
    internal static class UserFunctions
    {
        internal static void ShowAllStudents()
        {
            Console.WriteLine("Would you like the students to be ordered by [1]: First name, or [2]: Last name?");
            Console.Write("Enter your choice, 1/2: ");

            string firstChoice = Console.ReadLine();
            if(firstChoice != "1" && firstChoice != "2")
            {
                Console.Clear();
                Console.WriteLine("Invalid input");
            }

            Console.WriteLine("\nWould you like the students to be listed in a [1]: Descending order, or [2]: Ascending order?");
            Console.Write("Enter your choice, 1/2: ");

            string secondChoice = Console.ReadLine();
            if (secondChoice != "1" && secondChoice != "2")
            {
                Console.Clear();
                Console.WriteLine("Invalid input");
            }

            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
            }
        } 
    }
}
