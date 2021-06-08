﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_House
{
    public class StudentHouse
    {
        private List<DayOrWeek> days;
        private List<User> users;
        private List<Task> tasks;
        private List<String> daysOfTheWeek;
        private Random rand;
        public StudentHouse()
        {
            this.users = new List<User>() { new User(4545, "Viktor", "Aleksandrov", "Skachkov", "viktor.skachkov01@gmail.com", "sulphur", "@student", false, false),
                                            new User(1234, "Johnny", "Brown", "Kerrigan", "j.kerriganY@gmail.com", "pas", "@student", true, false),
                                            new User(1, "Pavel", "Ivanov", "Vasilov", "pav.vasilov999@gmail.com", "admin", "@admin", false, false)};
            this.daysOfTheWeek = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            this.days = new List<DayOrWeek>() { new DayOrWeek("Monday"), new DayOrWeek("Tuesday"), new DayOrWeek("Wednesday"), new DayOrWeek("Thursday"), new DayOrWeek("Friday"), new DayOrWeek("Saturday"), new DayOrWeek("Sunday"), new DayOrWeek("This week")};
            this.tasks = new List<Task>() { new Task("do the garbage disposal", "daily"), new Task("swipe the whole property", "daily"), new Task("clean the dishes", "daily"), new Task("clean the bathroom", "weekly"), new Task("clean the whole property", "weekly") };
            this.rand = new Random();
        }
        public void AddUser(int userNumber, String firstName, String surname, String lastName, String email, String password, String determinePassword, bool pending, bool banned)
        {
            if (determinePassword != "@student" && determinePassword != "@admin")
            {
                
            }
            else
            {
                if (!this.CheckName(firstName, surname, lastName))
                {
                    if (!this.CheckEmail(email))
                    {
                        if (!this.CheckPassword(password))
                        {
                            this.users.Add(new User(userNumber, firstName, surname, lastName, email, password, determinePassword, pending, banned));
                        }//gorniq red kod ima problem s USER
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    
                }
            }
        }
        public void RemoveUser(int userNumber)
        {
            User u = this.users.Find(x => x.UserNumber == userNumber);
            String name = u.FirstName;
            this.users.Remove(u);
        }
        public bool CheckNumber(int userNumber)
        {
            if(this.users.Exists(x => x.UserNumber == userNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckName(String firstName, String surname, String lastName)
        {
            if (this.users.Exists(x => x.FirstName == firstName) && this.users.Exists(x => x.Surname == surname)
                && this.users.Exists(x => x.LastName == lastName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckPassword(String password)
        {
            if(this.users.Exists(x => x.Password == password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckEmail(String email)
        {
            if(this.users.Exists(x => x.Email == email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AddTask(String day, int i, Task task)
        {
            DayOrWeek d = this.days.Find(x => x.Name == day);
            User u = this.GetUser(i);
            if (this.CheckUser(u) && u.DeterminePassword != "@admin")
            {
                d.Add(u, task);
            }
            else
            {
                for (i = 0; i < this.users.Count; i++)
                {
                    u = this.GetUser(i);
                    if (this.CheckUser(u) && u.DeterminePassword != "@admin")
                    {
                        d.Add(u, task);
                        break;
                    }
                }
            }
        }
        private bool CheckUser(User u)
        {
            int number = 0;
            foreach (DayOrWeek d in this.days)
            {
                if (d.Students.Contains(u))
                {
                    number += d.GetNumberOfStudents(u);
                }
            }
            double checker = 23 / (this.users.Count - 1);
            if (number <= Math.Ceiling(checker))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ClearAllDays()
        {
            foreach(DayOrWeek d in this.days)
            {
                d.RemoveAll();
            }
        }
        public bool getPending(int userNumber, String password) {

            User user = this.users.Find(x => x.UserNumber == userNumber);
            return user.Pending;
        }
        
        public User GetUser(int userNumber, String password)
        {
            if (this.users.Exists(x => x.Password == password && x.UserNumber == userNumber))
            {
                User u = this.users.Find(x => x.Password == password && x.UserNumber == userNumber);
                return u;
            }
            else
            {
                throw new Exception("There is no such account!");
            }
        }
        public User GetUser(int index)
        {
            User u = this.users[index];
            return u;
        }
        public List<DayOrWeek> GetAllDays(User u)
        {
            if (u == null)
            {
                return this.days;
            }
            else
            {
                return this.days.FindAll(x => x.Students.Contains(u));
            }
        }
        public List<User> GetAllUsers()
        {
            return this.users;
        }
        public List<String> GetDays()
        {
            return this.daysOfTheWeek;
        }
        public List<Task> GetAllTasks()
        {
            return this.tasks;
        }
    


    }
}