using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using ZedGraph;

namespace CalorieTracker
{
    public partial class Form1 : Form
    {
        
        private List<Food> foods = new List<Food>();
        private List<Meal> meals = new List<Meal>(); 
        public Form1()
        {
            InitializeComponent();
            LoadFoods();
            UpdateFoodList();
            LoadMeals();
            UpdateMealTree();
        }

        private void UpdateFoodList()
        {
            foodListView.Items.Clear();
            foreach(Food food in foods)
            {
                if (food.Hidden)
                    continue;
                ListViewItem lvi = new ListViewItem(food.Name);
                lvi.Name = food.Name;
                lvi.SubItems.Add(food.TotalKcal.ToString());
                foodListView.Items.Add(lvi);
            }
        }

        private List<Week> weeks = new List<Week>();
        private void UpdateMealTree()
        {
            string[] splitter = new string[] {"|"};
            List<string> expandedNodes = new List<string>();
            foreach(TreeNode weekNode in mealTreeView.Nodes)
            {
                if (weekNode.IsExpanded)
                {
                    expandedNodes.Add(GetDateStringFromTreeNodeText(weekNode));
                    foreach (TreeNode dayNode in weekNode.Nodes)
                    {
                        if (dayNode.IsExpanded)
                            expandedNodes.Add(GetDateStringFromTreeNodeText(dayNode));
                    }
                }
            }


            mealTreeView.BeginUpdate();

            mealTreeView.Nodes.Clear();

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

            weeks.Clear();
            for (DateTime date = startDate; date < endDate; date = date.AddDays(7))
            {
                weeks.Add(new Week(date,
                                   date.Add(new TimeSpan(6,23,59,59,999))));
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
                    foreach (Meal meal in day.Meals.OrderBy(m => m.Date))
                    {
                        string mealString = meal.Date.ToString("HH:mm") + " | " + meal.Food.Name + " | "
                                            + meal.Food.TotalKcal;
                        TreeNode mealNode = new TreeNode(mealString);
                        dayNode.Nodes.Add(mealNode);
                    }
                    if (day.TotalKCal > Settings.DailyMaxCalories)
                        dayNode.BackColor = Color.Firebrick;
                    else
                        dayNode.BackColor = Color.LightGreen;
                    weekNode.Nodes.Add(dayNode);
                }
                if (week.TotalKCal > Settings.DailyMaxCalories*7)
                    weekNode.BackColor = Color.Firebrick;
                else
                    weekNode.BackColor = Color.LightGreen;
                mealTreeView.Nodes.Add(weekNode);
            }
            foreach(TreeNode weekNode in mealTreeView.Nodes)
            {
                string weekNodeDateString = GetDateStringFromTreeNodeText(weekNode);
                if(expandedNodes.Contains(weekNodeDateString))
                {
                    weekNode.Expand();
                    expandedNodes.Remove(weekNodeDateString);
                    foreach(TreeNode dayNode in weekNode.Nodes)
                    {
                        string dayNodeDateString = GetDateStringFromTreeNodeText(dayNode);
                        if(expandedNodes.Contains(dayNodeDateString))
                        {
                            dayNode.Expand();
                            expandedNodes.Remove(dayNodeDateString);
                        }
                    }
                }
            }
            mealTreeView.EndUpdate();
        }
        private string GetDateStringFromTreeNodeText(TreeNode node)
        {
            string[] splitter = new string[] {"|"};
            return node.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries)[0];
        }
        private void LoadFoods()
        {
            foods.Clear();
            
            SQLiteCommand command = Sql.SqlConnection.CreateCommand();
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
        }
        private void LoadMeals()
        {
            meals.Clear();
            
            SQLiteCommand command = Sql.SqlConnection.CreateCommand();
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
        }
        private void AddMeal(Food food,DateTime dateTime)
        {
            SQLiteCommand command = Sql.SqlConnection.CreateCommand();
            command.Parameters.AddWithValue("@foodId", food.Id);
            command.Parameters.AddWithValue("@date", dateTime);
            command.CommandText = "INSERT INTO meals (foodId,date) VALUES(@foodId,@date)";
            command.ExecuteNonQuery();
            
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
        private void RemoveMeal(Meal meal)
        {
            Sql.ExecuteNonQuery("DELETE FROM meals WHERE id=" + meal.Id);
            LoadMeals();
            UpdateMealTree();
        }
        private void RemoveFood(Food food)
        {
            Sql.ExecuteNonQuery("UPDATE food SET hidden=1 WHERE id=" + food.Id);
            LoadFoods();
            UpdateFoodList();
        }
        private void AddFood(string name, int kcalPer100g, int grams)
        {
            SQLiteCommand command = Sql.SqlConnection.CreateCommand();
            command.Parameters.AddWithValue("@foodName", name);
            command.Parameters.AddWithValue("@kcalPer100g", kcalPer100g);
            command.Parameters.AddWithValue("@grams", grams);
            command.Parameters.AddWithValue("@hidden", false);
            command.CommandText =
                "INSERT INTO food (name,kcalper100g,grams,hidden) VALUES(@foodName,@kcalPer100g,@grams,@hidden)";
            command.ExecuteNonQuery();

            LoadFoods();
            UpdateFoodList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = foodNameTextBox.Text;
            int kcalPer100g = int.Parse(caloriesPer100gTextBox.Text);
            int grams = int.Parse(gramsTextBox.Text);
            AddFood(name, kcalPer100g, grams);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = foodNameTextBox.Text;
            int kcalPer100g = int.Parse(caloriesPer100gTextBox.Text);
            int grams = int.Parse(gramsTextBox.Text);
            AddFood(name, kcalPer100g, grams);
            Food food = foods.Where(f => f.Name == name).Single();
            AddMeal(food, DateTime.Now);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.maskedTextBox1.Text = Settings.DailyMaxCalories.ToString();
            settingsForm.Show();
        }

        private void mealTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && mealTreeView.SelectedNode != null)
            {
                TreeNode node = mealTreeView.SelectedNode;
                if (node.Level != 2)
                    return;
                if (MessageBox.Show("Do you want to delete " + node.Text + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RemoveMeal(node);
                }

            }
        }

        private void foodListView_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem lvi = foodListView.SelectedItems[0];
            if (MessageBox.Show("Add " + lvi.Name + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Food food = foods.Where(f => f.Name == lvi.Name).Single();
                AddMeal(food, DateTime.Now);

            }
        }

        private void foodListView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (foodListView.SelectedItems.Count != 1)
                    return;
                ListViewItem item = foodListView.SelectedItems[0];
                string foodName = item.SubItems[0].Text;
                int foodKcal = Int32.Parse(item.SubItems[1].Text);
                Food food = foods.Where(f => f.Name == foodName && f.TotalKcal == foodKcal).Last();
                if(MessageBox.Show("Do you want to delete " + food.Name + " ?","",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RemoveFood(food);
                }
            }
        }
        private void RemoveMeal(TreeNode node)
        {
            string[] parts = node.Text.Split(new string[] {" | "}, StringSplitOptions.RemoveEmptyEntries);
            string time = parts[0];
            string date = node.Parent.Text.Substring(0, node.Parent.Text.IndexOf(' '));
            DateTime dt = DateTime.Parse(date + "T" + time);
            string name = parts[1];
            Meal meal =
                meals.Where(m => m.Food.Name == name && m.Date.Hour == dt.Hour && m.Date.Minute == dt.Minute)
                    .Last();
            RemoveMeal(meal);
        }
        private Meal GetMealFromTreeNode(TreeNode node)
        {
            string[] parts = node.Text.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
            string time = parts[0];
            string date = node.Parent.Text.Substring(0, node.Parent.Text.IndexOf(' '));
            DateTime dt = DateTime.Parse(date + "T" + time);
            string name = parts[1];
            Meal meal =
                meals.Where(m => m.Food.Name == name && m.Date.Hour == dt.Hour && m.Date.Minute == dt.Minute)
                    .Last();
            return meal;
        }

        private void plotButton_Click(object sender, EventArgs e)
        {
            PlotWindow plotWindow = new PlotWindow();
            plotWindow.Show();
        }

        private void mealTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void mealTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void mealTreeView_DragDrop(object sender, DragEventArgs e)
        {
            //should probably be replaced with a predefined format
            TreeNode movingNode = (TreeNode) e.Data.GetData(e.Data.GetFormats()[0]);
            TreeNode targetNode = mealTreeView.GetNodeAt(PointToClient(new Point(e.X, e.Y)));
            if (movingNode == null || targetNode == null)
                return;
            //only move meal nodes, not days or weeks
            if (movingNode.Level != 2)
                return;
            //do not allow moving a mealnode to a weeknode
            if (targetNode.Level < 1 || targetNode.Level > 2)
                return;
            if (targetNode.Level == 2)
                targetNode = targetNode.Parent;

            string dateString = targetNode.Text.Split(new string[] {"|"}, StringSplitOptions.RemoveEmptyEntries)[0];
            DateTime date = DateTime.Parse(dateString);
            Meal meal = GetMealFromTreeNode(movingNode);
            DateTime newDate = new DateTime(date.Year, date.Month, date.Day, meal.Date.Hour, meal.Date.Minute,
                                            meal.Date.Second);

            SQLiteCommand command = Sql.SqlConnection.CreateCommand();
            command.Parameters.AddWithValue("@mealid", meal.Id);
            command.Parameters.AddWithValue("@newDate", newDate);
            command.CommandText =
                "UPDATE meals SET date=@newDate WHERE id=@mealid";
            command.ExecuteNonQuery();

            LoadMeals();
            UpdateMealTree();
        }
    }
}
