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

        // Method showing all students. User selects in what order the information is displayed.
        internal static void ShowAllStudents()
        {
            Console.WriteLine("Would you like the students to be ordered by [1]: First name, or [2]: Last name?");
            Console.Write("Enter your choice, 1/2: ");

            string firstChoice = Console.ReadLine();
            if (firstChoice != "1" && firstChoice != "2")
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

            // The users choices are injected into the SQL-query string which will be used to fetch information from the database.
            string orderByColumn = (firstChoice == "1") ? "FirstName" : "LastName";
            string sortOrder = (secondChoice == "1") ? "DESC" : "ASC";                  // Parameterize this later for injection prevention.

            string queryString = $"SELECT * FROM Students ORDER BY {orderByColumn} {sortOrder}";

            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.Clear();
                Console.WriteLine("\t--- STUDENT LIST ---\n");
                connection.Open();
                using (SqlCommand command = new SqlCommand($"{queryString}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            int studentId = Convert.ToInt32(reader["StudentID"]);

                            Console.WriteLine($"Student ID: {studentId}, Name: {firstName} {lastName}");
                        }
                        Console.WriteLine("\nPress ENTER to return to menu");
                        Console.ReadLine();
                    }
                }
            }
        }

        // Method for adding new students to the database.
        internal static void AddStudent()
        {
            Console.Write("Enter student first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter student last name: ");
            string lastName = Console.ReadLine();

            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";

            // Query is parameterized to prevent query injection.
            string insertQuery = $"INSERT INTO Students (firstName, lastName) VALUES (@FirstName, @LastName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);

                    int numRowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"New student added! Rows affected: {numRowsAffected}");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
