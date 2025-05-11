using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Entities.State
{
    /// <summary>
    /// State that represents the end of a query.
    /// </summary>
    public class FinalState : AbstractState
    {
        public FinalState(QueryInfo queryInfo) : base(queryInfo)
        {

        }
        public override AbstractState TransitionToNextState(string token)
        {
            throw new GlorboException("%END_OF_QUERY%", token);
        }

        public override bool IsFinalState()
        {
            return true;
        }
    }
}