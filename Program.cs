using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace UniversityTask
{
    internal class Program
    {
        static List<string> myCourses = new List<string>();
        static string filePath2 = "C:\\Users\\Codeline User\\Desktop\\UniversityTask\\Waitlist.txt";
        static string filePath = "C:\\Users\\Codeline User\\Desktop\\UniversityTask\\Dictionary.txt"; // Dictionary Data
        static Dictionary<string, HashSet<string>> mydict1 = new Dictionary<string, HashSet<string>>();
        static Dictionary<string, List<string>> waitlists = new Dictionary<string, List<string>>(); // Store waitlists for each course

        static void Main(string[] args)
        {
            ReadingDictionary();
            ReadWaitlist();
            try
            {
                bool repeat = true;
                do
                {
                    Console.WriteLine("Please Select an Option:\n1. Add a Course. \n2. Enroll an Student to the course.\n3. Remove Course.\n4. Remove Student.\n5. Waitinglist.\n6. Exit");
                    int Choice = int.Parse(Console.ReadLine());

                    switch (Choice)
                    {
                        case 1:
                            AppendDictionary();
                            AddingCourse();
                            break;
                        case 2:
                            AddingStudent();
                            break;
                        case 3:
                            RemoveCourse();
                            break;
                        case 4:
                            RemoveStudent();
                            break;
                        case 5:
                            ShowWaitlist();
                            break;
                        case 6:
                            repeat = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            repeat = true;
                            break;
                    }
                } while (repeat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void AddingCourse()
        {
            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("Please Select an Option. \n1. Add A Course.\n2. Exit.");
                int Choice2 = int.Parse(Console.ReadLine());

                if (Choice2 == 1)
                {
                    Console.Write("Please Enter The Course Code Example: MATH6012: ");
                    string CourseCode = Console.ReadLine();

                    // Check if course already exists
                    if (myCourses.Contains(CourseCode.ToUpper()))
                    {
                        Console.WriteLine("The course already exists.");
                        continue;
                    }

                    // Add course to dictionary and course list
                    myCourses.Add(CourseCode.ToUpper());
                    mydict1.Add(CourseCode.ToUpper(), new HashSet<string>());

                    Console.Write("Enter student names (separated by commas): ");
                    string studentNames = Console.ReadLine();

                    // Split student names by commas and add them to the dictionary
                    foreach (string studentName in studentNames.Split(','))
                    {
                        mydict1[CourseCode.ToUpper()].Add(studentName.Trim());
                    }

                    SavingDictionary();

                    Console.WriteLine("The course has been added successfully.");

                    // Ask if the user wants to add another course
                    Console.Write("Do you want to add another course? (y/n): ");
                    string answer = Console.ReadLine().ToLower();
                    if (answer != "y")
                    {
                        repeat = false;
                    }
                }
                else
                {
                    repeat = false;
                }
            }
        }

        static void SavingDictionary()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    string json = JsonSerializer.Serialize(mydict1);
                    writer.Write(json);
                }

                Console.WriteLine("Dictionary saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        static void ReadingDictionary()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string json = reader.ReadToEnd();
                        mydict1 = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(json);
                    }

                    Console.WriteLine("Dictionary loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }

        static void AppendDictionary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{"Coursecode:",-20}|{"Student Name: ",-30}");

            foreach (var (CourseCode, Students) in mydict1)
            {
                sb.AppendLine($"{CourseCode,-20}|{string.Join(",", Students),-30}");
            }

            Console.WriteLine(sb.ToString());
        }

        static void RemoveCourse()
        {
            AppendDictionary();
            Console.WriteLine("Please Enter The Name Of the Course To Remove: ");
            string course = Console.ReadLine();

            // Check if course already exists
            for (int i = 0; i < mydict1.Count; i++)
            {
                if (myCourses.Contains(course) && mydict1.Values == null)
                {
                    mydict1.Remove(course.ToUpper());
                }
                else
                {
                    Console.WriteLine("The course cannot be removed since students are enrolling on it");
                    break;
                }
            }
        }

        static void AddingStudent()
        {
            bool continueAdding = false;
            while (!continueAdding)
            {
                Console.WriteLine("Please Enter The Student Name");
                string studentName = Console.ReadLine();

                if (mydict1.Count == 0)
                {
                    Console.WriteLine("No courses available. Please add courses first.");
                    continue; // Continue to the next iteration of the outer loop
                }

                // Display available courses
                Console.WriteLine("\nAvailable Courses:");
                foreach (var course in mydict1.Keys)
                {
                    Console.WriteLine(course);
                }

                Console.WriteLine("\nSelect a course to enroll the student (enter course code):");
                string chosenCourse = Console.ReadLine().ToUpper();

                if (!mydict1.ContainsKey(chosenCourse))
                {
                    Console.WriteLine("Invalid course code. Please try again.");
                    continue; // Continue to the next iteration of the outer loop
                }

                // Add multiple students to the chosen course
                if (mydict1[chosenCourse].Count >= 8)
                {
                    // Course is full, add student to waitlist
                    if (!waitlists.ContainsKey(chosenCourse))
                    {
                        waitlists[chosenCourse] = new List<string>();
                    }
                    waitlists[chosenCourse].Add(studentName);
                    AppendWaitlist();
                    Console.WriteLine($"Course {chosenCourse} is full. {studentName} added to waitlist.");
                }
                else
                {
                    mydict1[chosenCourse].Add(studentName);
                    Console.WriteLine($"Student {studentName} successfully added to course {chosenCourse}.");
                }

                Console.Write("Do you want to add another student to this course: \n1.Yes \n2. No: ");
                int answer = int.Parse(Console.ReadLine());
                if (answer == 1)
                {
                    continueAdding = false;
                }
                else
                {
                    continueAdding = true;
                }
            }

            SavingDictionary();
        }

        static void RemoveStudent()
        {
            Console.Write("Enter The Student Name want to Remove:");
            string StudentRemove = Console.ReadLine();
            for (int i = 0; i < mydict1.Count; i++)
            {
                // ... (implementation for removing a student)
            }
        }

        static void ShowWaitlist()
        {
            if (waitlists.Count == 0)
            {
                Console.WriteLine("No waitlists exist for any courses.");
            }
            else
            {
                Console.WriteLine("\nWaitlists:");
                foreach (var (courseCode, waitlist) in waitlists)
                {
                    Console.WriteLine($"Course: {courseCode}");
                    Console.WriteLine("Waitlisted Students:");
                    foreach (var studentName in waitlist)
                    {
                        Console.WriteLine(studentName);
                    }
                    Console.WriteLine();
                }
            }
        }
        static void ReadWaitlist()
        {
            {
                try
                {
                    if (File.Exists(filePath2))
                    {
                        using (StreamReader reader = new StreamReader(filePath2))
                        {
                            string json = reader.ReadToEnd();
                            waitlists = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                        }

                        Console.WriteLine("Dictionary loaded from file successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading from file: {ex.Message}");
                }
            }
        }
        static void SavingWaitlist()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath2))
                {
                    string json = JsonSerializer.Serialize(waitlists);
                    writer.Write(json);
                }

                Console.WriteLine("student saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        static void AppendWaitlist()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{"Coursecode:",-20}|{"Student Name: ",-30}");

            foreach (var (CourseCode, Students) in waitlists)
            {
                sb.AppendLine($"{CourseCode,-20}|{string.Join(",", Students),-30}");
            }

            Console.WriteLine(sb.ToString());
        }
    }
}