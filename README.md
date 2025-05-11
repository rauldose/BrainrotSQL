# BrainrotSQL

BrainrotSQL is a SQL dialect interpreter that translates queries written using Italian Brainrot meme character names into standard SQL. This project features both a basic and an enhanced version of the BrainrotSQL language.

![image](https://github.com/user-attachments/assets/7f006ccd-0fc4-4d42-823d-7cd6d5dd9a62)

## Project Structure

- **BrainrotSql.Engine**: Core library for the basic BrainrotSQL implementation
- **BrainrotSql.SqliteDemo**: Console application demonstrating the basic BrainrotSQL

## What is Italian Brainrot?

Italian Brainrot is an internet meme trend that emerged in early 2025, featuring absurd AI-generated creatures with pseudo-Italian names. Characters like "Tralalero Tralala" (a shark with Nike shoes), "Bombardiro Crocodilo" (a crocodile bomber plane hybrid), and "Cappuccino Assassino" (a ninja coffee cup) gained popularity on social media platforms like TikTok and Instagram.

## Basic BrainrotSQL Syntax

BrainrotSQL uses Italian Brainrot character names as SQL keywords:

```sql
-- Select all users
tralalero tralala frulli lirili larila users

-- Insert a new user
bombardiro crocodilo users id name email trulimero 1 'John' 'john@example.com'

-- Update a user's name
cappuccino assassino users cocofanto name burbaloni 'New Name' patapim id = 1

-- Delete a user
bombombini gusini lirili larila users patapim id = 1
```

## Enhanced BrainrotSQL Features

The enhanced version offers more flexibility in query writing by supporting:

1. **Multiple synonyms** for each operation
2. **More Italian Brainrot characters** as keywords
3. **Advanced SQL features** like various JOINs, ordering, grouping, and more
4. **Easy extensibility** to add your own characters

```sql
-- These are all equivalent (SELECT * FROM users)
tralalero tralala frulli lirili larila users
troppi frulli lirili larila users
trippa frulli lirilì larilà users
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or another C# IDE

### Building the Project

1. Clone the repository
2. Open the solution in Visual Studio
3. Build the solution

### Running the Demo

1. Set either `BrainrotSql.Demo` or `ImprovedBrainrotSql.Demo` as the startup project
2. Run the application to see example translations of BrainrotSQL to standard SQL

## Using in Your Project

```csharp
// Initialize the interpreter with a database connection
var connection = new SqlConnection("your_connection_string");
connection.Open();
var interpreter = new BrainrotSqlInterpreter(connection);

// Execute a query
var result = interpreter.Execute("tralalero tralala frulli lirili larila users");

// Process the result set
var reader = result.GetResultSet();
while (reader.Read())
{
    // Process each row
}
```

## Documentation

- See the [BrainrotSQL_CheatSheet.md](BrainrotSQL_CheatSheet.md) for a quick reference of keywords

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Disclaimer

This project is meant for entertainment purposes and is not intended for production use. The Italian Brainrot meme contains potentially offensive content, and this project does not endorse or promote any offensive material associated with the meme.
