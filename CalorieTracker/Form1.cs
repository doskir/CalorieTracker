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
        private Sql sql;
        private List<Food> foods = new List<Food>();
        private List<Meal> meals = new List<Meal>(); 
        public Form1()
        {
            InitializeComponent();
            sql = new Sql();
            LoadSettings();
            LoadFoods();
            UpdateFoodList();
            LoadMeals();
            UpdateMealTree();
        }
        private void LoadSettings()
        {
            sql.SqlConnection.Open();
            SQLiteCommand command = sql.SqlConnection.CreateCommand();
            command.CommandText = "SELECT name,value FROM settings";
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        string settingName = reader.GetString(0);
                        switch(settingName)
                        {
                            case "dailymaxkcal":
                                {
                                    DailyMaxCalories = int.Parse(reader.GetString(1));
                                    break;
                                }
                        }
                    }
                }
            }
            sql.SqlConnection.Close();
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
            if (meals.Count == 0)
                return;
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
                string weekString = week.StartDate.ToString("yyyy.MM.dd") + "-" +
                                    week.StopDate.ToString("dd") + " | " + week.TotalKCal.ToString();
                TreeNode weekNode = new TreeNode(weekString);
                if (week.Contains(DateTime.Now))
                    weekNode.Expand();
                foreach (Day day in week.Days)
                {
                    string dayString = day.StartDate.ToString("yyyy.MM.dd") + " | " + day.TotalKCal.ToString();
                    TreeNode dayNode = new TreeNode(dayString);
                    if (day.Contains(DateTime.Now))
                        dayNode.Expand();
                    foreach (Meal meal in day.Meals)
                    {
                        string mealString = meal.Date.ToString("HH:mm") + " | " + meal.Food.Name + " | "
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
                if (week.TotalKCal > DailyMaxCalories*7)
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

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete && treeView1.SelectedNode != null)
            {
                TreeNode node = treeView1.SelectedNode;
                if (node.Level != 2)
                    return;

                string[] parts = node.Text.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                if (MessageBox.Show("Do you want to delete " + node.Text + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string time = parts[0];
                    string date = node.Parent.Text.Substring(0, node.Parent.Text.IndexOf(' '));
                    DateTime dt = DateTime.Parse(date + "T" + time);
                    string name = parts[1];
                    Meal meal =
                        meals.Where(m => m.Food.Name == name && m.Date.Hour == dt.Hour && m.Date.Minute == dt.Minute)
                            .Last();
                    RemoveMeal(meal);
                }

            }
        }
        private void RemoveMeal(Meal meal)
        {
            sql.SqlConnection.Open();
            SQLiteCommand command = sql.SqlConnection.CreateCommand();
            command.Parameters.AddWithValue("@mealId", meal.Id);
            command.CommandText = "DELETE FROM meals WHERE id=@mealId";
            command.ExecuteNonQuery();
            sql.SqlConnection.Close();
            LoadMeals();
            UpdateMealTree();
        }
        private void AddFood(string name,int kcalPer100g,int grams)
        {
            sql.SqlConnection.Open();
            SQLiteCommand command = sql.SqlConnection.CreateCommand();
            command.Parameters.AddWithValue("@foodName", name);
            command.Parameters.AddWithValue("@kcalPer100g", kcalPer100g);
            command.Parameters.AddWithValue("@grams", grams);
            command.Parameters.AddWithValue("@hidden", false);
            command.CommandText =
                "INSERT INTO food (name,kcalper100g,grams,hidden) VALUES(@foodName,@kcalPer100g,@grams,@hidden)";
            command.ExecuteNonQuery();
            sql.SqlConnection.Close();
            LoadFoods();
            UpdateFoodList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            int kcalPer100g = int.Parse(textBox2.Text);
            int grams = int.Parse(textBox3.Text);
            AddFood(name, kcalPer100g, grams);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            int kcalPer100g = int.Parse(textBox2.Text);
            int grams = int.Parse(textBox3.Text);
            AddFood(name, kcalPer100g, grams);
            Food food = foods.Where(f => f.Name == name).Single();
            AddMeal(food, DateTime.Now);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.maskedTextBox1.Text = DailyMaxCalories.ToString();
            settingsForm.Show();
        }
    }
}
