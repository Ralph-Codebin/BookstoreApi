using System;

namespace SanQuoteWeb.Common.Types.MultipleResults
{
    /// <summary>
    /// Guard for ResultMarshal
    /// </summary>
    internal static class ResultMarshalGuard
    {
        /// <summary>
        /// Ensures that two types do not inherit from one another
        /// </summary>
        /// <typeparam name="T1">First type to check</typeparam>
        /// <typeparam name="T2">Second type to check</typeparam>
        public static void GuardAgainstRelationship<T1, T2>()
        {
            var t1Type = typeof(T1);
            var t2Type = typeof(T2);
            if (t1Type.IsSubclassOf(t2Type))
            {
                throw new ArgumentException($"Improper use of ResultMarshal, Type [{t1Type.Name}] must not be a subclass of Type [{t2Type.Name}]");
            }

            if (t2Type.IsSubclassOf(t1Type))
            {
                throw new ArgumentException($"Improper use of ResultMarshal, Type [{t2Type.Name}] must not be a subclass of Type [{t1Type.Name}]");
            }
        }
    }
}
