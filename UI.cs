using System;
using System.Collections.Generic;
using System.Linq;
using CivSem1Challenge2_RegistrationSystem.helpers;
using CivSem1Challenge2_RegistrationSystem.models;

using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace CivSem1Challenge2_RegistrationSystem
{
    public class UI
    {
        
        public List<Course> Courses { get; set; }
        public List<Student> Students { get; set; }
        public UI() {
            DataHandler dh = new DataHandler();
            this.Courses = dh.GetCourses();
            this.Students = dh.GetStudents();
            TopMenu();
        }

        public void TopMenu() {
            Console.WriteLine("Welcome to Dod&Gy Student Reg system - Alpha ver");

            Console.WriteLine("1. Print the names and courseNo of the courses");
            Console.WriteLine("2. Get the number of students from course given CourseNo");
            Console.WriteLine("3. Print the name of a student");
            Console.WriteLine("4. Print amount of students in the system");
            Console.WriteLine("5. Print number of students enrolled into valid courses");
            Console.WriteLine("6. Print Add a student");
            Console.WriteLine("7. Print all students who first registered on a given year and a doing a given course");
            Console.WriteLine("8. (optional) Print a list of students NOT enrolled into valid courses");
            Console.WriteLine("9. (optional) Print the oldest student - StudentNo");
            Console.WriteLine("10. (optional) Write the current state of the system back to csv files (save)");
            System.Console.WriteLine("x. Exit");

            var input = Console.ReadLine();

            switch(input) {

                case "1":
                    //TO(Done)DO: from the attribute this.Courses, print the courseNo and names of all of the courses
                    // use GetCourseDetails to do this
                    foreach (Course item in this.Courses)
                    {
                        Console.WriteLine(item.GetCourseDetails());
                    }

                    //----------
                    break;
                
                case "2":
                    System.Console.WriteLine("Please enter the course number");
                    int num;
                    while(!int.TryParse(Console.ReadLine(), out num)) {
                        System.Console.WriteLine("Invalid, enter again");
                    }

                    int numStudents = this.CourseGetNumStudents(num);
                    if(numStudents == -1) {
                        System.Console.WriteLine($"Course {num} doesn't exist");
                        break;
                    }
                    System.Console.WriteLine($"Course {num} has {numStudents} students");
                    break;

                case "3":
                    System.Console.WriteLine("Please enter a student number");
                    while(!int.TryParse(Console.ReadLine(), out num)) {
                        System.Console.WriteLine("Invalid, enter again");
                    }

                    string studentName = this.GetStudentName(num);
                    if(studentName == null) {
                        System.Console.WriteLine($"Student {num} doesn't exist");
                        break;
                    }
                    System.Console.WriteLine($"StudentNo: {num} Name: {studentName}");
                    break;

                case "4":
                    //TO(Done)DO: Print the amount of students in the system
                    // Create and call a method/function named GetNumStudents() to do this.
                    Console.WriteLine(this.GetNumStudents());
                    break;

                case "5":
                    //TO(Done)DO: Print the number of students enrolled in valid courses
                    Console.WriteLine(this.GetValidStudents());
                    break;

                case "6":
                   //TO(Done)DO: Add a student to the student List. Then add that student to a valid course
                   this.AddStudent();
                   break;

                case "7":
                    //TO(DONE)DO: Print all students who first registered on a given year and a doing a given course
                    System.Console.WriteLine("Please enter a year");
                    while(!int.TryParse(Console.ReadLine(), out num)) {
                        System.Console.WriteLine("Invalid, enter again");
                    }

                    int courseNum;
                    System.Console.WriteLine("Please enter the course number");
                    while(!int.TryParse(Console.ReadLine(), out courseNum)) {
                        System.Console.WriteLine("Invalid, enter again");
                    }

                    //TO(DONE)DO: print the students who first registered in year num and are doing course courseNum
                    Console.WriteLine(this.CourseGetNumStudentsYear(courseNum,num));

                    break;

                case "8":
                    //TO(DONE)DO: (optional CREDIT TASK) - Print a list of students who are not enrolled in a valid courses
                    // create a method/function called GetUnenrolledStudents to do this
                    this.GetUnenrolledStudents();
                    break;

                case "9":
                    //TO(Done)DO: (optional DISTINCTION TASK) - Print the oldest student's studentno
                    this.GetOldestStudent();
                    break;

                case "10":
                    //TODO: (optional HIGH DISTINCTION TASK) - Write the current state of the system back to the csv files.
                    // add a method to the DataHandler class to do this
                    this.SaveData();
                    break;

                case "x":
                    System.Console.WriteLine("Bye bye");
                    return;

                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }

            this.TopMenu();



            
        }


        //TO(Done)DO: create the GetNumStudents method/function here
        private int GetNumStudents(){
            return this.Students.Count;
        }
        //---------------------

        private string GetValidStudents(){
            string validStudents="Valid Enrolments:";
            foreach (Course item in this.Courses)
            {
                validStudents+=$"\nCourse {item.CourseNo}, Students:{item.Enrolments.Count}";
            }
            return validStudents;
        }

        private string GetStudentName(int num)
        {
            //TO(Done)DO: write code find the relevant student in Students and return the student's first name and surname
            // if num doesn't exist in Students, return null;
            // should use the method GetFullName() from Student/Person to get the name
            foreach (Student item in this.Students)
            {
                if(item.StudentNo==num){
                    return item.GetFullName();
                }
            }
            return null;
        }

        private int CourseGetNumStudents(int num)
        {
            //TO(Done)DO: write code find the relevant courseNo in Courses and return the number of students/enrolments
            // if num doesn't exist in Courses, return -1
            foreach (Course item in this.Courses)
            {
                if(item.CourseNo==num){
                    return item.Enrolments.Count;
                }
            }
            return -1;
        }
        private string CourseGetNumStudentsYear(int num, int iyear)
        {
            // if num doesn't exist in Courses, return -1
            string outputString=$"Students in {num} first enrolled in {iyear}";

            foreach (Course item in this.Courses)
            {
                if(item.CourseNo==num){
                    int studentcount=0;
                    foreach (Student studentitem in item.Enrolments)
                    {
                        if(studentitem.FirstRegistrationYear==iyear){
                            studentcount++;
                            outputString+=$"\n{studentitem.GetFullName()}";
                        }
                    }
                    outputString+=$"\nNumber of students: {studentcount}";
                    return outputString;
                }
            }
            outputString="invalid course";
            return outputString;
        }

        private void AddStudent()
        {
            string fname;
            string sname;
            int yob;
            int mob;
            int dob;
            int sno;
            int fyor;

            int courseno;

            System.Console.Write("Please enter student's first name: ");
            fname = Console.ReadLine();
            System.Console.Write("Please enter student's surname: ");
            sname = Console.ReadLine();

            System.Console.Write("Please enter student's year of birth: ");
            while(!int.TryParse(Console.ReadLine(), out yob)) {
                System.Console.WriteLine("Invalid, enter again");
            }

            System.Console.Write("Please enter student's month of birth: ");
            while(!int.TryParse(Console.ReadLine(), out mob)) {
                System.Console.WriteLine("Invalid, enter again");
            }

            System.Console.Write("Please enter student's date of birth: ");
            while(!int.TryParse(Console.ReadLine(), out dob)) {
                System.Console.WriteLine("Invalid, enter again");
            }

            System.Console.Write("Please enter student's id/number: ");
            while(!int.TryParse(Console.ReadLine(), out sno)) {
                System.Console.WriteLine("Invalid, enter again");
            }

            System.Console.Write("Please enter student's first year of registration: ");
            while(!int.TryParse(Console.ReadLine(), out fyor)) {
                System.Console.WriteLine("Invalid, enter again");
            }

            //TO(Done)DO: add student to the this.StudentList
            Student newStudent = new Student(fname,sname,yob,mob,dob,sno,fyor);
            this.Students.Add(newStudent);

            System.Console.Write("Enter course number to add the student to: ");
            //TO(Done)DO: add the student to the desired course in this.Courses.  
            //      If the course doesn't exist let the user know and go back to the main menu.
            // -----------------------
            // (optional - CREDIT TASK)  If the course doesn't exist keep asking until a valid course is entered.
            //                           User may enter 0000 for no course to be enrolled into
            bool validcourse=false;
            while(!validcourse){
                while(!int.TryParse(Console.ReadLine(), out courseno)) {
                    System.Console.WriteLine("Invalid, enter again");
                }
                foreach (Course item in this.Courses)
                {
                    if(item.CourseNo==courseno){
                        validcourse=true;
                        item.Enrolments.Add(newStudent);
                    }
                }
                // technically allows any number of 0's to be input to accept null course
                if(courseno==0){
                    validcourse=true;
                }
                if(!validcourse){
                System.Console.WriteLine("Invalid course, enter again");
                }
            }

        }

        //TO(Done)DO: Create the GetUnerolledStudents method/function here
        private void GetUnenrolledStudents(){
            List<Student> tempstudentlist=new List<Student>();
            List<Student> enrolledlist=new List<Student>();

            foreach (Course icourse in this.Courses)
            {
                enrolledlist=enrolledlist.Concat(icourse.Enrolments).ToList();
            }

            foreach (Student allstudent in this.Students)
            {
                foreach (Student enrol in enrolledlist)
                {
                    if(allstudent.StudentNo==enrol.StudentNo){
                        tempstudentlist.Add(allstudent);
                    }
                }
            }

            var list1 = this.Students.Where(i => !tempstudentlist.Contains(i)).ToList();

            foreach (Student item in list1)
            {
                Console.WriteLine(item.GetFullName());
            }

            Console.WriteLine($"number of unenrolled students: {list1.Count}");
        }

        private void GetOldestStudent(){
            Student oldest=new Student("aa","aa",2021,12,31,0,2021);
            for(int i=0;i<this.Students.Count;i++){
                if(this.Students[i].YearOfBirth<oldest.YearOfBirth){
                    oldest=this.Students[i];
                } else if(this.Students[i].YearOfBirth==oldest.YearOfBirth){
                    if(this.Students[i].MonthOfBirth<oldest.MonthOfBirth){
                    oldest=this.Students[i];
                    }else if(this.Students[i].MonthOfBirth==oldest.MonthOfBirth){
                        if(this.Students[i].DateOfBirth<oldest.DateOfBirth){
                            oldest=this.Students[i];
                        }

                    }
                }
            }
            Console.WriteLine($"Oldest student {oldest.StudentNo}: {oldest.GetFullName()}, {oldest.DateOfBirth}/{oldest.MonthOfBirth}/{oldest.YearOfBirth}");
        }

        private void SaveData(){
            var config = new CsvConfiguration (CultureInfo.InvariantCulture) {
                PrepareHeaderForMatch = args => args.Header.ToLower (),
            };

            using (var writer = new StreamWriter("savedatastudent.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(this.Students);
            }
            using (var writer = new StreamWriter("savedatacourse.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(this.Courses);
            }
        }
    }
}