using BrainrotSql.Engine;
using ImprovedBrainrotSql.Engine;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace BrainrotSql.SqliteDemo
{
    /// <summary>
    /// Adapter class to use BrainrotSQL with SQLite
    /// </summary>
    public class SqliteAdapter
    {
        private readonly string _connectionString;
        private SqliteConnection? _connection;
        private ImprovedBrainrotSqlInterpreter? _improvedInterpreter;
        private bool _useImprovedInterpreter;

        public SqliteAdapter(string databasePath, bool useImprovedInterpreter = false)
        {
            _connectionString = $"Data Source={databasePath}";
            _useImprovedInterpreter = useImprovedInterpreter;
        }

        /// <summary>
        /// Open connection to the SQLite database
        /// </summary>
        public void OpenConnection()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();


            _improvedInterpreter = new ImprovedBrainrotSqlInterpreter(_connection);
            Console.WriteLine("Using Improved BrainrotSQL Interpreter");

        }

        /// <summary>
        /// Close connection to the SQLite database
        /// </summary>
        public void CloseConnection()
        {
            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
            _improvedInterpreter = null;
        }

        /// <summary>
        /// Execute a BrainrotSQL query and print the results
        /// </summary>
        public void ExecuteQuery(string brainrotQuery)
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection is not open");
            }

            try
            {
                Console.WriteLine($"\nExecuting BrainrotSQL query: {brainrotQuery}");

                // Convert BrainrotSQL to standard SQL for display
                string sqlQuery = ImprovedBrainrotSqlInterpreter.ToSqlQuery(brainrotQuery);
                
                Console.WriteLine($"Translated SQL: {sqlQuery}");

                // Execute the query
                var result = _improvedInterpreter?.Execute(brainrotQuery);

                if (result == null)
                {
                    Console.WriteLine("No result returned");
                    return;
                }

                // Check if the query returned a result set
                var reader = result.GetResultSet();
                if (reader != null)
                {
                    PrintResultSet(reader);
                }
                else
                {
                    // For non-SELECT queries, show the number of affected rows
                    Console.WriteLine($"Affected rows: {result.GetAffectedRows()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Initialize the SQLite database with sample tables and data
        /// </summary>
        public void InitializeDatabase()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("Connection is not open");
            }

            Console.WriteLine("Initializing database...");

            try
            {
                // Create tables
                ExecuteSqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY,
                        name TEXT NOT NULL,
                        email TEXT,
                        age INTEGER,
                        status TEXT
                    )");

                ExecuteSqliteCommand(@"
                    CREATE TABLE IF NOT EXISTS orders (
                        id INTEGER PRIMARY KEY,
                        user_id INTEGER,
                        order_date TEXT,
                        total_amount REAL,
                        FOREIGN KEY(user_id) REFERENCES users(id)
                    )");

                // Check if data already exists
                using (var cmd = _connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM users";
                    var count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 0)
                    {
                        // Insert sample data
                        ExecuteSqliteCommand(@"
                            INSERT INTO users (id, name, email, age, status) VALUES 
                            (1, 'Tralalero Tralala', 'tralala@example.com', 25, 'active'),
                            (2, 'Bombardiro Crocodilo', 'bombard@example.com', 32, 'inactive'),
                            (3, 'Cappuccino Assassino', 'cappuccino@example.com', 18, 'active'),
                            (4, 'Bombombini Gusini', 'bomber@example.com', 45, 'active'),
                            (5, 'Boneca Ambalabu', 'boneca@example.com', NULL, 'pending')");

                        ExecuteSqliteCommand(@"
                            INSERT INTO orders (id, user_id, order_date, total_amount) VALUES 
                            (1, 1, '2025-01-15', 129.99),
                            (2, 1, '2025-02-20', 55.50),
                            (3, 2, '2025-01-10', 75.25),
                            (4, 3, '2025-03-05', 210.75),
                            (5, 5, '2025-02-28', 85.00)");
                    }
                }

                Console.WriteLine("Database initialized successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void ExecuteSqliteCommand(string sql)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        private void PrintResultSet(IDataReader reader)
        {
            Console.WriteLine("\nResults:");

            // Print header
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i),-15}");
            }
            Console.WriteLine();

            // Print separator
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(new string('-', 15));
            }
            Console.WriteLine();

            // Print rows
            int rowCount = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();
                    Console.Write($"{value,-15}");
                }
                Console.WriteLine();
                rowCount++;
            }

            Console.WriteLine($"\n{rowCount} row(s) returned");
        }
    }
}