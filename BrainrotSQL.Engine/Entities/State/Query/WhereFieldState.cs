using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Entities.State.Where
{
    public class WhereFieldState : AbstractState
    {
        public WhereFieldState(QueryInfo queryInfo) : base(queryInfo)
        {

        }

        public override AbstractState TransitionToNextState(String token)
        {
            try
            {
                return new WhereOperatorState(queryInfo, new WhereCondition(token));
            }

            catch (Exception e)
            {
                throw new GlorboException(e);
            }
        }
    }
}