namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class KlineInterval
    {
        private KlineInterval(string value) { Value = value; }

        private string Value { get; }

        public static KlineInterval Minute1 => new KlineInterval("1m");
        public static KlineInterval Minute3 => new KlineInterval("3m");
        public static KlineInterval Minute5 => new KlineInterval("5m");
        public static KlineInterval Minute15 => new KlineInterval("15m");
        public static KlineInterval Minute30 => new KlineInterval("30m");
        public static KlineInterval Hour1 => new KlineInterval("1h");
        public static KlineInterval Hour2 => new KlineInterval("2h");
        public static KlineInterval Hour4 => new KlineInterval("4h");
        public static KlineInterval Hour6 => new KlineInterval("6h");
        public static KlineInterval Hour8 => new KlineInterval("8h");
        public static KlineInterval Hour12 => new KlineInterval("12h");
        public static KlineInterval Day1 => new KlineInterval("1d");
        public static KlineInterval Day3 => new KlineInterval("3d");
        public static KlineInterval Week1 => new KlineInterval("1w");
        public static KlineInterval Month1 => new KlineInterval("1M");

        public static implicit operator string(KlineInterval version) => version.Value;
        public static implicit operator KlineInterval(string text) => new KlineInterval(text);
        public override string ToString() => Value;
    }
}
