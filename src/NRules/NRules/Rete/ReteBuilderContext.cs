﻿using NRules.RuleModel;
using System.Collections.Generic;

namespace NRules.Rete
{
    internal class ReteBuilderContext
    {
        private readonly List<Declaration> _declarations;

        public ReteBuilderContext(DummyNode dummyNode)
        {
            _declarations = new List<Declaration>();
            BetaSource = dummyNode;
        }

        public ReteBuilderContext(ReteBuilderContext context)
        {
            BetaSource = context.BetaSource;
            _declarations = new List<Declaration>(context._declarations);
        }

        public IEnumerable<Declaration> Declarations
        {
            get { return _declarations; }
        }

        public AlphaNode CurrentAlphaNode { get; set; }
        public IAlphaMemoryNode AlphaSource { get; set; }
        public IBetaMemoryNode BetaSource { get; set; }
        public bool HasSubnet { get; set; }

        public void RegisterDeclaration(Declaration declaration)
        {
            _declarations.Add(declaration);
        }

        public void ResetAlphaSource()
        {
            AlphaSource = null;
            HasSubnet = false;
        }
    }
}