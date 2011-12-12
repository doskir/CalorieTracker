using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace CalorieTracker
{
    static class Settings
    {
        public static int DailyMaxCalories = 2000;
        public static void LoadSettings()
        {
            SQLiteCommand command = Sql.SqlConnection.CreateCommand();
            command.CommandText = "SELECT name,value FROM settings";
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string settingName = reader.GetString(0);
                        switch (settingName)
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

        }
    }
}
