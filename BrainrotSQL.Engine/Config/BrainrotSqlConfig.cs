using BrainrotSql.Engine.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImprovedBrainrotSql.Engine.Config
{
    /// <summary>
    /// Configuration class for the Improved BrainrotSQL interpreter
    /// </summary>
    public static class BrainrotSqlConfig
    {
        /// <summary>
        /// Initialize the Enhanced BrainrotSQL engine with additional character mappings
        /// </summary>
        public static void Initialize()
        {
            // Optionally register custom characters
            CharacterExtensions.RegisterCustomCharacters();
        }

        /// <summary>
        /// Enable SQL features not supported in the basic version
        /// </summary>
        /// <param name="enableAdvancedClauses">Enable ORDER BY, GROUP BY, etc.</param>
        /// <param name="enableAggregateFunctions">Enable COUNT, SUM, etc.</param>
        /// <param name="enableLeftRightJoins">Enable LEFT, RIGHT, and FULL JOINs</param>
        /// <param name="enableSubqueries">Enable subquery support</param>
        public static void EnableAdvancedFeatures(
            bool enableAdvancedClauses = true,
            bool enableAggregateFunctions = true,
            bool enableLeftRightJoins = true,
            bool enableSubqueries = false)
        {
            // This method would enable or disable parsing of various SQL features
            // Implementation could set static flags that the parser checks

            // For now, all features are enabled by default in the parser
            // This method is a placeholder for future extension
        }

        /// <summary>
        /// Set the parsing mode for the BrainrotSQL interpreter
        /// </summary>
        /// <param name="strictMode">If true, requires exact keyword matches; if false, allows more flexibility</param>
        /// <param name="caseSensitive">If true, keywords are case-sensitive; if false, case-insensitive</param>
        public static void SetParsingMode(bool strictMode = false, bool caseSensitive = false)
        {
            // This method would configure how strictly the parser matches keywords
            // Again, this is a placeholder for future extension
        }
    }
}