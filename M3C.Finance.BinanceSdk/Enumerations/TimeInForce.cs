namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class TimeInForce
    {
        private TimeInForce(string value) { Value = value; }

        private string Value { get; }

        public static TimeInForce ImmediateOrCancel => new TimeInForce("IOC");
        public static TimeInForce GoodUntilCanceled => new TimeInForce("GTC");

        public static implicit operator string(TimeInForce timeInForce) => timeInForce.Value;
        public static implicit operator TimeInForce(string text) => new TimeInForce(text);
        public override string ToString() => Value;
    }
}
