using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CalorieTracker
{
    public partial class Form1 : Form
    {
        private int DailyMaxCalories = 2000;
        private int WeeklyMaxCalories = 2000*7;
        private Sql sql;
        private List<Food> foods = new List<Food>();
        private List<Meal> meals = new List<Meal>(); 
        public Form1()
        {
            InitializeComponent();
            sql = new Sql();
            LoadFoods();
            UpdateFoodList();
            LoadMeals();
            UpdateMealTree();
        }
        private void UpdateFoodList()
        {
            listView1.Items.Clear();
            foreach(Food food in foods)
            {
                if (food.Hidden)
                    continue;
                ListViewItem lvi = new ListViewItem(food.Name);
                lvi.Name = food.Name;
                lvi.SubItems.Add(food.TotalKcal.ToString());
                listView1.Items.Add(lvi);
            }
        }
        private void UpdateMealTree()
        {
            treeView1.Nodes.Clear();
            List<Meal> mealsByDate = meals.OrderBy(m => m.Date).ToList();

            DateTime firstMealTime = mealsByDate.First().Date;
            DateTime startDate = new DateTime(firstMealTime.Year, firstMealTime.Month, firstMealTime.Day, 0, 0, 0);
            while (startDate.DayOfWeek != DayOfWeek.Monday)
                startDate = startDate.Subtract(TimeSpan.FromDays(1));
            DateTime lastMealTime = mealsByDate.Last().Date;
            DateTime endDate = new DateTime(lastMealTime.Year, lastMealTime.Month, lastMealTime.Day, 23, 59, 59, 999);
            while (endDate.DayOfWeek != DayOfWeek.Sunday)
                endDate = endDate.AddDays(1);

            List<Week> weeks = new List<Week>();
            for (DateTime date = startDate; date < endDate; date = date.AddDays(7))
            {
                weeks.Add(new Week(date,
                                   date.AddDays(6).Date.AddHours(23).Date.AddMinutes(59).Date.AddSeconds(59).Date.
                                       AddMilliseconds(999)));
            }
            foreach (Meal meal in meals)
            {
                Week week = weeks.Where(w => w.Contains(meal.Date)).Single();
                Day day = week.Days.Where(d => d.Contains(meal.Date)).Single();
                day.Meals.Add(meal);
            }
            //we want to list the current week at the top
            weeks.Reverse();
            foreach (Week week in weeks)
            {
                week.Refresh();
                string weekString = week.StartDate.ToString("yyyy.mm.dd") + "-" +
                                    week.StopDate.ToString("dd") + " | " + week.TotalKCal.ToString();
                TreeNode weekNode = new TreeNode(weekString);
                if (week.Contains(DateTime.Now))
                    weekNode.Expand();
                foreach (Day day in week.Days)
                {
                    string dayString = day.StartDate.ToString("yyyy.mm.dd") + " | " + day.TotalKCal.ToString();
                    TreeNode dayNode = new TreeNode(dayString);
                    if (day.Contains(DateTime.Now))
                        dayNode.Expand();
                    foreach (Meal meal in day.Meals)
                    {
                        string mealString = meal.Date.ToString("hh:mm:") + " | " + meal.Food.Name + " | "
                                            + meal.Food.TotalKcal;
                        TreeNode mealNode = new TreeNode(mealString);
                        dayNode.Nodes.Add(mealNode);
                    }
                    if (day.TotalKCal > DailyMaxCalories)
                        dayNode.BackColor = Color.Firebrick;
                    else
                        dayNode.BackColor = Color.LightGreen;
                    weekNode.Nodes.Add(dayNode);
                }
                if (week.TotalKCal > WeeklyMaxCalories)
                    weekNode.BackColor = Color.Firebrick;
                else
                    weekNode.BackColor = Color.LightGreen;
                treeView1.Nodes.Add(weekNode);
            }
        }

        private void LoadFoods()
        {
            foods.Clear();
            sql.SqlConnection.Open();
            System.Data.SQLite.SQLiteCommand command = sql.SqlConnection.CreateCommand();
            command.CommandText = "SELECT * FROM food ORDER BY name";
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int kcalPer100g = reader.GetInt32(2);
                        int grams = reader.GetInt32(3);
                        bool hidden = reader.GetBoolean(4);
                        Food food = new Food(id, name, kcalPer100g, grams, hidden);
                        foods.Add(food);
                    }
                }
            }
            sql.SqlConnection.Close();
        }
        private void LoadMeals()
        {
            meals.Clear();
            sql.SqlConnection.Open();
            SQLiteCommand command = sql.SqlConnection.CreateCommand();
            command.CommandText = "SELECT id,foodId,date FROM meals";
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int foodId = reader.GetInt32(1);
                        DateTime date = reader.GetDateTime(2);
                        Food food = foods.Where(f => f.Id == foodId).Single();
                        Meal meal = new Meal(id, food, date);
                        meals.Add(meal);
                    }
                }
            }
            sql.SqlConnection.Close();
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem lvi = listView1.SelectedItems[0];
            if(MessageBox.Show("Add " + lvi.Name + " ?","",MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                Food food = foods.Where(f => f.Name == lvi.Name).Single();
                AddMeal(food, DateTime.Now);

            }
        }
        private void AddMeal(Food food,DateTime dateTime)
        {
            sql.SqlConnection.Open();
            SQLiteCommand command = sql.SqlConnection.CreateCommand();
            command.Parameters.AddWithValue("@foodId", food.Id);
            command.Parameters.AddWithValue("@date", dateTime);
            command.CommandText = "INSERT INTO meals (foodId,date) VALUES(@foodId,@date)";
            command.ExecuteNonQuery();
            sql.SqlConnection.Close();
            LoadMeals();
            UpdateMealTree();
        }
        public class Day
        {
            public DateTime StartDate { get; set; }
            public DateTime StopDate { get; set; }
            public int TotalKCal { get; set; }
            public List<Meal> Meals = new List<Meal>();
            public Day(DateTime startDate, DateTime stopDate)
            {
                StartDate = startDate;
                StopDate = stopDate;
            }
            public void Refresh()
            {
                TotalKCal = 0;
                foreach (Meal meal in Meals)
                {
                    TotalKCal += meal.Food.TotalKcal;
                }
            }
            public bool Contains(DateTime date)
            {
                return date >= StartDate && date <= StopDate;
            }
        }
        public class Week
        {
            public DateTime StartDate { get; set; }
            public DateTime StopDate { get; set; }
            public int TotalKCal { get; set; }
            public List<Day> Days = new List<Day>();
            public Week(DateTime startDate, DateTime stopDate)
            {
                StartDate = startDate;
                StopDate = stopDate;
                TotalKCal = 0;
                for (int i = 0; i < 7; i++)
                {
                    Days.Add(new Day(startDate.AddDays(i), startDate.AddDays(i).AddHours(23).AddMinutes(59).AddSeconds(59)));
                }
            }
            public void Refresh()
            {
                TotalKCal = 0;
                foreach (Day day in Days)
                {
                    day.Refresh();
                    TotalKCal += day.TotalKCal;
                }
            }
            public bool Contains(DateTime date)
            {
                return date >= StartDate && date <= StopDate;
            }
        }
    }
}
