﻿using System.Collections.Generic;

namespace NRules.Rule
{
    internal class CollectionAggregate<T> : IAggregate
    {
        private readonly List<T> _items = new List<T>();
        private bool _initialized;

        public AggregationResults Add(object fact)
        {
            _items.Add((T) fact);
            if (!_initialized)
            {
                _initialized = true;
                return AggregationResults.Added;
            }
            return AggregationResults.Modified;
        }

        public AggregationResults Modify(object fact)
        {
            return AggregationResults.Modified;
        }

        public AggregationResults Remove(object fact)
        {
            _items.Remove((T) fact);
            if (_items.Count > 0)
            {
                return AggregationResults.Modified;
            }
            return AggregationResults.Removed;
        }

        public object Result
        {
            get { return _items; }
        }
    }
}