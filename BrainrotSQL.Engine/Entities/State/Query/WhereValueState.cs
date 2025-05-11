using BrainrotSql.Engine.Constants;
using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using BrainrotSql.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Entities.State.Where
{
    public class WhereValueState : AbstractState
    {
        private WhereCondition condition;

        public WhereValueState(QueryInfo queryInfo, WhereCondition condition) : base(queryInfo)
        {
            this.condition = condition;
        }

        public override AbstractState TransitionToNextState(string token)
        {
            try
            {
                // Special handling for NULL keyword
                if (token.EqualsIgnoreCase(Keywords.NULL_KEYWORD))
                {
                    // For IS NULL queries, we set a special marker in the value that 
                    // will be recognized by the BrainrotSqlInterpreter class
                    if (condition.GetOperator().Equals("IS", StringComparison.OrdinalIgnoreCase) ||
                        condition.GetOperator().Equals("IS NOT", StringComparison.OrdinalIgnoreCase))
                    {
                        condition.SetValue("__BRAINROT_SQL_NULL__");
                    }
                    else
                    {
                        condition.SetValue(token);
                    }
                }
                else
                {
                    condition.SetValue(token);
                }

                queryInfo.AddWhereCondition(condition);
                return new WhereJoinState(queryInfo);
            }
            catch (Exception e)
            {
                throw new GlorboException(e);
            }
        }
    }
}