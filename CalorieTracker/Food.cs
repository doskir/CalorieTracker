using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalorieTracker
{
    public class Food
    {
        public int Id;
        public string Name;
        public int KcalPer100g;
        public int Grams;
        public int TotalKcal;
        public bool Hidden;
        public Food(int id,string name,int kcalPer100g,int grams,bool hidden)
        {
            Id = id;
            Name = name;
            KcalPer100g = kcalPer100g;
            Grams = grams;
            Hidden = hidden;
            TotalKcal = grams*kcalPer100g/100;
        }

    }
}
