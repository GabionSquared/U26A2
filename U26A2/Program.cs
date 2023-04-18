using System;
using System.Collections.Generic;
using System.Linq;

/*Reqs

-	Add new student
-	Add new department
    -	3 already present:
        -	Business
        -	Health care
        -	Computing
-	Remove student record via ID or phone number
-	Search student record based on phone number, id, or course name (string? IEnum?)
-	Display all student IDs, first names, last names, based on course
-	Update student details


*/

//Tools > NuGet Package Manager > Package Manager Console
//dotnet restore

class Program
{
    static List<Department> AllDepartments;
    static List<Student> AllStudents;

    #region Structs
    struct Student
    {
        public string Forename;
        public string Lastname;
        public int ID;
        public int contactNumber;
        public List<Course> EnrolledCourses;
    }

    struct Course
    {
        public string CourseName;
        public List<Student> Students;
    }
    #endregion

    static void Main()
    {
        AllDepartments = new List<Department>();
        AllStudents = new List<Student>();

        MainMenu();
    }

    static void GenerateTestInformation()
    {
        NewCourse("ERROR");
        NewCourse("Business");
        NewCourse("Health Care");
        NewCourse("Computing");

        Student s = new()
        {
            Forename = "Jimmy",
            Lastname = "Johnson",
            ID = 40139037,
            contactNumber = 02084220909,
            EnrolledCourses = new List<Course>()
        };

        EnrollStudent(s, "Computing");
    }

    static void MainMenu()
    {
        //https://patorjk.com/software/taag/#p=display&f=Big&t=REG%20SYSTEM
        Scroll(
            "\n\t1) View All Courses" +
            "\n\t2) Search For Student" + //phone, id or course name
            "\n\t3) Display All Students From Course" + //"-	Display all student IDs, first names, last names, based on course" 
            "\n\t4) Add Student Record" +
            "\n\t5) Delete Student Record" +
            "\n\t6) Update Student Record" +
            "\n\t7) Exit");

        int input = SafeIntInput(">", 1, 6);

        switch (input) 
        {
            case 1: //View all courses
                DisplayAllCourses();
                break;
            case 2: //Search for student (phone, id or course name)
                FindStudentMenu();
                break;
            case 3: //Display all students from course (IDs, first names, last names)
                Console.WriteLine("Wednesday");
                break;
            case 4: //Add student record
                NewStudent();
                break;
            case 5: //Delete student record
                Console.WriteLine("Thursday");
                break;
            case 6: //Update student record
                Console.WriteLine("Friday");
                break;
            case 7: //Exit
                Environment.Exit(0);
                break;
        }
    }

    //technical
    static Course NewCourse(string name)
    {
        Course newD = new()
        {
            CourseName = name,
            Students = new List<Student>(),
        };

        AllCourses.Add(newD);

        return newD;
    }

    //graphical
    static Student NewStudent()
    {
        Student newS = new();
        string input = "";

        bool Flag_Forename = false;
        bool Flag_Lastname = false;
        bool Flag_ID = false;
        bool Flag_ContactNumber = false;
        bool Flag_Course = false;

        while (!Flag_Forename) {
            Console.WriteLine("Forename:    ");
            input = Console.ReadLine();
            Flag_Forename = ValidateStringInput(input);
        }
        newS.Forename = input;

        while (!Flag_Lastname) {
            Console.WriteLine("Lastname:    ");
            input = Console.ReadLine();
            Flag_Lastname = ValidateStringInput(input);
        }
        newS.Lastname = input;

        while (!Flag_ID) {
            Console.WriteLine("Forename:    ");
            input = Console.ReadLine();
            Flag_ID = ValidateIntInput(input);
        }
        newS.ID = int.Parse(input);

        while (!Flag_ContactNumber) {
            Console.WriteLine("Forename:    ");
            input = Console.ReadLine();
            Flag_ContactNumber = ValidatePhoneNumberInput(input);
        }
        newS.contactNumber = int.Parse(new(input.Where(char.IsDigit).ToArray()));

        //TODO a loop for adding courses to the student

        while (!Flag_Course)
        {
            Console.WriteLine("(Type NaN to exit)");
            Console.WriteLine("Course Name:    ");
            input = Console.ReadLine();
            Flag_ContactNumber = ValidateStringInput(input);

            if (input == "NaN")
            {
                Flag_Course = true;
            }
            else
            {
                try
                {
                    EnrollStudent(newS, input);
                }
                catch
                {
                    Console.WriteLine("Course not found");
                }
            }
        }
        return newS;
    }


    #region Student Utils
    static void EnrollStudent(Student s, string course)
    {
        Course d = FindCourseByName(course);
        s.EnrolledCourses.Add(d);
        d.Students.Add(s);
    }

    static void DisplayStudent(Student s)
    {
        Console.WriteLine("----------------------");
        Scroll(s.Forename + " " + s.Lastname);
        Scroll(s.ID.ToString());
        Scroll(s.contactNumber.ToString());
        Scroll("Enrolled Courses:");


        Scroll(s.ID.ToString());

        if (s.EnrolledCourses.Count == 0)
        {
            Scroll("(None found)");
        }
        else
        {
            foreach (var Course in s.EnrolledCourses)
            {
                Scroll("- " + Course.CourseName, finishTime: 0);
            }
        }
        Console.WriteLine("----------------------");
    }
    #endregion

    #region Course Utils
    static Course FindCourseByName(string course)
    {
        foreach (var Course in AllCourses)
        {
            try
            {
                if (Course.CourseName.ToLower() == course.ToLower())
                {
                    return Course;
                }
            }
            catch
            {
                Console.WriteLine("that course does not exist.");
            }
        }
        return FindCourseByName("ERROR");
    }

    static void AllStudentsInCourse(Course c)
    {
        foreach (var Student in c.Students)
        {
            DisplayStudent(Student);
        }
    }

    static void DisplayAllCourses()
    {
        Console.WriteLine();
        foreach (var Course in AllCourses)
        {
            Scroll("- " + Course.CourseName, finishTime: 0);
        }
    }
    #endregion

    #region Generic Utils

    static int SafeIntInput(string prompt = ">", int? lower = null, int? upper = null)
    {
        string input = "";
        bool inputFlag = false;

        while (!inputFlag)
        {
            Console.Write("\n\t" + prompt);
            input = Console.ReadLine();
            inputFlag = ValidateIntInput(input, lower, upper);

        }
        return int.Parse(input);
    }

    //this function has served me 6 years and it'll do 6 years more. UX is king.
    /// <summary>
    /// For making strings print one character at a time
    /// </summary>
    /// <param name="msg">The string that will be printed.<para>does not work with the "value: {0}",var method, use "value: "+var</para></param>
    /// <param name="scrollTime">Time between each character being printed in thousands of a second.</param>
    /// <param name="finishTime">Time after the string is finished in thousands of a second.</param>
    /// <param name="lineBreak">How many times \n is sent AFTER msg</param>
    /// <param name="tabs">How many times \t is sent BEFORE msg</param>
    public static void Scroll(string msg, int scrollTime = 10, int finishTime = 500, int lineBreak = 1, int tabs = 1)
    {
        for (int i = 0; i < tabs; i++)
        { //do the tabs
            Console.Write("\t");
        }
        for (int i = 0; i < msg.Length; i++)
        { //print the message
            System.Threading.Thread.Sleep(scrollTime);
            Console.Write(msg[i]);
        }
        for (int i = 0; i < lineBreak; i++)
        { //do the line breaks
            Console.Write("\n");
        }
        System.Threading.Thread.Sleep(finishTime); //end sleep
    }
    #endregion
    #region Validations (string, int, phone number, range)
    //returns true when passing the check successfully

    static bool ValidateStringInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        if (input.Contains(' '))
        {
            return false;
        }

        foreach (char c in input)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }

        return true;
    }
    static bool ValidateIntInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        if (int.TryParse(input, out int result))
        {
            // faster than counting the characters dont @ me (for an int)
            if (result >= 10000000 && result <= 99999999)
            {
                return true;
            }
        }

        return false;
    }
    static bool ValidatePhoneNumberInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        //removes non-digits incase they put + or whitespace
        string cleanedInput = new(input.Where(char.IsDigit).ToArray());

        //exactly 10 digits
        if (cleanedInput.Length == 10)
        {
            return true;
        }

        return false;
    }
    #endregion

    struct Student
    {
        public string Forename;
        public string Lastname;
        public int ID;
        public int contactNumber;
        public List<Department> EnrolledDepartments;
    }

    #region Find Student

    static Student FindStudent_ID(int ID)
    {
        foreach (var Student in AllStudents)
        {
            if (ID == Student.ID)
            {
                return Student;
            }
        }
        return new Student();
    }
    static Student FindStudent_Contact(int contactNumber)
    {
        foreach (var Student in AllStudents)
        {
            if (contactNumber == Student.contactNumber)
            {
                return Student;
            }
        }
        return new Student();
    }

    static void FindStudent_Course(string courseName)
    {
        Console.Write(
        @"  _____  ______ _____    _______     _______ _______ ______ __  __ " + "\n" +
        @" |  __ \|  ____/ ____|  / ____\ \   / / ____|__   __|  ____|  \/  |" + "\n" +
        @" | |__) | |__ | |  __  | (___  \ \_/ / (___    | |  | |__  | \  / |" + "\n" +
        @" |  _  /|  __|| | |_ |  \___ \  \   / \___ \   | |  |  __| | |\/| |" + "\n" +
        @" | | \ \| |___| |__| |  ____) |  | |  ____) |  | |  | |____| |  | |" + "\n" +
        @" |_|  \_\______\_____| |_____/   |_| |_____/   |_|  |______|_|  |_|" + "\n\n"
        );
        Console.WriteLine(
            "\n\t1) View all courses" +
            "\n\t2) Search for student" + //phone, id or course name
            "\n\t3) Display all students from course" + //"-	Display all student IDs, first names, last names, based on course" 
            "\n\t4) Delete student record" +
            "\n\t5) Update student record");
    }

}

//https://patorjk.com/software/taag/#p=display&f=Big&t=REG%20SYSTEM