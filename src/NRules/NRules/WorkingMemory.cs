﻿using NRules.Rete;
using System.Collections.Generic;

namespace NRules
{
    internal interface IWorkingMemory
    {
        IEnumerable<Fact> Facts { get; }

        Fact GetFact(object factObject);

        void SetFact(Fact fact);

        void RemoveFact(Fact fact);

        Fact GetInternalFact(INode node, object factObject);

        void SetInternalFact(INode node, Fact fact);

        void RemoveInternalFact(INode node, Fact fact);

        IAlphaMemory GetNodeMemory(IAlphaMemoryNode node);

        IBetaMemory GetNodeMemory(IBetaMemoryNode node);
    }

    internal class WorkingMemory : IWorkingMemory
    {
        private readonly Dictionary<object, Fact> _factMap = new Dictionary<object, Fact>();
        private readonly Dictionary<INode, Dictionary<object, Fact>> _internalFactMap = new Dictionary<INode, Dictionary<object, Fact>>();

        private readonly Dictionary<IAlphaMemoryNode, IAlphaMemory> _alphaMap =
            new Dictionary<IAlphaMemoryNode, IAlphaMemory>();

        private readonly Dictionary<IBetaMemoryNode, IBetaMemory> _betaMap =
            new Dictionary<IBetaMemoryNode, IBetaMemory>();

        public IEnumerable<Fact> Facts { get { return _factMap.Values; } }

        public Fact GetFact(object factObject)
        {
            Fact fact;
            _factMap.TryGetValue(factObject, out fact);
            return fact;
        }

        public void SetFact(Fact fact)
        {
            _factMap[fact.RawObject] = fact;
        }

        public void RemoveFact(Fact fact)
        {
            _factMap.Remove(fact.RawObject);
        }

        public Fact GetInternalFact(INode node, object factObject)
        {
            Dictionary<object, Fact> factMap;
            if (!_internalFactMap.TryGetValue(node, out factMap)) 
                return null;

            Fact fact;
            factMap.TryGetValue(factObject, out fact);
            return fact;
        }

        public void SetInternalFact(INode node, Fact fact)
        {
            Dictionary<object, Fact> factMap;
            if (!_internalFactMap.TryGetValue(node, out factMap))
            {
                factMap = new Dictionary<object, Fact>();
                _internalFactMap[node] = factMap;
            }

            factMap[fact.RawObject] = fact;
        }

        public void RemoveInternalFact(INode node, Fact fact)
        {
            Dictionary<object, Fact> factMap;
            if (!_internalFactMap.TryGetValue(node, out factMap)) 
                return;

            factMap.Remove(fact.RawObject);
            if (factMap.Count == 0) 
                _internalFactMap.Remove(node);
        }

        public IAlphaMemory GetNodeMemory(IAlphaMemoryNode node)
        {
            IAlphaMemory memory;
            if (!_alphaMap.TryGetValue(node, out memory))
            {
                memory = new AlphaMemory();
                _alphaMap[node] = memory;
            }
            return memory;
        }

        public IBetaMemory GetNodeMemory(IBetaMemoryNode node)
        {
            IBetaMemory memory;
            if (!_betaMap.TryGetValue(node, out memory))
            {
                memory = new BetaMemory();
                _betaMap[node] = memory;
            }
            return memory;
        }
    }
}