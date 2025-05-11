using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using BrainrotSql.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Entities.State
{
    /// <summary>
    /// States that proceeds if a specific token is matched exactly.
    /// </summary>
    public class SingleTokenMatchState : AbstractState
    {
        private string expectedToken;
        private Func<QueryInfo, AbstractState> transitionFunction;

        public SingleTokenMatchState(QueryInfo queryInfo, String expectedToken,
                Func<QueryInfo, AbstractState> transitionFunction) : base(queryInfo)
        {
            this.expectedToken = expectedToken;
            this.transitionFunction = transitionFunction;
        }

        public override AbstractState TransitionToNextState(string token)
        {
            if (token.EqualsIgnoreCase(expectedToken))
            {
                return transitionFunction?.Invoke(queryInfo);
            }
            throw new GlorboException(expectedToken, token);
        }
    }
}