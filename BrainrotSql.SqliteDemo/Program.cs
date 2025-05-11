using BrainrotSql.SqliteDemo;
using System;
using System.IO;

namespace BrainrotSql.SqliteDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BrainrotSQL SQLite Demo");
            Console.WriteLine("======================");
            Console.WriteLine();

            string dbPath = "brainrot.db";
            bool useImprovedInterpreter = false;
            bool runInteractiveMode = false;

            // Process command-line arguments
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--db":
                    case "-d":
                        if (i + 1 < args.Length)
                        {
                            dbPath = args[++i];
                        }
                        break;
                    case "--improved":
                    case "-i":
                        useImprovedInterpreter = true;
                        break;
                    case "--interactive":
                    case "-r":
                        runInteractiveMode = true;
                        break;
                    case "--help":
                    case "-h":
                        ShowHelp();
                        return;
                }
            }

            Console.WriteLine($"Using database: {Path.GetFullPath(dbPath)}");
            Console.WriteLine($"Interpreter: {(useImprovedInterpreter ? "Improved" : "Basic")} BrainrotSQL");
            Console.WriteLine();

            var adapter = new SqliteAdapter(dbPath, useImprovedInterpreter);

            try
            {
                adapter.OpenConnection();
                adapter.InitializeDatabase();

                // Run predefined demo queries
                RunDemoQueries(adapter);

                // Run interactive mode if requested
                if (runInteractiveMode)
                {
                    RunInteractiveMode(adapter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                adapter.CloseConnection();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void ShowHelp()
        {
            Console.WriteLine("BrainrotSQL SQLite Demo");
            Console.WriteLine("======================");
            Console.WriteLine();
            Console.WriteLine("Usage: BrainrotSql.SqliteDemo [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --db, -d <path>      Path to SQLite database file (default: brainrot.db)");
            Console.WriteLine("  --improved, -i       Use improved BrainrotSQL interpreter");
            Console.WriteLine("  --interactive, -r    Run in interactive mode after demo queries");
            Console.WriteLine("  --help, -h           Show this help message");
        }

        static void RunDemoQueries(SqliteAdapter adapter)
        {
            Console.WriteLine("\nRunning demo queries...");
            Console.WriteLine("======================");

            // SELECT all users
            adapter.ExecuteQuery("tralalero tralala frulli lirili larila users");

            // SELECT specific columns
            adapter.ExecuteQuery("tralalero tralala id name email lirili larila users");

            // SELECT with WHERE condition
            adapter.ExecuteQuery("tralalero tralala id name lirili larila users patapim status = 'active'");

            // SELECT with multiple conditions
            adapter.ExecuteQuery("tralalero tralala id name age lirili larila users patapim status = 'active' camelo age > 20");

            // SELECT with NULL check
            adapter.ExecuteQuery("tralalero tralala id name email lirili larila users patapim email bobrito girafa");
      
            // SELECT with JOIN
            adapter.ExecuteQuery("tralalero tralala users.name orders.order_date orders.total_amount lirili larila users boneca ambalabu orders patapim users.id = orders.user_id");

            // INSERT a new user
            adapter.ExecuteQuery("bombardiro crocodilo users id name email age status trulimero 6 'Frullì Frullà' 'frulli@example.com' 29 'active'");

            // UPDATE a user
            adapter.ExecuteQuery("cappuccino assassino users cocofanto status burbaloni 'premium' patapim id = 6");

            // DELETE a user
            adapter.ExecuteQuery("bombombini gusini lirili larila users patapim id = 6");
        }

        static void RunInteractiveMode(SqliteAdapter adapter)
        {
            Console.WriteLine("\nInteractive Mode");
            Console.WriteLine("===============");
            Console.WriteLine("Enter BrainrotSQL queries (type 'exit' to quit)");
            Console.WriteLine("TIP: Type 'help' to see a list of common BrainrotSQL keywords");
            Console.WriteLine();

            while (true)
            {
                Console.Write("\nBrainrotSQL> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.ToLower() == "exit")
                    break;

                if (input.ToLower() == "help")
                {
                    ShowBrainrotSqlHelp();
                    continue;
                }

                try
                {
                    adapter.ExecuteQuery(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void ShowBrainrotSqlHelp()
        {
            Console.WriteLine("\nBrainrotSQL Common Keywords:");
            Console.WriteLine("==========================");
            Console.WriteLine("SELECT:    tralalero tralala");
            Console.WriteLine("FROM:      lirili larila");
            Console.WriteLine("WHERE:     patapim");
            Console.WriteLine("INSERT:    bombardiro crocodilo");
            Console.WriteLine("VALUES:    trulimero");
            Console.WriteLine("UPDATE:    cappuccino assassino");
            Console.WriteLine("SET:       cocofanto");
            Console.WriteLine("SET EQUAL: burbaloni");
            Console.WriteLine("DELETE:    bombombini gusini");
            Console.WriteLine("JOIN:      boneca ambalabu");
            Console.WriteLine("AND:       camelo");
            Console.WriteLine("OR:        vaca");
            Console.WriteLine("ALL (*):   frulli");
            Console.WriteLine("NULL:      girafa");
            Console.WriteLine("IS:        bobrito bandito");
            Console.WriteLine();
            Console.WriteLine("Example: tralalero tralala id name lirili larila users patapim age > 30");
        }
    }
}