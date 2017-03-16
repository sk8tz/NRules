﻿using NRules.RuleModel;
using NRules.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NRules.Rete
{
    [DebuggerDisplay("[{string.Join(\" \",_map)}]")]
    internal class IndexMap
    {
        private readonly int[] _map;

        public IndexMap(int[] map)
        {
            _map = map;
        }

        public int this[int index]
        {
            get { return (index >= 0) ? _map[index] : -1; }
        }

        public static void SetElementAt(ref object[] target, int index, int offset, object value)
        {
            if (index >= 0)
            {
                target[index + offset] = value;
            }
        }

        public static IndexMap CreateMap(IEnumerable<Declaration> declarations, IEnumerable<Declaration> baseDeclarations)
        {
            var positionMap = declarations.ToIndexMap();
            var map = baseDeclarations
                .Select(positionMap.IndexOrDefault).ToArray();
            return new IndexMap(map);
        }
    }
}