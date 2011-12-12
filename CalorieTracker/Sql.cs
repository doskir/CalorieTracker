using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace CalorieTracker
{
    class Sql
    {
        public static SQLiteConnection SqlConnection = new SQLiteConnection("Data Source=caloriedb.s3db");
        public static void ExecuteNonQuery(string commandText)
        {
            //since this is just sqlite it's not even worth it to protect the db against injection attacks
            SQLiteCommand command = SqlConnection.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }
    }
}
