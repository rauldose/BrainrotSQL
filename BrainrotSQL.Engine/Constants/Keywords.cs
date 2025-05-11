using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Constants
{
    public static class Keywords
    {
        // Dictionary mapping SQL operations to lists of synonymous Brainrot character names
        public static Dictionary<string, List<string>> SqlOperationToKeywords = new Dictionary<string, List<string>>
        {
            // Basic operations
            { "SELECT", new List<string> { "tralalero", "tralala", "tralaleritos", "troppi", "trippa" } },
            { "UPDATE", new List<string> { "cappuccino", "assassino", "capuccina", "caramello" } },
            { "INSERT", new List<string> { "bombardiro", "crocodilo", "bardiro", "bombardi" } },
            { "DELETE", new List<string> { "bombombini", "gusini", "bomby", "bombiti" } },
            { "FROM", new List<string> { "lirili", "larila", "lirilì", "larilà" } },
            { "WHERE", new List<string> { "patapim", "brrbrr", "brrbrius", "patapum" } },
            { "JOIN", new List<string> { "boneca", "ambalabu", "bonecus", "ambalabra" } },
            
            // Modifiers
            { "ORDER_BY", new List<string> { "graipussi", "medussi", "octopussini" } },
            { "GROUP_BY", new List<string> { "rhino", "toasterino", "rhinus" } },
            { "HAVING", new List<string> { "blueberrinni", "bluberini", "blueberry" } },
            { "LIMIT", new List<string> { "glorbo", "fruttodrillo", "glorbus" } },
            
            // Logical operators
            { "AND", new List<string> { "camelo", "frigo", "frigus", "camelus" } },
            { "OR", new List<string> { "vaca", "saturno", "vacus", "saturnito" } },
            { "NOT", new List<string> { "zibra", "zubra", "zibrus", "zubralini" } },
            
            // Special operators
            { "IS", new List<string> { "bobrito", "bandito", "bobrini", "banditi" } },
            { "IN", new List<string> { "orangutini", "ananasini", "orangutan" } },
            { "LIKE", new List<string> { "cacto", "hipopotamo", "cactus", "hippus" } },
            { "BETWEEN", new List<string> { "burbaloni", "lulilolli", "burba", "luli" } },
            
            // JOIN types
            { "INNER_JOIN", new List<string> { "boneca", "ambalabu" } },
            { "LEFT_JOIN", new List<string> { "frulli", "frulla" } },
            { "RIGHT_JOIN", new List<string> { "bobrito", "bandito" } },
            { "FULL_JOIN", new List<string> { "cocofanto", "elefanto" } },
            
            // Functions
            { "COUNT", new List<string> { "tric", "trac", "baraboom" } },
            { "SUM", new List<string> { "girafa", "celestre", "girafus" } },
            { "AVG", new List<string> { "dindin", "dunma", "dindindin" } },
            { "MAX", new List<string> { "ballerina", "cappucina" } },
            { "MIN", new List<string> { "trulimero", "trulicina" } },
            
            // Other SQL keywords
            { "DISTINCT", new List<string> { "chimpanzini", "bananini", "bananino" } },
            { "NULL", new List<string> { "girafa", "celestre", "celestra" } },
            { "DESC", new List<string> { "hotspot", "bro", "hotspotty" } },
            { "ASC", new List<string> { "rantasanta", "chinaranta", "rantus" } },
            
            // Transaction keywords
            { "BEGIN_TRANSACTION", new List<string> { "tung", "sahur", "tung", "tung" } },
            { "COMMIT", new List<string> { "ballerina", "cappuccina" } },
            { "ROLLBACK", new List<string> { "chimpanzini", "banana", "chimp" } },

            // Values and operators
            { "VALUES", new List<string> { "trulimero", "valuerus", "valus" } },
            { "SET", new List<string> { "cocofanto", "elefanto", "cococo" } },
            { "SET_EQUAL", new List<string> { "burbaloni", "lulilolli", "burburifici" } },
            { "ALL", new List<string> { "frulli", "frulla", "frullifrullo" } },
            
            // Table operations
            { "CREATE_TABLE", new List<string> { "tric", "trac", "baraboom", "create" } },
            { "DROP_TABLE", new List<string> { "bang", "bang", "eid", "droppi" } },
            { "ALTER_TABLE", new List<string> { "bearorito", "applepitolirotito", "alteri" } },
            
            // Index operations
            { "CREATE_INDEX", new List<string> { "bicicletta", "gatto", "santo", "indexi" } },
            { "DROP_INDEX", new List<string> { "bobrini", "cactusini", "saturno", "indexdrop" } }
        };

        // Reverse lookup for validation
        private static Dictionary<string, string> _keywordToOperation;

        static Keywords()
        {
            // Initialize the reverse lookup dictionary
            _keywordToOperation = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in SqlOperationToKeywords)
            {
                foreach (var keyword in entry.Value)
                {
                    _keywordToOperation[keyword] = entry.Key;
                }
            }
        }

        // Check if a token is a valid keyword
        public static bool IsValidKeyword(string token)
        {
            return _keywordToOperation.ContainsKey(token);
        }

        // Get the SQL operation for a given token
        public static string GetSqlOperation(string token)
        {
            return _keywordToOperation.TryGetValue(token, out var operation) ? operation : null;
        }

        // Check if a token belongs to a specific operation
        public static bool IsOperationKeyword(string token, string operation)
        {
            return _keywordToOperation.TryGetValue(token, out var foundOperation) &&
                   foundOperation.Equals(operation, StringComparison.OrdinalIgnoreCase);
        }

        // Get all keywords for a specific operation
        public static List<string> GetKeywordsForOperation(string operation)
        {
            return SqlOperationToKeywords.TryGetValue(operation, out var keywords) ? keywords : new List<string>();
        }

        // For backward compatibility
        public static string[] SELECT_KEYWORDS = SqlOperationToKeywords["SELECT"].ToArray();
        public static string[] UPDATE_KEYWORDS = SqlOperationToKeywords["UPDATE"].ToArray();
        public static string[] INSERT_KEYWORDS = SqlOperationToKeywords["INSERT"].ToArray();
        public static string[] DELETE_KEYWORDS = SqlOperationToKeywords["DELETE"].ToArray();
        public static string[] JOIN_KEYWORDS = SqlOperationToKeywords["JOIN"].ToArray();
        public static string[] FROM_KEYWORDS = SqlOperationToKeywords["FROM"].ToArray();
        public static string ASTERISK_KEYWORD = SqlOperationToKeywords["ALL"][0];
        public static string WHERE_KEYWORD = SqlOperationToKeywords["WHERE"][0];
        public static string[] BEGIN_TRANSACTION_KEYWORDS = SqlOperationToKeywords["BEGIN_TRANSACTION"].ToArray();
        public static string[] COMMIT_KEYWORDS = SqlOperationToKeywords["COMMIT"].ToArray();
        public static string ROLLBACK_KEYWORD = SqlOperationToKeywords["ROLLBACK"][0];
        public static string AND_KEYWORD = SqlOperationToKeywords["AND"][0];
        public static string OR_KEYWORD = SqlOperationToKeywords["OR"][0];
        public static string NULL_KEYWORD = SqlOperationToKeywords["NULL"][0];
        public static string[] IS_KEYWORDS = SqlOperationToKeywords["IS"].ToArray();
        public static string VALUES_KEYWORD = SqlOperationToKeywords["VALUES"][0];
        public static string[] IS_NOT_KEYWORDS = { "zibra", SqlOperationToKeywords["IS"][0] };
        public static string SET_KEYWORD = SqlOperationToKeywords["SET"][0];
        public static readonly string SET_EQUAL_KEYWORD = SqlOperationToKeywords["SET_EQUAL"][0];

        // Where operators would need to be updated to match new keywords
        public static List<string> WHERE_OPERATORS = new List<string>{">", "<", "=", "!=", "<>", ">=", "<=",
                IS_KEYWORDS[0], IS_NOT_KEYWORDS[0]};
    }
}