﻿using System.Collections.Generic;
using System.Linq;

namespace NRules.Rete
{
    internal class ObjectInputAdapter : ITupleSink, IAlphaMemoryNode
    {
        private readonly ITupleSource _source;
        private readonly List<IObjectSink> _sinks = new List<IObjectSink>();

        public IEnumerable<IObjectSink> Sinks { get { return _sinks; } }

        public ObjectInputAdapter(IBetaMemoryNode source)
        {
            _source = source;
            source.Attach(this);
        }

        public void PropagateAssert(IExecutionContext context, Tuple tuple)
        {
            var wrapperFact = new WrapperFact(tuple);
            context.WorkingMemory.SetInternalFact(this, wrapperFact);
            foreach (var sink in _sinks)
            {
                sink.PropagateAssert(context, wrapperFact);
            }
        }

        public void PropagateUpdate(IExecutionContext context, Tuple tuple)
        {
            var wrapperFact = context.WorkingMemory.GetInternalFact(this, tuple);
            foreach (var sink in _sinks)
            {
                sink.PropagateUpdate(context, wrapperFact);
            }
        }

        public void PropagateRetract(IExecutionContext context, Tuple tuple)
        {
            var wrapperFact = context.WorkingMemory.GetInternalFact(this, tuple);
            foreach (var sink in _sinks)
            {
                sink.PropagateRetract(context, wrapperFact);
            }
            context.WorkingMemory.RemoveInternalFact(this, wrapperFact);
        }

        public IEnumerable<Fact> GetFacts(IExecutionContext context)
        {
            var sourceTuples = _source.GetTuples(context);
            var sourceFacts = sourceTuples.Select(t => context.WorkingMemory.GetInternalFact(this, t));
            return sourceFacts;
        }

        public void Attach(IObjectSink sink)
        {
            _sinks.Add(sink);
        }

        public void Accept<TContext>(TContext context, ReteNodeVisitor<TContext> visitor)
        {
            visitor.VisitObjectInputAdapter(context, this);
        }
    }
}