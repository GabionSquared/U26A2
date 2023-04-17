using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<Department> AllDepartments;
    static List<Student> AllStudents;

    static void Main(string[] args)
    {
        AllDepartments = new List<Department>();
        AllStudents = new List<Student>();

        MainMenu();
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

        return newS;
    }

    #region checks
    static bool ValidateStringInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        if (input.Contains(" "))
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

    struct Department
    {
        public string DepartmentName;
        public List<Student> Students;
    }


    static void MainMenu()
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

        string _input = "";
        bool inputFlag = false;

        while (!inputFlag)
        {
            Console.WriteLine("Forename:    ");
            _input = Console.ReadLine();
            inputFlag = ValidateIntInput(_input);
        }
        int input = int.Parse(_input);
    }

}

//https://patorjk.com/software/taag/#p=display&f=Big&t=REG%20SYSTEM

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