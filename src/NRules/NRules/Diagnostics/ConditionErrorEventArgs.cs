using NRules.Rete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tuple = NRules.Rete.Tuple;

namespace NRules.Diagnostics
{
    /// <summary>
    /// Information related to error events raised during condition evaluation.
    /// </summary>
    public class ConditionErrorEventArgs : ErrorEventArgs
    {
        private readonly Expression _expression;
        private readonly Tuple _tuple;
        private readonly Fact _fact;

        internal ConditionErrorEventArgs(Exception exception, Expression expression, Tuple tuple, Fact fact)
            : base(exception)
        {
            _expression = expression;
            _tuple = tuple;
            _fact = fact;
        }

        /// <summary>
        /// Condition that caused exception.
        /// </summary>
        public Expression Condition { get { return _expression; } }

        /// <summary>
        /// Facts that caused exception.
        /// </summary>
        public IEnumerable<FactInfo> Facts
        {
            get
            {
                var wrappedFact = new[] { new FactInfo(_fact) };
                return _tuple == null
                    ? wrappedFact
                    : _tuple.Facts.Reverse().Select(x => new FactInfo(x)).Concat(wrappedFact).ToArray();
            }
        }
    }
}