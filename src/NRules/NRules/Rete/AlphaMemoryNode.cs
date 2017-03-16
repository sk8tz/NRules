﻿using System.Collections.Generic;

namespace NRules.Rete
{
    internal interface IAlphaMemoryNode : IObjectSource
    {
        IEnumerable<IObjectSink> Sinks { get; }
    }

    internal class AlphaMemoryNode : IObjectSink, IAlphaMemoryNode
    {
        private readonly List<IObjectSink> _sinks = new List<IObjectSink>();

        public IEnumerable<IObjectSink> Sinks { get { return _sinks; } }

        public void PropagateAssert(IExecutionContext context, Fact fact)
        {
            IAlphaMemory memory = context.WorkingMemory.GetNodeMemory(this);
            foreach (var sink in _sinks)
            {
                sink.PropagateAssert(context, fact);
            }
            memory.Add(fact);
        }

        public void PropagateUpdate(IExecutionContext context, Fact fact)
        {
            IAlphaMemory memory = context.WorkingMemory.GetNodeMemory(this);
            if (memory.Contains(fact))
            {
                foreach (var sink in _sinks)
                {
                    sink.PropagateUpdate(context, fact);
                }
            }
            else
            {
                PropagateAssert(context, fact);
            }
        }

        public void PropagateRetract(IExecutionContext context, Fact fact)
        {
            IAlphaMemory memory = context.WorkingMemory.GetNodeMemory(this);
            if (memory.Contains(fact))
            {
                foreach (var sink in _sinks)
                {
                    sink.PropagateRetract(context, fact);
                }
                memory.Remove(fact);
            }
        }

        public IEnumerable<Fact> GetFacts(IExecutionContext context)
        {
            IAlphaMemory memory = context.WorkingMemory.GetNodeMemory(this);
            return memory.Facts;
        }

        public void Attach(IObjectSink sink)
        {
            _sinks.Add(sink);
        }

        public void Accept<TContext>(TContext context, ReteNodeVisitor<TContext> visitor)
        {
            visitor.VisitAlphaMemoryNode(context, this);
        }
    }
}