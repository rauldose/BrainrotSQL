# Getting Started with BrainrotSQL

This guide will help you get started with BrainrotSQL, the SQL dialect that uses Italian Brainrot meme character names as SQL keywords.

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or another C# IDE
- Basic understanding of SQL

## Installation

### Option 1: Clone the Repository

1. Clone the repository
   ```
   git clone https://github.com/rauldose/brainrotsql/brainrotsql.git
   cd brainrotsql
   ```

2. Build the solution
   ```
   dotnet build
   ```

3. Run the demo application
   ```
   dotnet run --project BrainrotSql.SqliteDemo/BrainrotSql.SqliteDemo.csproj
   ```

### Option 2: Add as a NuGet Package (Coming Soon)

```
dotnet add package BrainrotSql.Engine
```

## Your First BrainrotSQL Query

Let's start with a simple SELECT query:

```csharp
// Create a translator
string brainrotQuery = "tralalero tralala id name lirili larila users";
string sqlQuery = BrainrotSqlInterpreter.ToSqlQuery(brainrotQuery);

Console.WriteLine(sqlQuery); // Outputs: "SELECT id, name FROM users"
```

## Using with a Database

To execute queries against a real database:

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
    string name = reader["name"].ToString();
    Console.WriteLine(name);
}
```

## Basic Query Examples

### SELECT Queries

```
// Select all columns
tralalero tralala frulli lirili larila users

// Select specific columns
tralalero tralala id name email lirili larila users

// Select with WHERE condition
tralalero tralala id name lirili larila users patapim age > 21

// Select with multiple conditions
tralalero tralala id name lirili larila users patapim status = 'active' camelo age > 21
```

### INSERT Queries

```
// Insert with values
bombardiro crocodilo users id name email trulimero 1 'John' 'john@example.com'
```

### UPDATE Queries

```
// Update a record
cappuccino assassino users cocofanto name burbaloni 'New Name' patapim id = 1

// Update multiple fields
cappuccino assassino users cocofanto name burbaloni 'New Name', email burbaloni 'new@example.com' patapim id = 1
```

### DELETE Queries

```
// Delete a record
bombombini gusini lirili larila users patapim id = 1

// Delete all records
bombombini gusini lirili larila users
```

### Transaction Operations

```
// Begin transaction
tung sahur

// Commit transaction
ballerina cappuccina

// Rollback transaction
chimpanzini
```

## Using the Enhanced BrainrotSQL

The enhanced version offers more flexibility with multiple character names for the same operations:

```csharp
// Initialize the enhanced interpreter
var connection = new SqlConnection("your_connection_string");
connection.Open();
var interpreter = new ImprovedBrainrotSqlInterpreter(connection);

// These queries are equivalent
var result1 = interpreter.Execute("tralalero tralala id name lirili larila users");
var result2 = interpreter.Execute("trippa id name lirili larila users");
var result3 = interpreter.Execute("troppi id name lirilì larilà users");
```

## Error Handling

BrainrotSQL provides detailed error messages when a query cannot be parsed:

```csharp
try
{
    var result = interpreter.Execute("invalid query");
}
catch (GlorboException ex)
{
    Console.WriteLine($"Brainrot Error: {ex.Message}");
}
```

## Next Steps

- Check out the [BrainrotSQL Cheat Sheet](BrainrotSQL_CheatSheet.md) for a quick reference
- Learn how to [extend BrainrotSQL](ExtendingBrainrotSQL.md) with your own characters
- Explore the [example queries](BrainrotSQL_Examples.md) for more advanced usage

Happy Brainrotting!
