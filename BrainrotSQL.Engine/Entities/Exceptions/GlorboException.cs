using System;
using System.Collections.Generic;
using System.Text;

namespace BrainrotSql.Engine.Entities.Exceptions
{
    /// <summary>
    ///  Exception thrown by BrainrotSql when an error occurs during query parsing/execution.
    ///  Named after "Glorbo Fruttodrillo", one of the Italian Brainrot characters.
    /// </summary>
    public class GlorboException : Exception
    {
        public GlorboException(List<string> expectedTokens, string actualToken) : base($"Brainrot Error: Expected one of {string.Join(", ", expectedTokens)} but got [{actualToken}]")
        {

        }

        public GlorboException(string expectedToken, string token) : base($"Brainrot Error: Expected {expectedToken} but got [{token}]")
        {

        }

        public GlorboException(string message) : base($"Brainrot Error: {message}")
        {

        }

        public GlorboException(Exception e) : base($"Brainrot Error: {e.Message}", e)
        {

        }
    }
}