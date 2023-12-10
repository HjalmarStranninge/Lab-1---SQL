using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            string sortOrder = (secondChoice == "1") ? "DESC" : "ASC";   

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

                            Console.WriteLine($"Student ID: {studentId}  Name: {firstName} {lastName}");
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

        // Lists all classes and lets the user pick one, then lists all the students in that class
        internal static void ShowStudentsByClass()
        {
            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.Clear();
                Console.WriteLine("\t--- CLASS LIST ---\n");
                connection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT * FROM Classes", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string className = reader["ClassName"].ToString();
                            int classId = Convert.ToInt32(reader["ClassID"]);

                            Console.WriteLine($"Class ID: {classId}, Class name: {className}");
                        }
                        
                    }
                }
                Console.Write("\nEnter Class ID to view all students: ");
                int input = int.Parse(Console.ReadLine());

                string queryString = @"
                                     SELECT  
                                     Students.StudentID, Students.FirstName, Students.LastName, Classes.ClassName
                                     FROM Students
                                     JOIN StudentsClasses ON Students.StudentID = StudentsClasses.StudentID_FK
                                     JOIN Classes ON StudentsClasses.ClassID_FK = Classes.ClassID
                                     WHERE StudentsClasses.ClassID_FK = @ClassID
                                     ORDER BY Students.FirstName";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@ClassID", input);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.Clear();

                        // Checks if there are students in the chosen class, then displays the class name first before displaying the student list.
                        if (reader.Read()) 
                        {
                            string className = reader["ClassName"].ToString();
                            Console.WriteLine($"CLASS: {className}\n");

                            do
                            {
                                string firstName = reader["FirstName"].ToString();
                                string lastName = reader["LastName"].ToString();
                                int studentId = Convert.ToInt32(reader["StudentID"]);

                                Console.WriteLine($"Student ID: {studentId}  Name: {firstName} {lastName}");
                            } while (reader.Read());
                        }
                        else
                        {
                            Console.WriteLine("No students found for the specified class.");
                        }

                        Console.WriteLine("\nPress ENTER to return to the menu");
                        Console.ReadLine();
                    }
                }
            }
        }

        // Adds new staff member to the database.
        internal static void AddStaff()
        {
            Console.Write("Enter staff members first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter staff members last name: ");
            string lastName = Console.ReadLine();

            Console.Write("What profession will the staff member have? ");
            string profession = Console.ReadLine();

            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";

            // Query is parameterized to prevent query injection.
            string insertQuery = $"INSERT INTO Staff (firstName, lastName, profession) VALUES (@FirstName, @LastName, @Profession)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Profession", profession);

                    int numRowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"New staff member added! Rows affected: {numRowsAffected}");
                    Thread.Sleep(2000);
                }
            }
        }

        // Displays a list of all staff members.
        internal static void ShowAllStaff()
        {          

            string queryString = $"SELECT * FROM Staff ORDER BY LastName";

            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.Clear();
                Console.WriteLine("\t--- STAFF LIST ---\n");
                connection.Open();
                using (SqlCommand command = new SqlCommand($"{queryString}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string profession = reader["Profession"].ToString();
                            int staffId = Convert.ToInt32(reader["StaffID"]);

                            Console.WriteLine($"Staff ID: {staffId}  Name: {firstName} {lastName} | Profession: {profession}");
                        }
                        Console.WriteLine("\nPress ENTER to return to menu");
                        Console.ReadLine();
                    }
                }
            }
        }

        // Shows average grade for a specific course.
        internal static void ShowAverageGrade()
        {
            string queryString = $"SELECT CourseName, CourseID FROM Courses";

            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.Clear();
                Console.WriteLine("\t--- COURSE LIST ---\n");
                connection.Open();
                using (SqlCommand command = new SqlCommand($"{queryString}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string courseName = reader["CourseName"].ToString();
                            int courseID = Convert.ToInt32(reader["CourseID"]);

                            Console.WriteLine($"Course ID: {courseID}  Course: {courseName} ");
                        }
                        Console.Write("\nEnter course ID to view average grade: ");                        
                    }
                }
                string input = Console.ReadLine();
                Console.Clear();

                queryString = @"
                            SELECT  
                            Courses.CourseName,
                            AVG(Grades.Grade) AS AverageGrade
                            FROM Grades
                            JOIN Courses ON Courses.CourseID = Grades.CourseID_FK
                            WHERE Grades.CourseID_FK = @CourseID
                            GROUP BY Courses.CourseName";

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@CourseID", input);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.Clear();

                        // Checks if there are grades set in the chosen course
                        if (reader.Read())
                        {
                            do
                            {
                                string courseName = reader["CourseName"].ToString();
                                string averageGrade = reader["AverageGrade"].ToString();

                                Console.WriteLine($"Course: {courseName}  Grade average: {averageGrade}");
                            } while (reader.Read());
                        }
                        else
                        {
                            Console.WriteLine("No grades have been set in the specified course.");
                        }

                        Console.WriteLine("\nPress ENTER to return to the menu");
                        Console.ReadLine();
                    }
                }
            }
        }

        // Shows all grades set last month.
        internal static void ShowGradesSetLastMonth()
        {

            //Variable for sorting out the grades set withing 30 days.
            DateTime lastMonth = DateTime.Now.AddMonths(-1);

            string queryString = @"
                            SELECT  
                            FirstName,
                            LastName,
                            Courses.CourseName,
                            Grades.Grade,
                            Grades.GradeDate
                            FROM Students
                            JOIN Grades ON Students.StudentID = Grades.StudentID_FK
                            JOIN Courses ON Courses.CourseID = Grades.CourseID_FK
                            WHERE Grades.GradeDate >= @LastMonth";

            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=SchoolDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.Clear();
                Console.WriteLine("\t--- GRADES SET LAST MONTH ---\n");
                connection.Open();
                using (SqlCommand command = new SqlCommand($"{queryString}", connection))
                {
                    command.Parameters.AddWithValue("@LastMonth", lastMonth);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"].ToString();
                            string courseName = reader["CourseName"].ToString();
                            string grade = reader["Grade"].ToString();

                            Console.WriteLine($"Name: {firstName} {lastName} | Course: {courseName}  Grade: {grade}");
                        }
                        Console.WriteLine("\nPress ENTER to return to menu");
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
