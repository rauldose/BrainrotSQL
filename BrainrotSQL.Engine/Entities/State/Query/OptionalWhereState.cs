﻿using BrainrotSql.Engine.Constants;
using BrainrotSql.Engine.Entities.Exceptions;
using BrainrotSql.Engine.Entities.Model;
using BrainrotSql.Engine.Entities.State.Where;
using BrainrotSql.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BrainrotSql.Engine.Entities.Model.QueryInfo;

namespace BrainrotSql.Engine.Entities.State.Query
{
    /// <summary>
    ///  * State that allows for an optional WHERE clause, JOIN clause (only when the query is a select) or end of the query.
    /// </summary>
    public class OptionalWhereState : AbstractState
    {
        public OptionalWhereState(QueryInfo queryInfo) : base(queryInfo)
        {

        }
        public override AbstractState TransitionToNextState(string token)
        {
            List<string> expectedKeywords = new List<string> { Keywords.WHERE_KEYWORD };
            if (token.EqualsIgnoreCase(Keywords.WHERE_KEYWORD))
            {
                return new WhereFieldState(queryInfo);
            }
            if (queryInfo.GetQueryType().Equals(QueryType.SELECT))
            {
                expectedKeywords.AddRange(Keywords.JOIN_KEYWORDS);
                if (Keywords.JOIN_KEYWORDS.ToList().Contains(token))
                {
                    return new GreedyMatchKeywordState(queryInfo, Keywords.JOIN_KEYWORDS,
                            q => new AnyTokenConsumerState(q, q.AddJoinedTable, (q) => new OptionalWhereState(q)));
                }
            }
            throw new GlorboException(expectedKeywords, token);
        }

        public override bool IsFinalState()
        {
            return true;
        }
    }
}