# Extending BrainrotSQL with Your Own Characters

BrainrotSQL is designed to be easily extended with your own Italian Brainrot character names or to support additional SQL features. This guide explains how to add your own characters and extend the language capabilities.

## Adding New Character Names

The easiest way to extend BrainrotSQL is to add more character names for existing SQL operations. This can be done by using the `CharacterExtensions` class:

```csharp
using BrainrotSql.Engine.Config;

// Register custom characters
CharacterExtensions.RegisterCustomCharacters();
```

You can edit the `CharacterExtensions.cs` file to add your own character mappings:

```csharp
public static void RegisterCustomCharacters()
{
    // Add new character names for existing SQL operations
    AddCharacterForOperation("SELECT", "skibidi", "toilet");
    AddCharacterForOperation("FROM", "aeromucca", "armata");
    AddCharacterForOperation("WHERE", "apollino", "cappuccino");
    
    // Add more characters as needed
}
```

## Adding New SQL Operations

You can also add entirely new SQL operations that aren't supported in the basic BrainrotSQL:

```csharp
public static void RegisterCustomSqlOperations()
{
    // Add support for a completely new SQL operation
    EnhancedKeywords.SqlOperationToKeywords["MERGE"] = new List<string> { "rantasanta", "chinaranta" };
    
    // Add support for window functions
    EnhancedKeywords.SqlOperationToKeywords["OVER"] = new List<string> { "panini", "carbonara" };
    EnhancedKeywords.SqlOperationToKeywords["PARTITION_BY"] = new List<string> { "pizzando", "mandolino" };
    
    // Add more operations as needed
}
```

## Custom Parser Extensions

For more advanced customization, you can extend the `FlexibleSqlParser` class to support additional SQL syntax:

1. Create a class that inherits from `FlexibleSqlParser`
2. Override the parsing methods to add your custom behavior
3. Use your extended parser in your application

Example:

```csharp
public class MyCustomParser : FlexibleSqlParser
{
    public MyCustomParser(string query) : base(query)
    {
    }
    
    // Override parsing methods to add custom behavior
    protected override void ParseSelectQuery()
    {
        // Call the base implementation first
        base.ParseSelectQuery();
        
        // Add your custom parsing logic here
    }
}
```

## Complete Example: Adding a New Italian Brainrot Character

Let's say you want to add a new Italian Brainrot character "Skibidi Toilet" as an alternative for the SELECT operation:

1. Update the EnhancedKeywords dictionary:

```csharp
// In CharacterExtensions.cs
public static void RegisterCustomCharacters()
{
    // Add "Skibidi Toilet" as a synonym for SELECT
    AddCharacterForOperation("SELECT", "skibidi", "toilet");
}
```

2. Initialize the configuration in your application:

```csharp
// In your application startup
BrainrotSqlConfig.Initialize();
CharacterExtensions.RegisterCustomCharacters();
```

3. Now you can use "skibidi toilet" instead of "tralalero tralala" in your queries:

```sql
-- These are now equivalent
tralalero tralala id name lirili larila users
skibidi toilet id name lirili larila users
```

## Guidelines for Creating Good BrainrotSQL Mappings

When adding new character names, consider these guidelines:

1. **Use recognizable Italian Brainrot characters** that are part of the meme ecosystem
2. **Keep it absurd and fun** - the more ridiculous, the better
3. **Use character traits** to hint at the SQL operation (e.g., aggressive characters for DELETE)
4. **Consider pairs of words** for multi-word operations (e.g., "BEGIN TRANSACTION")
5. **Be consistent with the existing style** - Italian-sounding nonsense words work best

## Adding Support for Complex SQL Features

For more complex SQL features like subqueries, CTEs, or window functions, you'll need to:

1. Define character mappings for the new operations
2. Extend the flexible parser to handle the new syntax
3. Update the SQL generation code to produce the correct SQL output

This is more advanced and might require deeper changes to the codebase. Consider contributing your extensions back to the main project for others to enjoy!
