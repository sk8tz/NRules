using NRules.Rete;
using System;

namespace NRules.Diagnostics
{
    /// <summary>
    /// Information related to working memory events.
    /// </summary>
    public class WorkingMemoryEventArgs : EventArgs
    {
        private readonly Fact _fact;

        internal WorkingMemoryEventArgs(Fact fact)
        {
            _fact = fact;
        }

        /// <summary>
        /// Fact related to the event.
        /// </summary>
        public FactInfo Fact { get { return new FactInfo(_fact); } }
    }
}