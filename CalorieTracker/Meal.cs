using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalorieTracker
{
    public class Meal
    {
        public int Id;
        public Food Food;
        public DateTime Date;
        public Meal(int id,Food food,DateTime date)
        {
            Id = id;
            Food = food;
            Date = date;
        }
    }
}
