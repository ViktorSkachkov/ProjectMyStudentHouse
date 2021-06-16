﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_House
{
    public partial class StudentForm : Form
    {
        private StudentHouse studentHouse;
        private LogIn logIn;
        private User user;
        private Rules rules;
        private List<Event> tempEvents;
        private List<User> studentsFromThisBuilding;
        public StudentForm(LogIn li, StudentHouse sh, User u, Rules r)
        {
            InitializeComponent();
            this.studentHouse = sh;
            this.logIn = li;
            this.user = u;
            this.rules = r;
            this.tempEvents = new List<Event>();
            this.UpdateList();
            this.Fill();
            this.Update();
            this.WelcomeUser();
        }
        private void UpdateList()
        {
            this.studentsFromThisBuilding = new List<User>(this.studentHouse.GetUsersFromSameBuilding(this.user.Building));
            this.cbStudentsFromThisBuilding.Items.Clear();
            foreach (User u in this.studentsFromThisBuilding)
            {
                if (u != this.user)
                {
                    this.cbStudentsFromThisBuilding.Items.Add(u.GetInfo());
                }
            }
        }
        private void lblBack_Click(object sender, EventArgs e)
        {
            this.logIn.Show();
            this.Close();
        }
        private void Fill()
        {
            this.cbDay.Items.Clear();
            this.cbDay.Items.AddRange(this.studentHouse.GetDays().ToArray());
            this.UpdateEvents();
            this.UpdateComplainstAndMessages();
        }
        private void UpdateComplainstAndMessages()
        {
            this.lbAnswers.Items.Clear();
            this.lbAnswers.Items.AddRange(this.user.GetAnswers().ToArray());
            this.lbMessages.Items.Clear();
            this.lbMessages.Items.AddRange(this.user.GetMessages().ToArray());
        }
        private void WelcomeUser()
        {
            String firstName = this.user.FirstName;
            String surname = this.user.Surname;
            String lastName = this.user.LastName;
            this.lblWelcome.Text = String.Format("Welcome back {0} {1} {2}!", firstName, surname, lastName);
        }
        private void Update()
        {
            this.lbRules.Items.Clear();
            this.lbRules.Items.AddRange(this.rules.GetRules.ToArray());
            this.lbDaily.Items.Clear();
            foreach(DayOrWeek d in this.studentHouse.GetAllDays(this.user))
            {
                this.lbDaily.Items.Add(d.GetInfo(this.user));
            }
        }
        private void UpdateEvents()
        {
            this.lbEvents.Items.Clear();
            this.tempEvents.Clear();
            foreach (String building in Enum.GetNames(typeof(Buildings)))
            {
                foreach (Event e in this.studentHouse.GetEvents(building))
                {
                    this.tempEvents.Add(e);
                    this.lbEvents.Items.Add(e.GetInfo(this.user));
                }
            }
        }
        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            string Event = cbEvent.Text;
            string Day = cbDay.Text;
            string Time = cbTime.Text;
            User organizer = this.user;
            String building = this.user.Building;

            this.studentHouse.AddEvent(this.user, Day, Time, Event, building);
            UpdateEvents();
            MessageBox.Show("Event successfully added!");
        }
        private void SendComplaint()
        {
            string complaintdescription = this.tbComp.Text.Trim();
            DateTime complaintdate = DateTime.Now;
            String building = this.user.Building;
            this.studentHouse.AddComplaint(complaintdescription, complaintdate, building, this.user);
        }
        private void btnSendComplaint_Click(object sender, EventArgs e)
        {
            try
            {
                this.SendComplaint();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RemoveMessage()
        {
            int index = this.lbAnswers.SelectedIndex;
            this.user.RemoveAnswer(index);
            this.UpdateComplainstAndMessages();
            throw new Exception("Message successfully deleted!");
        }
        private void btnRemoveMessage_Click(object sender, EventArgs e)
        {
            try
            {
                this.RemoveMessage();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Send()
        {
            int index = this.cbStudentsFromThisBuilding.SelectedIndex;
            User u = this.studentsFromThisBuilding[index];
            String comment = this.tbSendMessage.Text.Trim();
            if (comment != "")
            {
                u.AddMessage(comment, this.user);
                throw new Exception("Message successfully sent!");
            }
            else
            {
                throw new Exception("Write something in the text field!");
            }
        }
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                this.Send();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // SHARED ITEMS ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void btnAddSharedItem_Click(object sender, EventArgs e)
        {
            int studentsCount = 0;
            this.studentHouse.manager.CreateSharedItem(user.UserNumber, tbAddedItem.Text, Convert.ToDouble(tbItemPrice.Text), Convert.ToInt32(tbItemQuantity.Text));
            foreach (User u in studentHouse.GetAllUsers())
            {
                if (user.Building == u.Building)
                {
                    studentsCount++;
                }
               
                studentsCount = 0;
            }

            listBox1.Items.Clear();
            List<SharedItems> sharedItems = this.studentHouse.manager.GetSharedItems();

            int studentCount = 0;
            foreach (User u in studentHouse.GetAllUsers())
            {
                if (user.Building == u.Building) { studentCount++; }
            }

            foreach (var v in sharedItems)
            {
                foreach (User us in studentHouse.GetAllUsers())
                {
                    if (user.Building == us.Building && user.UserNumber != us.UserNumber) { listBox1.Items.Add($"{us.FirstName} have to pay {studentHouse.manager.splittingAmmount((v.ItemPrice * v.ItemQuantity), studentCount)} on {v.StudentNumber} for {v.ItemQuantity} {v.ItemName}"); }
                }

            }
        }
       

        private void btnRemoveEvent_Click(object sender, EventArgs e)
        {
            int index = this.lbEvents.SelectedIndex;
            Event eve = this.tempEvents[index];
            if (eve.Organizer != this.user)
            {
                MessageBox.Show("You can't delete this event because it wasn't created by you!");
            }
            else
            {
                this.studentHouse.RemoveEvent(eve);
                this.lbEvents.Items.RemoveAt(index);
                MessageBox.Show("The event was successfully deleted!");
            }
        }

        private void tpRules_Click(object sender, EventArgs e)
        {

        }

        private void tpEvents_Click(object sender, EventArgs e)
        {

        }

        private void lblTime_Click(object sender, EventArgs e)
        {

        }

        private void Remove()
        {
            int action = lbMessages.SelectedIndex;
            if (action > -1)
            {
                lbMessages.Items.RemoveAt(action);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // SHARED ITEMS ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
