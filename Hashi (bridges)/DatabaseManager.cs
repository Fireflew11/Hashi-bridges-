using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Hashi__bridges_
{
    internal class DatabaseManager
    {
        private string connectionString = "Data Source=hashi_scores.db;Version=3;";

        public DatabaseManager()
        {
            CreateDatabaseAndTable();
        }

        private void CreateDatabaseAndTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS TimeScores (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                player_name TEXT,
                time_score REAL,
                date_played TEXT,
                difficulty_level TEXT
            );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertTimeScore(string playerName, decimal timeScore, string difficultyLevel)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = @"
            INSERT INTO TimeScores (player_name, time_score, date_played, difficulty_level) 
            VALUES (@playerName, @timeScore, @datePlayed, @difficultyLevel)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@playerName", playerName);
                    command.Parameters.AddWithValue("@timeScore", timeScore);
                    command.Parameters.AddWithValue("@datePlayed", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@difficultyLevel", difficultyLevel);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void GetTopScores()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT player_name, time_score, difficulty_level FROM TimeScores ORDER BY time_score ASC LIMIT 10";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["player_name"]} - {reader["time_score"]} seconds - {reader["difficulty_level"]}");
                        }
                    }
                }
            }
        }
    }
}
