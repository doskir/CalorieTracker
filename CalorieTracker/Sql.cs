using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace CalorieTracker
{
    class Sql
    {
        public System.Data.SQLite.SQLiteConnection SqlConnection = new SQLiteConnection("Data Source=caloriedb.s3db");
    }
}
