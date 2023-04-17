using System;
using System.Collections.Generic;
using System.Linq;

class Program
{

    //OOP is overrated.

    static List<Course> AllCourses;
    static List<Student> AllStudents;

    #region Structs
    struct Student
    {
        public string Forename;
        public string Lastname;
        public int ID;
        public long contactNumber; //this is exclusivley so findstudent could be seperate
        public List<Course> EnrolledCourses;
    }

    struct Course
    {
        public string CourseName;
        public List<Student> Students;
    }
    #endregion

    static void Main(string[] args)
    {
        Console.Write(
        @"  _____  ______ _____    _______     _______ _______ ______ __  __ " + "\n" +
        @" |  __ \|  ____/ ____|  / ____\ \   / / ____|__   __|  ____|  \/  |" + "\n" +
        @" | |__) | |__ | |  __  | (___  \ \_/ / (___    | |  | |__  | \  / |" + "\n" +
        @" |  _  /|  __|| | |_ |  \___ \  \   / \___ \   | |  |  __| | |\/| |" + "\n" +
        @" | | \ \| |___| |__| |  ____) |  | |  ____) |  | |  | |____| |  | |" + "\n" +
        @" |_|  \_\______\_____| |_____/   |_| |_____/   |_|  |______|_|  |_|" + "\n\n"
        );

        AllCourses = new List<Course>();
        AllStudents = new List<Student>();

        GenerateTestInformation();

        while (true)
        {
            MainMenu();
        }
    }

    static void GenerateTestInformation()
    {
        NewCourse("ERROR");
        NewCourse("Maths");
        NewCourse("Science");
        NewCourse("English");

        Student s = new Student();
        s.Forename = "Jimmy";
        s.Lastname = "Johnson";
        s.ID = 40139037;
        s.contactNumber = 02084220909;
        s.EnrolledCourses = new List<Course>();
        EnrollStudent(s, "Maths");
    }

    static void MainMenu()
    {
        //https://patorjk.com/software/taag/#p=display&f=Big&t=REG%20SYSTEM
        Scroll(
            "\n\t1) View All Courses" +
            "\n\t2) Search For Student" + //phone, id or course name
            "\n\t3) Display All Students From Course" + //"-	Display all student IDs, first names, last names, based on course" 
            "\n\t4) Delete Student Record" +
            "\n\t5) Update Student Record" +
            "\n\t6) Exit");

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
          case 4: //Delete student record
            Console.WriteLine("Thursday");
            break;
          case 5: //Update student record
            Console.WriteLine("Friday");
            break;
          case 6: //Exit
            Environment.Exit(0);
            break;
        }
    }

    static Course NewCourse(string name)
    {
        Course newD = new Course();
        newD.CourseName = name;
        newD.Students = new List<Student>();

        AllCourses.Add(newD);

        return newD;
    }
    static Student NewStudent()
    {
        Student newS = new Student();
        string input = "";

        bool Flag_Forename = false;
        bool Flag_Lastname = false;
        bool Flag_ID = false;
        bool Flag_ContactNumber = false;

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
            Console.WriteLine("ID:    ");
            input = Console.ReadLine();
            Flag_ID = ValidateIntInput(input, 10000000, 99999999);
        }
        newS.ID = int.Parse(input);

        while (!Flag_ContactNumber) {
            Console.WriteLine("Contact Number:    ");
            input = Console.ReadLine();
            Flag_ContactNumber = ValidatePhoneNumberInput(input);
        }
        newS.contactNumber = int.Parse(new(input.Where(char.IsDigit).ToArray()));

        //TODO a loop for adding courses to the student

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
            System.Diagnostics.Debug.WriteLine("String Validation Failed (Empty)");
            return false;
        }

        if (input.Contains(" "))
        {
            System.Diagnostics.Debug.WriteLine("String Validation Failed (Contains Whitespace)");
            return false;
        }

        foreach (char c in input)
        {
            if (!char.IsLetter(c))
            {
                System.Diagnostics.Debug.WriteLine("String Validation Failed (Contains Non-Letter Characters)");
                return false;
            }
        }
        System.Diagnostics.Debug.WriteLine("String Validation Passed");
        return true;
    }

    //snuck in a range check. doing it in here because it's an easily accessable version of input already cleaned as int.
    static bool ValidateIntInput(string input, int? lower = null, int? upper = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            System.Diagnostics.Debug.WriteLine("Int Validation Failed (Empty)");
            return false;
        }

        if (int.TryParse(input, out int result))
        {
            if (!(upper == null && lower == null)) //this can absolutly be simplified
            {
                if(result >= lower && result <= upper)
                {
                    System.Diagnostics.Debug.WriteLine("Int & Range Validation Passed");
                    return true;
                }
                System.Diagnostics.Debug.WriteLine("Int & Range Validation Failed");
                return false;
            }
            System.Diagnostics.Debug.WriteLine("Int Validation Passed");
            return true;
        }
        System.Diagnostics.Debug.WriteLine("Int Validation Failed");
        return false;
    }
    static bool ValidatePhoneNumberInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            System.Diagnostics.Debug.WriteLine("Phone Validation Failed (Empty)");
            return false;
        }

        //removes non-digits incase they put + or whitespace
        string cleanedInput = new(input.Where(char.IsDigit).ToArray());

        //exactly 10 digits
        if (cleanedInput.Length == 10)
        {
            System.Diagnostics.Debug.WriteLine("Phone Validation Passed");
            return true;
        }

        System.Diagnostics.Debug.WriteLine("Phone Validation Failed");
        return false;
    }
    #endregion

    static void FindStudentMenu()
    {
        Scroll("Find From:");
        Scroll(
        "\n\t1) ID" +
        "\n\t2) Phone Number" +
        "\n\t3) Course Name"
        );

        int input = SafeIntInput("> ", 1, 3);

        switch (input)
        {
            case 1: //ID
                FindStudent(SafeIntInput("ID: ", 10000000, 99999999));
                break;
            case 2: //Phone Number
                //FindStudent(Convert.ToInt64(SafeIntInput("ID: ", 1000000000, 9999999999)));
                break;
            case 3: //Course Name
                //Console.WriteLine("Wednesday");
                break;
        }
    }

    #region Find Student

    static void FindStudent(int ID)
    {
        foreach (var Student in AllStudents)
        {
            if (ID == Student.ID)
            {
                DisplayStudent(Student);
            }
        }
    }
    static void FindStudent(long contactNumber)
    {
        foreach (var Student in AllStudents)
        {
            if (contactNumber == Student.contactNumber)
            {
                DisplayStudent(Student);
            }
        }
    }

    static void FindStudent(string courseName)
    {
        Course d = FindCourseByName(courseName);
        AllStudentsInCourse(d);
    }

    #endregion
}

/*
blockchain
 made by a mathmetician from the 70s
 legers, 1st and 2nd branches
 magic hashes verifying eachother
 money laundering (monero)

 literally the whole point of crypto is hiding income (not tax evasion trust me)

5g
 4g but faster
 iot
 hacking peoples doorbel
 "smart microwave"
  cognative dissonance
   programmers knows it's wank
   regular people slurp it up
   botnet
   hackers go brr

quantum
 a model of comuting ivented by (dead) richard feynman et al, promising to use superposed entanglement between cubits (subatomic)
 deomstrated in a lab, theoretical physisits ate it up
 logic gates to control interactions -> computer?
 usually phosperous & silicon because of resonance and other magic
 need billions, can actually do barely 20
 THEORISTS can use it to sort list
 fourier transform (specialist engineering shit)
 peter schwarz says a working quantum computer could do semi-prime factorisation, semi-descreet (?) SIGNIFICANTLY FASTER than regular computers
 comprimise significant amounts of other garbage 
 biochemistry would benifit massivley from huge sorting
 hadamard gates?
 gordon moore
 if you lose track of the cubits the algorithm just stops

ai
 3rd gen neural net
 recursive neural net (long-short term memory)
 2016-2017 it got better (trasformative neural net) qualitive nodes
  everything caught fire
  generally learns much faster than long-short
 biocomputing
 cybersecurity
 chinese room thought experiment
 tesla archetecture

 protien folding problem
  predicting the shape of a protien by it's formula
  stuck on long-short memory
  went from 20 -> 90%
  US medical lobbying is 725.1 million USD (Statistica, 2022)




*/