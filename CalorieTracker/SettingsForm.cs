using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CalorieTracker
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text == "")
                return;
            Sql sql = new Sql();
            sql.SqlConnection.Open();
            SQLiteCommand command = sql.SqlConnection.CreateCommand();
            command.Parameters.AddWithValue("@name", "dailymaxkcal");
            command.Parameters.AddWithValue("@value", maskedTextBox1.Text);
            
            command.CommandText = "UPDATE settings SET value=@value WHERE name=@name";
            command.ExecuteNonQuery();
            sql.SqlConnection.Close();
            MessageBox.Show("Please close and reopen the application to activate your personal setting.");
        }
    }
}
