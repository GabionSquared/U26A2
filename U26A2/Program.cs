using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    //OOP is overrated.

    static List<Course> AllCourses;
    static List<Student> AllStudents;

    #region Structs
    struct Student
    {
        public string Forename;
        public string Lastname;
        public ulong ID;
        public ulong contactNumber;
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
        NewCourse("Business");
        NewCourse("Health Care");
        NewCourse("Computing");

        Student a = new()
        {
            Forename = "Jimmy",
            Lastname = "Johnson",
            ID = 40139037,
            contactNumber = 02084220909,
            EnrolledCourses = new List<Course>()
        };
        AllStudents.Add(a);
        EnrollStudent(a, "Business");

        Student b = new()
        {
            Forename = "Hagen",
            Lastname = "Key",
            ID = 65123516,
            contactNumber = 08745847565,
            EnrolledCourses = new List<Course>()
        };
        AllStudents.Add(b);
        EnrollStudent(b, "Health Care");

        Student c = new()
        {
            Forename = "Kendrick",
            Lastname = "Catalano",
            ID = 16561561,
            contactNumber = 04685468468,
            EnrolledCourses = new List<Course>()
        };
        AllStudents.Add(c);
        EnrollStudent(c, "Computing");
    }

    static void MainMenu()
    {
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        //https://patorjk.com/software/taag/#p=display&f=Big&t=REG%20SYSTEM
        Scroll(
            "\n\t1) View All Courses" +
            "\n\t2) View All Students" +
            "\n\t3) Search For Student" + //phone, id or course name
            "\n\t4) Display All Students From Course" + //"-	Display all student IDs, first names, last names, based on course" 
            "\n\t5) Add Student Record" +
            "\n\t6) Delete Student Record" +
            "\n\t7) Update Student Record" +
            "\n\t8) Exit" +
            "\n");

        ulong input = SafeIntInput(">", 1, 8);

        switch (input) 
        {
            case 1: //View all courses
                DisplayAllCourses();
                break;
            case 2: //View all courses
                DisplayAllStudents();
                break;
            case 3: //Search for student (phone, id or course name)
                FindStudentMenu();
                break;
            case 4: //Display all students from course (IDs, first names, last names)
                AllStudentsInCourse(FindCourseByName(SafeStrInput("Course Name: ")));
                break;
            case 5: //Add student record
                NewStudent();
                break;
            case 6: //Delete student record
                DeleteStudent(FindStudentForOther());
                break;
            case 7: //Update student record
                UpdateStudent(FindStudentForOther());
                break;
            case 8: //Exit
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
        Console.WriteLine();
        Student newS = new()
        {
            Forename = SafeStrInput("Forename: "),
            Lastname = SafeStrInput("Lastname: "),
            ID = SafeIntInput("ID: "),
            contactNumber = SafePhoneInput("Contact Number: "),
            EnrolledCourses = new List<Course>()
        };
        AllStudents.Add(newS);
        bool Flag_Course = false;
        string input;

        while (!Flag_Course)
        {
            Console.WriteLine("(Type EXIT to exit. "+ ((AllCourses.Count)-1).ToString() +" courses found.)");
            input = SafeStrInput("Course Name: ");

            if (input.ToUpper() == "EXIT")
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
                    Console.WriteLine("Course '" + input + "' not found");
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
        if (EqualityComparer<Student>.Default.Equals(s, default))
        {
            Scroll("(None found)");
            return;
        }

        Scroll("----------------------", finishTime: 0);
        Scroll(s.Forename + " " + s.Lastname, finishTime:0);
        Scroll("ID:\t" + s.ID.ToString(), finishTime: 0);
        Scroll("PHN:\t" + s.contactNumber.ToString(), finishTime: 0);
        Scroll("Enrolled Courses:", finishTime: 0);


        

        if (s.EnrolledCourses.Count == 0)
        {
            Scroll("(No courses found)");
        }
        else
        {
            foreach (var Course in s.EnrolledCourses)
            {
                Scroll("- " + Course.CourseName, finishTime: 0);
            }
        }
        Scroll("----------------------", finishTime: 0);

    }

    static void DeleteStudent(Student s) {
        foreach (var Course in s.EnrolledCourses)
        {
            Course.Students.Remove(s);
        }
        s.EnrolledCourses = null;

        //list.remove(T item) wasn't working, i think it's a pass by reference/item issue.
        //there SHOULD only be 1 student for each id anyway. that's the point.

        Debug.WriteLine("-------------");
        Debug.WriteLine("looking for: " + s.Forename);


        foreach (var student in AllStudents)
        {
            Debug.WriteLine(student.Forename);
            if(s.ID == student.ID)
            {
                AllStudents.Remove(student);
                return;
            }
        }
        /*
        int i = AllStudents.IndexOf(s);
        Debug.WriteLine(i);
        AllStudents.RemoveAt(i);
        */
        Debug.WriteLine("-------------");
    }

    static void UpdateStudent(Student s)
    {
        DeleteStudent(s);
        NewStudent();
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
        Console.WriteLine(c.Students.Count + " Students Found.");
        foreach (var Student in c.Students)
        {
            DisplayStudent(Student);
        }
    }

    static void DisplayAllCourses()
    {
        string s = ((AllCourses.Count) - 1).ToString() + " courses found.";
        Scroll(s);
        foreach (var Course in AllCourses)
        {
            if (!(Course.CourseName == "ERROR"))
            {
                Scroll("- " + Course.CourseName, finishTime: 0);
            }
        }
    }
    static void DisplayAllStudents()
    {
        string s = (AllStudents.Count).ToString() + " students found.";
        Scroll(s);
        foreach (var Student in AllStudents)
        {
            DisplayStudent(Student);
        }
    }
    #endregion

    #region Validations & Safe Inputs (string, int, phone number, range)
    //returns true when passing the check successfully

    //STRINGS
    static bool ValidateStringInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Debug.WriteLine("String Validation Failed (Empty)");
            return false;
        }

        if (input.Contains(' '))
        {
            Debug.WriteLine("String Validation Failed (Contains Whitespace)");
            return false;
        }

        foreach (char c in input)
        {
            if (!char.IsLetter(c))
            {
                Debug.WriteLine("String Validation Failed (Contains Non-Letter Characters)");
                return false;
            }
        }
        Debug.WriteLine("String Validation Passed");
        return true;
    }
    static string SafeStrInput(string prompt = ">")
    {
        string input = "";
        bool inputFlag = false;

        while (!inputFlag)
        {
            Console.Write("\t" + prompt);
            input = Console.ReadLine();
            inputFlag = ValidateStringInput(input);

        }
        return input;
    }

    //INTS
    //snuck in a range check. doing it in here because it's an easily accessable version of input already cleaned as int.
    static bool ValidateIntInput(string input, ulong? lower = null, ulong? upper = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            Debug.WriteLine("Int Validation Failed (Empty)");
            return false;
        }

        if (ulong.TryParse(input, out ulong result))
        {
            if (!(upper == null && lower == null)) //this can absolutly be simplified
            {
                if(result >= lower && result <= upper)
                {
                    Debug.WriteLine("Int & Range Validation Passed");
                    return true;
                }
                Debug.WriteLine("Int & Range Validation Failed");
                return false;
            }
            Debug.WriteLine("Int Validation Passed");
            return true;
        }
        Debug.WriteLine("Int Validation Failed");
        return false;
    }
    static ulong SafeIntInput(string prompt = ">", ulong? lower = null, ulong? upper = null)
    {
        string input = "";
        bool inputFlag = false;

        while (!inputFlag)
        {
            Console.Write("\t" + prompt);
            input = Console.ReadLine();
            inputFlag = ValidateIntInput(input, lower, upper);

        }
        return ulong.Parse(input);
    }

    //PHONES
    static bool ValidatePhoneNumberInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Debug.WriteLine("Phone Validation Failed (Empty)");
            return false;
        }

        //removes non-digits incase they put + or whitespace
        string cleanedInput = new(input.Where(char.IsDigit).ToArray());

        if (cleanedInput.Length == 0)
        {
            Debug.WriteLine("Phone Validation Failed (no int)");
            return false;
        }

        if (cleanedInput[0] != '0')
        {
            Debug.WriteLine("Phone Validation Failed (no 0)");
            Console.WriteLine("Please start with a 0");
            return false;
        }

        //exactly 10 digits
        if (cleanedInput.Length == 11)
        {
            Debug.WriteLine("Phone Validation Passed");
            return true;
        }

        Debug.WriteLine("Phone Validation Failed");
        return false;
    }
    static ulong SafePhoneInput(string prompt = ">")
    {
        string input = "";
        bool inputFlag = false;

        while (!inputFlag)
        {
            Console.Write("\t" + prompt);
            input = Console.ReadLine();
            inputFlag = ValidatePhoneNumberInput(input);

        }

        return ulong.Parse(input.Where(char.IsDigit).ToArray());
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

    #region Find Student
    static void FindStudentMenu()
    {
        Scroll("Find From:");
        Scroll(
        "\n\t1) ID" +
        "\n\t2) Phone Number" +
        "\n\t3) Course Name"
        );

        ulong input = SafeIntInput("> ", 1, 3);

        switch (input)
        {
            case 1: //ID
                DisplayStudent(FindStudent_ID(SafeIntInput("ID: ", 10000000, 99999999)));
                break;
            case 2: //Phone Number
                DisplayStudent(FindStudent_Contact(SafePhoneInput("Contact Number: ")));
                break;
            case 3: //Course Name
                FindStudent_Course(SafeStrInput("Course Name: "));
                break;
        }
    }
    static Student FindStudentForOther()
    {
        Scroll("Find From:");
        Scroll(
        "\n\t1) ID" +
        "\n\t2) Phone Number"
        );

        ulong input = SafeIntInput("> ", 1,2);

        return input switch
        {
            //ID
            1 => FindStudent_ID(SafeIntInput("ID: ", 10000000, 99999999)),
            //Phone Number
            2 => FindStudent_Contact(SafePhoneInput("Contact Number: ")),
            _ => default,
        };
    }

    static Student FindStudent_ID(ulong ID)
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

    static Student FindStudent_Contact(ulong contactNumber)
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