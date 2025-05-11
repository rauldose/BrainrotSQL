using BrainrotSql.Engine.Constants;
using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using BrainrotSql.Engine.Entities.State.Where;
using BrainrotSql.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Entities.State.Query
{
    public class UpdateSetState : AbstractState
    {
        public UpdateSetState(QueryInfo queryInfo) : base(queryInfo)
        {

        }
        public override AbstractState TransitionToNextState(string token)
        {
            // Adds another variable
            if (token.EqualsIgnoreCase(","))
            {
                return new AnyTokenConsumerState(queryInfo, queryInfo.AddColumnName,
                        q => new SingleTokenMatchState(q, Keywords.SET_EQUAL_KEYWORD,
                                q2 => new AnyTokenConsumerState(q2, q2.AddValue, (q) => new UpdateSetState(q))));
            }
            // Moves to a where clause
            if (token.EqualsIgnoreCase(Keywords.WHERE_KEYWORD))
            {
                return new WhereFieldState(queryInfo);
            }
            throw new GlorboException(new List<string> { ",", Keywords.WHERE_KEYWORD, "%END_OF_QUERY%" }, token);
        }


        public override bool IsFinalState()
        {
            return true;
        }
    }
}