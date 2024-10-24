using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hashi__bridges_
{
    public partial class TopScoresForm : Form
    {
        private string connectionString = "Data Source=hashi_scores.db;Version=3;";

        public TopScoresForm()
        {
            InitializeComponent();
            LoadTopScores();
        }

        private void LoadTopScores()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT player_name AS 'Player', time_score AS 'Time (seconds)', difficulty_level AS 'Difficulty' " +
                               "FROM TimeScores ORDER BY time_score ASC LIMIT 5";

                using (var adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Bind the DataTable to the DataGridView
                    scoresDataGridView.DataSource = dataTable;
                }
            }
        }
    }
}
