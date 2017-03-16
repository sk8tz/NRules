using System;
using System.Runtime.Serialization;
using System.Security;

namespace NRules
{
    /// <summary>
    /// Represents errors that occur while evaluating expressions as part of rules execution.
    /// </summary>
    [Serializable]
    public class RuleExpressionEvaluationException : RuleExecutionException
    {
        internal RuleExpressionEvaluationException(string message, string expression, Exception innerException)
            : base(message, innerException)
        {
            Expression = expression;
        }

        [SecuritySafeCritical]
        protected RuleExpressionEvaluationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Expression = info.GetString("Expression");
        }

        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            base.GetObjectData(info, context);
            info.AddValue("Expression", Expression, typeof(String));
        }

        /// <summary>
        /// Expression that caused exception.
        /// </summary>
        public string Expression { get; private set; }

        public override string Message
        {
            get
            {
                string message = base.Message;
                if (!string.IsNullOrEmpty(Expression))
                {
                    return message + Environment.NewLine + Expression;
                }
                return message;
            }
        }
    }
}