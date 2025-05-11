using BrainrotSql.Engine.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedBrainrotSql.Engine.Config
{
    /// <summary>
    /// This class shows how to extend BrainrotSQL with new character names.
    /// To add your own Italian Brainrot characters, follow this template.
    /// </summary>
    public static class CharacterExtensions
    {
        /// <summary>
        /// Call this method during application startup to add custom character names
        /// </summary>
        public static void RegisterCustomCharacters()
        {
            // Add new character names for existing SQL operations
            AddCharacterForOperation("SELECT", "skibidi", "toilet");
            AddCharacterForOperation("FROM", "aeromucca", "armata");
            AddCharacterForOperation("WHERE", "apollino", "cappuccino");

            // Add new characters for joins
            AddCharacterForOperation("JOIN", "tatratatratatratra", "sahur");

            // Add new characters for logical operators
            AddCharacterForOperation("AND", "rhino", "toasterino");
            AddCharacterForOperation("OR", "bluberini", "octopusini");

            // Add new characters for functions
            AddCharacterForOperation("COUNT", "bearorito", "appletto");

            // Add new characters for transaction operations
            AddCharacterForOperation("BEGIN_TRANSACTION", "bicicletta", "gatto");
            AddCharacterForOperation("COMMIT", "bobrini", "cactusini");
            AddCharacterForOperation("ROLLBACK", "bang", "eid");
        }

        /// <summary>
        /// Helper method to add a character name for a specific SQL operation
        /// </summary>
        private static void AddCharacterForOperation(string operation, params string[] characterNames)
        {
            // Ensure the operation exists in the dictionary
            if (!Keywords.SqlOperationToKeywords.ContainsKey(operation))
            {
                Keywords.SqlOperationToKeywords[operation] = new List<string>();
            }

            // Add each character name to the list
            foreach (var name in characterNames)
            {
                if (!Keywords.SqlOperationToKeywords[operation].Contains(name))
                {
                    Keywords.SqlOperationToKeywords[operation].Add(name);
                }
            }

            // The reverse lookup dictionary would need to be rebuilt after adding new characters
            RebuildKeywordLookup();
        }

        /// <summary>
        /// Rebuilds the keyword-to-operation lookup dictionary after adding new characters
        /// </summary>
        private static void RebuildKeywordLookup()
        {
            // This would rebuild the private _keywordToOperation dictionary in Keywords
            // However, since that's a private field, we'd need a different approach in a real implementation

            // For demonstration purposes, this is how it would work conceptually:
            /*
            Dictionary<string, string> newLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var entry in Keywords.SqlOperationToKeywords)
            {
                foreach (var keyword in entry.Value)
                {
                    newLookup[keyword] = entry.Key;
                }
            }
            
            // Replace the old lookup dictionary with the new one
            _keywordToOperation = newLookup;
            */
        }

        /// <summary>
        /// Example of how to create entirely new SQL operations with custom characters
        /// </summary>
        public static void RegisterCustomSqlOperations()
        {
            // Add support for a completely new SQL operation
            Keywords.SqlOperationToKeywords["MERGE"] = new List<string> { "rantasanta", "chinaranta" };

            // Add support for window functions
            Keywords.SqlOperationToKeywords["OVER"] = new List<string> { "panini", "carbonara" };
            Keywords.SqlOperationToKeywords["PARTITION_BY"] = new List<string> { "pizzando", "mandolino" };

            // Add support for Common Table Expressions (CTEs)
            Keywords.SqlOperationToKeywords["WITH"] = new List<string> { "mozzarella", "burrata" };
            Keywords.SqlOperationToKeywords["AS"] = new List<string> { "primo", "secondo" };

            // Add support for UNION operations
            Keywords.SqlOperationToKeywords["UNION"] = new List<string> { "spaghetti", "ravioli" };
            Keywords.SqlOperationToKeywords["UNION_ALL"] = new List<string> { "spaghetti", "carbonara" };

            // Add support for ALTER operations
            Keywords.SqlOperationToKeywords["ALTER"] = new List<string> { "vespa", "ferrari" };
            Keywords.SqlOperationToKeywords["ADD_COLUMN"] = new List<string> { "gelato", "tiramisu" };

            // Rebuild the keyword lookup after adding new operations
            RebuildKeywordLookup();
        }
    }
}