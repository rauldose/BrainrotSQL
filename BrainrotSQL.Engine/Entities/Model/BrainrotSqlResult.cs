using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BrainrotSql.Engine.Entities.Model
{
    public class BrainrotSqlResult
    {
        private int affectedRows;
        private IDataReader resultSet;

        public int GetAffectedRows()
        {
            return affectedRows;
        }

        public void SetAffectedRows(int affectedRows)
        {
            this.affectedRows = affectedRows;
        }

        public IDataReader GetResultSet()
        {
            return resultSet;
        }

        public void SetResultSet(IDataReader resultSet)
        {
            this.resultSet = resultSet;
        }
    }
}