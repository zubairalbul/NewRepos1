using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text;

namespace UniversityTask
{
    internal class Program
    {
        HashSet<string> myhash = new HashSet<string>();
        static string filePath = "C:\\Users\\Codeline User\\Desktop\\Zubair2\\UniversityTask\\Dictionary.txt";//Dictionary Data
        static Dictionary< string, HashSet<string>> mydict1 = new Dictionary< string, HashSet<string>>();
        static void Main(string[] args)
        {
            AddingCourse();
            ReadingDictionary();
            AppendDictionary();
        }
        static void AddingCourse()
        {
            Console.WriteLine("Please Enter The Course Code Example: MATH6012");
            string CourseCode=Console.ReadLine();
           

            for (int i = 0; i < mydict1.Count; i++)
            {
                if (mydict1.Keys.ElementAt(i) == CourseCode) { Console.WriteLine("the course is already exist"); }
                else 
                { 
                    mydict1.Add(CourseCode.ToUpper(), null); 
                    SavingDictionary();
                    Console.WriteLine("The course has been added successfully.");
                    break;
                }
                
            }
            
        }
        static void SavingDictionary()//Saving Dictionary data in files
        {
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (var Course in mydict1)
                        {
                            writer.WriteLine($"{Course.Key}|{Course.Value}");
                        }
                    }
                    Console.WriteLine("Dictionary saved to file successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving to file: {ex.Message}");
                }
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
                            if (parts.Length == 2)
                            {
                                mydict1.Add(parts[0], new HashSet<string> { parts[1] });
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

            int Coursenum = 0;
            sb.AppendLine($"{"Coursecode:",-20}|{"Student Name: ",-30}");
            for (int i = 0; i < mydict1.Count; i++)
            {
                Coursenum = i + 1;
                sb.AppendLine($"{mydict1.ElementAt(i).Key,-20}|{mydict1.ElementAt(i).Value,-30}");



            }
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }
    }
}
