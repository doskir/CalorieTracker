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
        }
        private void UpdateFoodList()
        {
            listView1.Items.Clear();
            foreach(Food food in foods)
            {
                if (food.Hidden)
                    continue;
                ListViewItem lvi = new ListViewItem(food.Name);
                lvi.SubItems.Add(food.TotalKcal.ToString());
                listView1.Items.Add(lvi);
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
            command.CommandText = "SELECT * FROM meals";
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int foodId = reader.GetInt32(1);
                        DateTime date = reader.GetDateTime(2);
                        Food food = foods.Where(f => f.Id == foodId).Single();
                        Meal meal = new Meal(id, food, date);
                    }
                }
            }
        }
       
    }
}
