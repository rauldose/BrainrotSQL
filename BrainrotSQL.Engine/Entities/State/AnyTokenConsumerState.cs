using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Entities.State
{
    public class AnyTokenConsumerState : AbstractState
    {
        private Action<string> _tokenAction;
        private Func<QueryInfo, AbstractState> _transitionFunction;

        public AnyTokenConsumerState(QueryInfo queryInfo, Action<String> tokenConsumer,
                Func<QueryInfo, AbstractState> transitionFunction) : base(queryInfo)
        {
            this._tokenAction = tokenConsumer;
            this._transitionFunction = transitionFunction;
        }

        public override AbstractState TransitionToNextState(string token)
        {
            try
            {
                _tokenAction?.Invoke(token);
                return _transitionFunction?.Invoke(queryInfo);
            }
            catch (Exception e)
            {
                throw new GlorboException(e);
            }
        }
    }
}