namespace KSPCompiler.Domain.Ast
{
    /// <summary>
    /// Definition of a constant values.
    /// </summary>
    public static class KspValueConstants
    {
        // Min, Max definitions
        public static int KspIntMinValue => int.MinValue;
        public static int KspIntMaxValue => int.MaxValue;
        public static float KspRealMinValue => float.MinValue;
        public static float KspRealMaxValue => float.MaxValue;

        /// <summary>
        /// The prefix of a variable name that NI disallows to be used.
        /// </summary>
        public static readonly string[] NiReservedPrefix =
        {
            // From KSP Reference Manual:
            // Please do not create variables with the prefixes below, as these prefixes are used for
            // internal variables and constants
            "$NI_",
            "$CONTROL_PAR_",
            "$EVENT_PAR_",
            "$ENGINE_PAR_",
        };
    }
}
