namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class OrderType
    {
        private OrderType(string value) { Value = value; }

        private string Value { get; }

        public static OrderType Limit => new OrderType("LIMIT");
        public static OrderType Market => new OrderType("MARKET");

        public static implicit operator string(OrderType orderType) => orderType.Value;
        public static implicit operator OrderType(string text) => new OrderType(text);
        public override string ToString() => Value;
    }
}
