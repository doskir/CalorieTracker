using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace CalorieTracker
{
    public partial class PlotWindow : Form
    {
        public PlotWindow()
        {
            InitializeComponent();
            PlotCalorieIntake();
        }
        public void PlotCalorieIntake()
        {
            PointPairList pointPairList = new PointPairList();
            
            SQLiteCommand command = Sql.SqlConnection.CreateCommand();
            command.CommandText =
                "SELECT meals.date,food.kcalper100g,food.grams FROM meals INNER JOIN food ON meals.foodId=food.id ORDER BY meals.date";

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime storedDate = reader.GetDateTime(0);
                        double kcal = reader.GetDouble(1) / 100 * reader.GetDouble(2);

                        double xDate = new XDate(storedDate.Year, storedDate.Month, storedDate.Day, 12, 0, 0);

                        PointPair olderPoint = pointPairList.SingleOrDefault(pp => pp.X == xDate);
                        if(olderPoint != null)
                        {
                            olderPoint.Y += kcal;
                        }
                        else
                        {
                            pointPairList.Add(new PointPair(xDate, kcal));
                        }
                    }
                }
            }

            zedGraphControl1.GraphPane.XAxis.Title.Text = "Date";
            zedGraphControl1.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphControl1.GraphPane.XAxis.Scale.MajorUnit = DateUnit.Day;

            zedGraphControl1.GraphPane.YAxis.Title.Text = "Kcal";
            zedGraphControl1.GraphPane.YAxis.Type = AxisType.Linear;

            zedGraphControl1.GraphPane.AddCurve("KCal per Day", pointPairList, Color.Black, SymbolType.Circle);
            zedGraphControl1.RestoreScale(zedGraphControl1.GraphPane);
            zedGraphControl1.IsShowPointValues = true;


            LineObj maxCalorieLine = new LineObj(Color.Firebrick, zedGraphControl1.GraphPane.XAxis.Scale.Min, Settings.DailyMaxCalories, zedGraphControl1.GraphPane.XAxis.Scale.Max, Settings.DailyMaxCalories);
            maxCalorieLine.ZOrder = ZOrder.E_BehindCurves;
            zedGraphControl1.GraphPane.GraphObjList.Add(maxCalorieLine);

            Show();
        }
    }
}
