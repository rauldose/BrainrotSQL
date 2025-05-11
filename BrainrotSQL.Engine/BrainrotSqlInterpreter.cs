using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using BrainrotSql.Engine.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static BrainrotSql.Engine.Entities.Model.QueryInfo;

namespace BrainrotSql.Engine
{
    /// <summary>
    /// An improved interpreter for BrainrotSQL that uses the flexible parser
    /// and supports more advanced SQL features
    /// </summary>
    public class ImprovedBrainrotSqlInterpreter
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public ImprovedBrainrotSqlInterpreter(IDbConnection connection)
        {
            _connection = connection;
            _transaction = null;
        }

        public static string ToSqlQuery(string brainrotQuery)
        {
            // Use the flexible parser to parse the query
            FlexibleSqlParser parser = new FlexibleSqlParser(brainrotQuery);
            QueryInfo info = parser.Parse();
            string sqlQuery = BuildSqlQuery(info);

            return sqlQuery;
        }

        private static string BuildSqlQuery(QueryInfo queryInfo)
        {
            switch (queryInfo.GetQueryType())
            {
                case QueryType.SELECT:
                    return BuildSelectQuery(queryInfo);
                case QueryType.INSERT:
                    return BuildInsertQuery(queryInfo);
                case QueryType.UPDATE:
                    return BuildUpdateQuery(queryInfo);
                case QueryType.DELETE:
                    return BuildDeleteQuery(queryInfo);
                case QueryType.BEGIN_TRANSACTION:
                    return "START TRANSACTION";
                case QueryType.COMMIT:
                    return "COMMIT";
                case QueryType.ROLLBACK:
                    return "ROLLBACK";
                default:
                    throw new GlorboException("Unrecognized query type. Brainrot intensifies!");
            }
        }

        private static string BuildSelectQuery(QueryInfo queryInfo)
        {
            StringBuilder query = new StringBuilder("SELECT ");

            // Process column names
            query.Append(string.Join(", ", queryInfo.GetColumnNames()));

            // Table name
            query.Append(" FROM ").Append(queryInfo.GetTableName());

            // Join clauses
            List<string> joinedTables = queryInfo.GetJoinedTables();
            if (joinedTables.Count > 0)
            {
                query.Append(" INNER JOIN ")
                     .Append(string.Join(" INNER JOIN ", joinedTables));
            }

            // Where clause
            string whereClause = BuildWhereClause(queryInfo);
            if (!string.IsNullOrEmpty(whereClause))
            {
                query.Append(whereClause);
            }

            return query.ToString();
        }

        private static string BuildInsertQuery(QueryInfo queryInfo)
        {
            StringBuilder query = new StringBuilder("INSERT INTO ")
                .Append(queryInfo.GetTableName());

            // Column names if specified
            List<string> columnNames = queryInfo.GetColumnNames();
            if (columnNames.Count > 0)
            {
                query.Append(" (")
                     .Append(string.Join(", ", columnNames))
                     .Append(")");
            }

            // Values
            query.Append(" VALUES (")
                 .Append(string.Join(", ", queryInfo.GetValues()))
                 .Append(")");

            return query.ToString();
        }

        private static string BuildUpdateQuery(QueryInfo queryInfo)
        {
            StringBuilder query = new StringBuilder("UPDATE ")
                .Append(queryInfo.GetTableName())
                .Append(" SET ");

            // Column=Value pairs
            List<string> columnNames = queryInfo.GetColumnNames();
            List<string> values = queryInfo.GetValues();

            for (int i = 0; i < columnNames.Count; i++)
            {
                if (i > 0) query.Append(", ");
                query.Append(columnNames[i])
                     .Append(" = ")
                     .Append(values[i]);
            }

            // Where clause
            string whereClause = BuildWhereClause(queryInfo);
            if (!string.IsNullOrEmpty(whereClause))
            {
                query.Append(whereClause);
            }

            return query.ToString();
        }

        private static string BuildDeleteQuery(QueryInfo queryInfo)
        {
            StringBuilder query = new StringBuilder("DELETE FROM ")
                .Append(queryInfo.GetTableName());

            // Where clause
            string whereClause = BuildWhereClause(queryInfo);
            if (!string.IsNullOrEmpty(whereClause))
            {
                query.Append(whereClause);
            }

            return query.ToString();
        }

        private static string BuildWhereClause(QueryInfo queryInfo)
        {
            List<WhereCondition> conditions = queryInfo.GetWhereConditions();
            if (conditions.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder whereClause = new StringBuilder(" WHERE ");

            for (int i = 0; i < conditions.Count; i++)
            {
                if (i > 0)
                {
                    whereClause.Append(" ")
                               .Append(queryInfo.GetWhereConditionsJoinOperators()[i - 1])
                               .Append(" ");
                }

                WhereCondition condition = conditions[i];

                // Special handling for our NULL marker value
                if (condition.GetValue() == "__BRAINROT_SQL_NULL__")
                {
                    whereClause.Append(condition.GetField())
                           .Append(" ")
                           .Append(condition.GetOperator())
                           .Append(" NULL");
                }
                // Also handle "girafa" as NULL for IS operators
                else if ((condition.GetOperator().ToUpper() == "IS" ||
                          condition.GetOperator().ToUpper() == "IS NOT") &&
                         condition.GetValue().ToLower() == Keywords.NULL_KEYWORD.ToLower())
                {
                    whereClause.Append(condition.GetField())
                           .Append(" ")
                           .Append(condition.GetOperator())
                           .Append(" NULL");
                }
                // Regular case - just append normally
                else
                {
                    whereClause.Append(condition.GetField())
                           .Append(" ")
                           .Append(condition.GetOperator())
                           .Append(" ")
                           .Append(condition.GetValue());
                }
            }

            return whereClause.ToString();
        }

        public BrainrotSqlResult Execute(string brainrotQuery)
        {
            // Parse the query
            FlexibleSqlParser parser = new FlexibleSqlParser(brainrotQuery);
            QueryInfo queryInfo = parser.Parse();

            // Convert to SQL
            string sqlQuery = BuildSqlQuery(queryInfo);

            // Create the result object
            BrainrotSqlResult result = new BrainrotSqlResult();

            // Execute the query
            try
            {
                IDbCommand command = _connection.CreateCommand();
                command.CommandText = sqlQuery;

                // Set the transaction if there is one
                if (_transaction != null)
                {
                    command.Transaction = _transaction;
                }

                // Execute based on query type
                switch (queryInfo.GetQueryType())
                {
                    case QueryType.SELECT:
                        IDataReader reader = command.ExecuteReader();
                        result.SetResultSet(reader);
                        break;

                    case QueryType.INSERT:
                    case QueryType.UPDATE:
                    case QueryType.DELETE:
                        int rowsAffected = command.ExecuteNonQuery();
                        result.SetAffectedRows(rowsAffected);
                        break;

                    case QueryType.BEGIN_TRANSACTION:
                        if (_transaction == null)
                        {
                            _transaction = _connection.BeginTransaction();
                        }
                        else
                        {
                            throw new GlorboException("Transaction already in progress!");
                        }
                        break;

                    case QueryType.COMMIT:
                        if (_transaction != null)
                        {
                            _transaction.Commit();
                            _transaction = null;
                        }
                        else
                        {
                            throw new GlorboException("No transaction in progress to commit!");
                        }
                        break;

                    case QueryType.ROLLBACK:
                        if (_transaction != null)
                        {
                            _transaction.Rollback();
                            _transaction = null;
                        }
                        else
                        {
                            throw new GlorboException("No transaction in progress to rollback!");
                        }
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new GlorboException(ex);
            }
        }
    }
}