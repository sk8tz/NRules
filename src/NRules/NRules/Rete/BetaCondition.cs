﻿using NRules.Utilities;
using System;
using System.Linq.Expressions;

namespace NRules.Rete
{
    internal interface IBetaCondition
    {
        bool IsSatisfiedBy(IExecutionContext context, Tuple leftTuple, Fact rightFact);
    }

    internal class BetaCondition : IBetaCondition, IEquatable<BetaCondition>
    {
        private readonly LambdaExpression _expression;
        private readonly IndexMap _factIndexMap;
        private readonly FastDelegate<Func<object[], bool>> _compiledExpression;

        public BetaCondition(LambdaExpression expression, IndexMap factIndexMap)
        {
            _expression = expression;
            _factIndexMap = factIndexMap;
            _compiledExpression = FastDelegate.Create<Func<object[], bool>>(expression);
        }

        public bool IsSatisfiedBy(IExecutionContext context, Tuple leftTuple, Fact rightFact)
        {
            var args = new object[_compiledExpression.ParameterCount];
            int index = leftTuple.Count - 1;
            foreach (var fact in leftTuple.Facts)
            {
                IndexMap.SetElementAt(ref args, _factIndexMap[index], 0, fact.Object);
                index--;
            }
            IndexMap.SetElementAt(ref args, _factIndexMap[leftTuple.Count], 0, rightFact.Object);

            try
            {
                return _compiledExpression.Delegate(args);
            }
            catch (Exception e)
            {
                context.EventAggregator.RaiseConditionFailed(context.Session, e, _expression, leftTuple, rightFact);
                throw new RuleConditionEvaluationException("Failed to evaluate condition", _expression.ToString(), e);
            }
        }

        public bool Equals(BetaCondition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ExpressionComparer.AreEqual(_expression, other._expression);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BetaCondition)obj);
        }

        public override int GetHashCode()
        {
            return (_expression != null ? _expression.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return _expression.ToString();
        }
    }
}