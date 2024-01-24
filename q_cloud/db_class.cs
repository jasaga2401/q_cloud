using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace q_cloud
{
    internal class db_class
    {
        public static string _dbPath = string.Empty;

        public static void create_db()
        {
            string databaseName = "MyDatabase.db";
            _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databaseName);

            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT EXISTS MyTable " +
                "(uid INTEGER PRIMARY KEY, " +
                "forename VARCHAR(100) NOT NULL, " +
                "surname VARCHAR(100) NOT NULL, " +
                "dob DATE, " +
                "start_date DATE, " +
                "quit_date DATE, " +
                "averageSmokeDay INTEGER NOT NULL, " +
                "cstPckCig DOUBLE NOT NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }            
        }

        public static void insert_data(string var_forename, string var_surname, DateOnly var_dob, DateOnly var_start_date, DateOnly var_quit_date, int var_averageSmokeDay, double var_cstPckCig)
        {
            Console.WriteLine("***************** named data *******************************" + var_forename + " " + var_surname + " " + var_dob + " " + var_start_date + " " + var_quit_date + " " + var_averageSmokeDay + " " + var_cstPckCig);
            // DateOnly var_dob 

            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry1, @Entry2, @Entry3, @Entry4, @Entry5, @Entry6, @Entry7);";
                insertCommand.Parameters.AddWithValue("@Entry1", var_forename);
                insertCommand.Parameters.AddWithValue("@Entry2", var_surname);
                insertCommand.Parameters.AddWithValue("@Entry3", var_dob);
                insertCommand.Parameters.AddWithValue("@Entry4", var_start_date);
                insertCommand.Parameters.AddWithValue("@Entry5", var_quit_date);
                insertCommand.Parameters.AddWithValue("@Entry6", var_averageSmokeDay);
                insertCommand.Parameters.AddWithValue("@Entry7", var_cstPckCig);

                // Use ExecuteNonQuery for INSERT, UPDATE, and DELETE operations 
                insertCommand.ExecuteNonQuery();


                string query = "SELECT * FROM MyTable"; // Your SQL query
                using (var command = new SqliteCommand(query, db))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read()) // Iterates through each row
                        {
                            for (int i = 0; i < reader.FieldCount; i++) // Iterates through each column
                            {
                                Console.Write(reader.GetValue(i) + " " + " ***********"); // Outputs each field value
                            }
                            Console.WriteLine(); // New line for next row
                        }
                    }
                }


                db.Close();
            }
        }

        public static List<List<String>> get_data()
        {
            List<List<String>> entries = new List<List<string>>();

            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT uid, forename, surname, dob, start_date, quit_date, averageSmokeDay, cstPckCig from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    // Create a new List for each row
                    List<string> row = new List<string>
                    {
                        query.GetString(0), // uid
                        query.GetString(1), // forename
                        query.GetString(2),  // surname
                        query.GetString(3),  // dob
                        query.GetString(4),  // start date
                        query.GetString(5),  // quit date
                        query.GetString(6),  // average smoke day
                        query.GetString(7)  // cost packet cigarettes
                    };

                    entries.Add(row); // Add the row to the main list
                }                

                // Console.WriteLine("Only the last entry: " + entries[0][4]);

                db.Close();
            }

            return entries;
        }

        public static void delete_data(string var_item)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={_dbPath}"))
            {
                db.Open();

                SqliteCommand deleteCommand = new SqliteCommand();
                deleteCommand.Connection = db;

                Console.WriteLine("Abount to delete data " + var_item);

                // Use parameterized query to prevent SQL injection attacks
                deleteCommand.CommandText = "DELETE FROM MyTable WHERE Text_Entry = @Entry;";
                deleteCommand.Parameters.AddWithValue("@Entry", var_item);

                deleteCommand.ExecuteReader();

                db.Close();
            }
        }
    }
}
