using BrainrotSql.Engine.Constants;
using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using BrainrotSql.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrainrotSql.Engine.Entities.State.Where
{
    public class WhereOperatorState : AbstractState
    {
        private WhereCondition condition;

        public WhereOperatorState(QueryInfo queryInfo, WhereCondition condition) : base(queryInfo)
        {
            this.condition = condition;
        }

        public override AbstractState TransitionToNextState(string token)
        {
            if (Keywords.WHERE_OPERATORS.Contains(token) || Keywords.IS_KEYWORDS.ToList().Contains(token))
            {
                if (token.EqualsIgnoreCase(Keywords.IS_NOT_KEYWORDS[0]))
                {
                    // Set operator to SQL standard "IS NOT"
                    condition.SetOperator("IS NOT");
                    return new GreedyMatchKeywordState(queryInfo, Keywords.IS_NOT_KEYWORDS, q => new WhereValueState(q, condition));
                }
                if (Keywords.IS_KEYWORDS.ToList().Contains(token))
                {
                    // Set operator to SQL standard "IS"
                    condition.SetOperator("IS");
                }
                else
                {
                    condition.SetOperator(token);
                }
                return new WhereValueState(queryInfo, condition);
            }
            throw new GlorboException(Keywords.WHERE_OPERATORS, token);
        }
    }
}