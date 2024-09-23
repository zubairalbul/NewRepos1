using System.Collections.Generic;
using System.Text;

namespace UniversityTask
{
    internal class Program
    {
        static HashSet<string> myhash = new HashSet<string>();
        static string filePath = "C:\\Users\\Codeline User\\Desktop\\UniversityTask\\Dictionary.txt"; // Dictionary Data
        static Dictionary<string, HashSet<string>> mydict1 = new Dictionary<string, HashSet<string>>();

        static void Main(string[] args)
        {
            bool flag=false;
            Console.WriteLine("Please Select an Option:\n1. Add a Course. \n2. Enroll an Student to the course.");
            int Choice = int.Parse(Console.ReadLine());
            switch (Choice)
            {
                case 1:
                    {
                        AddingCourse();
                        flag = !true;
                        break;
                    }
                case 2:
                    {
                        AddingStudent();
                        flag = !true;
                        break;
                    }
                default:
                    flag = true;
                    return;

                    //AddingCourse();
                    //ReadingDictionary();
                    //AppendDictionary();
                    //AddingStudent();
            }
        }

            static void AddingCourse()
            {
                Console.Write("Please Enter The Course Code Example: MATH6012: ");
                string CourseCode = Console.ReadLine();

                if (mydict1.ContainsKey(CourseCode.ToUpper()))
                {
                    Console.WriteLine("The course already exists.");
                    return;
                }

                mydict1.Add(CourseCode.ToUpper(), new HashSet<string>());
                SavingDictionary();
                Console.WriteLine("The course has been added successfully.");
            }

            static void SavingDictionary()
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (var (CourseCode, Students) in mydict1)
                        {
                            writer.WriteLine($"{CourseCode}|{string.Join(",", Students)}");
                        }
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
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                var parts = line.Split('|');
                                if (parts.Length >= 2)
                                {
                                    string courseCode = parts[0];
                                    HashSet<string> students = new HashSet<string>(parts.Skip(1).First().Split('|'));
                                    mydict1[courseCode] = students;
                                }
                            }
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
                    sb.AppendLine($"{CourseCode,-20}|{string.Join(", ", Students),-30}");
                }

                Console.WriteLine(sb.ToString());
            }

            static void AddingStudent()
            {
                Console.WriteLine("Please Enter The Student Name");
                string studentName = Console.ReadLine();

                if (mydict1.Count == 0)
                {
                    Console.WriteLine("No courses available. Please add courses first.");
                    return;
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
                    return;
                }

                // Add multiple students to the chosen course
                bool continueAdding = true;
                while (continueAdding)
                {
                    mydict1[chosenCourse].Add(studentName);
                    Console.WriteLine($"Student {studentName} successfully added to course {chosenCourse}.");

                    Console.Write("Do you want to add another student to this course? (1.Yes \n2. No): ");
                    int answer = int.Parse(Console.ReadLine());
                    if (answer == 1)
                    {
                        continueAdding = true;
                    }
                    else
                        break;
                }

                SavingDictionary();
            }
        }
    }
