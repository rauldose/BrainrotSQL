using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainrotSql.Engine.Constants;
using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using BrainrotSql.Engine.Extensions;
using static BrainrotSql.Engine.Entities.Model.QueryInfo;

namespace BrainrotSql.Engine
{
    /// <summary>
    /// A flexible parser for BrainrotSQL that supports multiple keyword synonyms
    /// </summary>
    public class FlexibleSqlParser
    {
        // Tokens from the query
        private List<string> tokens;

        // Current position in the token list
        private int position;

        // The result of the parsing
        private QueryInfo queryInfo;

        public FlexibleSqlParser(string query)
        {
            // Split the query into tokens
            tokens = query.SplitAndKeep(" ,()".ToCharArray())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim())
                .ToList();

            position = 0;
            queryInfo = new QueryInfo();
        }

        public QueryInfo Parse()
        {
            // Parse the query type
            ParseQueryType();

            // Parse the rest of the query based on the query type
            switch (queryInfo.GetQueryType())
            {
                case QueryType.SELECT:
                    ParseSelectQuery();
                    break;
                case QueryType.INSERT:
                    ParseInsertQuery();
                    break;
                case QueryType.UPDATE:
                    ParseUpdateQuery();
                    break;
                case QueryType.DELETE:
                    ParseDeleteQuery();
                    break;
                case QueryType.BEGIN_TRANSACTION:
                case QueryType.COMMIT:
                case QueryType.ROLLBACK:
                    // These are simple queries with no further parsing needed
                    break;
                default:
                    throw new GlorboException("Unknown query type. Brainrot intensifies!");
            }

            return queryInfo;
        }

        private void ParseQueryType()
        {
            if (position >= tokens.Count)
                throw new GlorboException("Empty query. No Brainrot detected!");

            string token = tokens[position];

            // Try to identify the query type based on the first keyword
            if (IsSelectKeyword(token))
            {
                queryInfo.SetType(QueryType.SELECT);
                position++; // Consume the SELECT token

                // Check for second SELECT keyword
                if (position < tokens.Count && IsSelectKeyword(tokens[position]))
                {
                    position++; // Consume the second SELECT token
                }
            }
            else if (IsInsertKeyword(token))
            {
                queryInfo.SetType(QueryType.INSERT);
                position++; // Consume the INSERT token

                // Check for second INSERT keyword
                if (position < tokens.Count && IsInsertKeyword(tokens[position]))
                {
                    position++; // Consume the second INSERT token
                }
            }
            else if (IsUpdateKeyword(token))
            {
                queryInfo.SetType(QueryType.UPDATE);
                position++; // Consume the UPDATE token

                // Check for second UPDATE keyword
                if (position < tokens.Count && IsUpdateKeyword(tokens[position]))
                {
                    position++; // Consume the second UPDATE token
                }
            }
            else if (IsDeleteKeyword(token))
            {
                queryInfo.SetType(QueryType.DELETE);
                position++; // Consume the DELETE token

                // Check for second DELETE keyword
                if (position < tokens.Count && IsDeleteKeyword(tokens[position]))
                {
                    position++; // Consume the second DELETE token
                }
            }
            else if (IsBeginTransactionKeyword(token))
            {
                queryInfo.SetType(QueryType.BEGIN_TRANSACTION);
                position++; // Consume the BEGIN TRANSACTION token

                // Check for second BEGIN TRANSACTION keyword
                if (position < tokens.Count && IsBeginTransactionKeyword(tokens[position]))
                {
                    position++; // Consume the second BEGIN TRANSACTION token
                }
            }
            else if (IsCommitKeyword(token))
            {
                queryInfo.SetType(QueryType.COMMIT);
                position++; // Consume the COMMIT token

                // Check for second COMMIT keyword
                if (position < tokens.Count && IsCommitKeyword(tokens[position]))
                {
                    position++; // Consume the second COMMIT token
                }
            }
            else if (IsRollbackKeyword(token))
            {
                queryInfo.SetType(QueryType.ROLLBACK);
                position++; // Consume the ROLLBACK token
            }
            else
            {
                var validOperations = new List<string>
                {
                    "tralalero", "cappuccino", "bombardiro", "bombombini", "tung", "ballerina", "chimpanzini"
                };

                throw new GlorboException(validOperations, token);
            }
        }

        private void ParseSelectQuery()
        {
            // Parse column list
            if (position < tokens.Count && IsAsteriskKeyword(tokens[position]))
            {
                // SELECT * (ALL)
                queryInfo.AddColumnName("*");
                position++; // Consume the asterisk token
            }
            else
            {
                // Parse comma-separated column list
                do
                {
                    if (position >= tokens.Count)
                        throw new GlorboException("Unexpected end of query. Expected column name or FROM keyword.");

                    string columnName = tokens[position++];

                    // Check if this is the FROM keyword
                    if (IsFromKeyword(columnName))
                    {
                        position--; // Move back to the FROM keyword
                        break;
                    }

                    queryInfo.AddColumnName(columnName);

                    // Check for comma
                    if (position < tokens.Count && tokens[position] == ",")
                        position++;

                } while (position < tokens.Count && !IsFromKeyword(tokens[position]));
            }

            // Parse FROM clause
            ConsumeFromKeyword();

            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected table name.");

            queryInfo.SetTableName(tokens[position++]);

            // Parse optional JOIN clauses
            while (position < tokens.Count && IsJoinKeyword(tokens[position]))
            {
                position++; // Consume the JOIN keyword

                // Check for second JOIN keyword
                if (position < tokens.Count && IsJoinKeyword(tokens[position]))
                {
                    position++; // Consume the second JOIN keyword
                }

                if (position >= tokens.Count)
                    throw new GlorboException("Unexpected end of query. Expected table name for JOIN.");

                queryInfo.AddJoinedTable(tokens[position++]);
            }

            // Parse optional WHERE clause
            if (position < tokens.Count && IsWhereKeyword(tokens[position]))
            {
                ParseWhereClause();
            }
        }

        private void ParseInsertQuery()
        {
            // Get the table name
            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected table name.");

            queryInfo.SetTableName(tokens[position++]);

            // Parse column list
            while (position < tokens.Count && !IsValuesKeyword(tokens[position]))
            {
                if (tokens[position] == ",")
                {
                    position++; // Skip comma
                    continue;
                }

                queryInfo.AddColumnName(tokens[position++]);
            }

            // Parse VALUES keyword
            ConsumeValuesKeyword();

            // Parse values list
            while (position < tokens.Count)
            {
                if (tokens[position] == ",")
                {
                    position++; // Skip comma
                    continue;
                }

                queryInfo.AddValue(tokens[position++]);
            }
        }

        private void ParseUpdateQuery()
        {
            // Get the table name
            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected table name.");

            queryInfo.SetTableName(tokens[position++]);

            // Parse SET keyword
            ConsumeSetKeyword();

            // Parse column-value pairs
            while (position < tokens.Count && !IsWhereKeyword(tokens[position]))
            {
                // Skip comma
                if (tokens[position] == ",")
                {
                    position++;
                    continue;
                }

                // Get column name
                if (position >= tokens.Count)
                    throw new GlorboException("Unexpected end of query. Expected column name.");

                queryInfo.AddColumnName(tokens[position++]);

                // Parse = or SET_EQUAL keyword
                ConsumeSetEqualKeyword();

                // Get value
                if (position >= tokens.Count)
                    throw new GlorboException("Unexpected end of query. Expected value.");

                queryInfo.AddValue(tokens[position++]);
            }

            // Parse optional WHERE clause
            if (position < tokens.Count && IsWhereKeyword(tokens[position]))
            {
                ParseWhereClause();
            }
        }

        private void ParseDeleteQuery()
        {
            // Parse FROM keywords
            ConsumeFromKeyword();

            // Get the table name
            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected table name.");

            queryInfo.SetTableName(tokens[position++]);

            // Parse optional WHERE clause
            if (position < tokens.Count && IsWhereKeyword(tokens[position]))
            {
                ParseWhereClause();
            }
        }

        private void ParseWhereClause()
        {
            // Consume WHERE keyword
            position++; // Already checked it's a WHERE keyword

            while (position < tokens.Count)
            {
                // Get the field name
                if (position >= tokens.Count)
                    throw new GlorboException("Unexpected end of query. Expected field name.");

                string fieldName = tokens[position++];
                var condition = new WhereCondition(fieldName);

                // Get the operator
                if (position >= tokens.Count)
                    throw new GlorboException("Unexpected end of query. Expected operator.");

                string op = tokens[position++];

                // Check for IS NULL or IS NOT NULL
                if (IsIsKeyword(op))
                {
                    // Check for NOT
                    if (position < tokens.Count && IsNotKeyword(tokens[position]))
                    {
                        position++; // Consume NOT
                        condition.SetOperator("IS NOT");
                    }
                    else
                    {
                        condition.SetOperator("IS");
                    }

                    // Get value (probably NULL)
                    if (position >= tokens.Count)
                        throw new GlorboException("Unexpected end of query. Expected NULL.");

                    string value = tokens[position++];
                    if (IsNullKeyword(value))
                    {
                        // Use a special marker for NULL values with IS operators
                        condition.SetValue("__BRAINROT_SQL_NULL__");
                    }
                    else
                    {
                        condition.SetValue(value);
                    }
                }
                else
                {
                    // Standard operator
                    condition.SetOperator(op);

                    // Get value
                    if (position >= tokens.Count)
                        throw new GlorboException("Unexpected end of query. Expected value.");

                    condition.SetValue(tokens[position++]);
                }

                // Add the condition
                queryInfo.AddWhereCondition(condition);

                // Check for AND/OR
                if (position < tokens.Count)
                {
                    if (IsAndKeyword(tokens[position]))
                    {
                        position++; // Consume AND
                        queryInfo.AddWhereConditionsJoinOperator("AND");
                    }
                    else if (IsOrKeyword(tokens[position]))
                    {
                        position++; // Consume OR
                        queryInfo.AddWhereConditionsJoinOperator("OR");
                    }
                    else
                    {
                        // Not a join operator, so we're done with the WHERE clause
                        break;
                    }
                }
                else
                {
                    // End of query
                    break;
                }
            }
        }

        // Helper methods for checking token types

        private bool IsSelectKeyword(string token)
        {
            return Keywords.SELECT_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsInsertKeyword(string token)
        {
            return Keywords.INSERT_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsUpdateKeyword(string token)
        {
            return Keywords.UPDATE_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsDeleteKeyword(string token)
        {
            return Keywords.DELETE_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsBeginTransactionKeyword(string token)
        {
            return Keywords.BEGIN_TRANSACTION_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsCommitKeyword(string token)
        {
            return Keywords.COMMIT_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsRollbackKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.ROLLBACK_KEYWORD);
        }

        private bool IsFromKeyword(string token)
        {
            return Keywords.FROM_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsJoinKeyword(string token)
        {
            return Keywords.JOIN_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsWhereKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.WHERE_KEYWORD);
        }

        private bool IsAsteriskKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.ASTERISK_KEYWORD);
        }

        private bool IsValuesKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.VALUES_KEYWORD);
        }

        private bool IsSetKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.SET_KEYWORD);
        }

        private bool IsSetEqualKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.SET_EQUAL_KEYWORD);
        }

        private bool IsIsKeyword(string token)
        {
            return Keywords.IS_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsNotKeyword(string token)
        {
            return Keywords.IS_NOT_KEYWORDS.Any(k => token.EqualsIgnoreCase(k));
        }

        private bool IsNullKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.NULL_KEYWORD);
        }

        private bool IsAndKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.AND_KEYWORD);
        }

        private bool IsOrKeyword(string token)
        {
            return token.EqualsIgnoreCase(Keywords.OR_KEYWORD);
        }

        // Helper methods to consume specific keywords

        private void ConsumeFromKeyword()
        {
            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected FROM keyword.");

            string token = tokens[position];
            if (IsFromKeyword(token))
            {
                position++; // Consume the FROM keyword

                // Look for a second FROM keyword (if it exists)
                if (position < tokens.Count && IsFromKeyword(tokens[position]))
                {
                    position++; // Consume the second FROM keyword
                }
            }
            else
            {
                throw new GlorboException(Keywords.FROM_KEYWORDS.FirstOrDefault() ?? "FROM", token);
            }
        }

        private void ConsumeValuesKeyword()
        {
            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected VALUES keyword.");

            string token = tokens[position];
            if (IsValuesKeyword(token))
            {
                position++; // Consume the VALUES keyword
            }
            else
            {
                throw new GlorboException(Keywords.VALUES_KEYWORD, token);
            }
        }

        private void ConsumeSetKeyword()
        {
            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected SET keyword.");

            string token = tokens[position];
            if (IsSetKeyword(token))
            {
                position++; // Consume the SET keyword
            }
            else
            {
                throw new GlorboException(Keywords.SET_KEYWORD, token);
            }
        }

        private void ConsumeSetEqualKeyword()
        {
            if (position >= tokens.Count)
                throw new GlorboException("Unexpected end of query. Expected SET_EQUAL keyword or '='.");

            string token = tokens[position];
            if (IsSetEqualKeyword(token) || token == "=")
            {
                position++; // Consume the SET_EQUAL keyword or '='
            }
            else
            {
                throw new GlorboException($"{Keywords.SET_EQUAL_KEYWORD} or =", token);
            }
        }
    }
}