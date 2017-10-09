namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class ExecutionType
    {
        private ExecutionType(string value) { Value = value; }

        public string Value { get; }

        public static ExecutionType New => new ExecutionType("NEW");
        public static ExecutionType Canceled => new ExecutionType("CANCELED");
        public static ExecutionType Replaced => new ExecutionType("REPLACED");
        public static ExecutionType Rejected => new ExecutionType("REJECTED");
        public static ExecutionType Trade => new ExecutionType("TRADE");
        public static ExecutionType Expired => new ExecutionType("EXPIRED");

        public static implicit operator string(ExecutionType value) => value.Value;
        public static implicit operator ExecutionType(string text) => new ExecutionType(text);
        public override string ToString() => Value;
    }
}
