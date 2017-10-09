namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class OrderRejectReason
    {
        private OrderRejectReason(string value) { Value = value; }

        public string Value { get; }

        public static OrderRejectReason None => new OrderRejectReason("NONE");
        public static OrderRejectReason UnknownInstrument => new OrderRejectReason("UNKNOWN_INSTRUMENT");
        public static OrderRejectReason MarketClosed => new OrderRejectReason("MARKET_CLOSED");
        public static OrderRejectReason PriceQuantityExceedHardLimits => new OrderRejectReason("PRICE_QTY_EXCEED_HARD_LIMITS");
        public static OrderRejectReason UnknownOrder => new OrderRejectReason("UNKNOWN_ORDER");
        public static OrderRejectReason DuplicateOrder => new OrderRejectReason("DUPLICATE_ORDER");
        public static OrderRejectReason UnknownAccount => new OrderRejectReason("UNKNOWN_ACCOUNT");
        public static OrderRejectReason InsufficientBalance => new OrderRejectReason("INSUFFICIENT_BALANCE");
        public static OrderRejectReason AccountInactive => new OrderRejectReason("ACCOUNT_INACTIVE");
        public static OrderRejectReason AccountCannotSettle => new OrderRejectReason("ACCOUNT_CANNOT_SETTLE");

        public static implicit operator string(OrderRejectReason value) => value.Value;
        public static implicit operator OrderRejectReason(string text) => new OrderRejectReason(text);
        public override string ToString() => Value;
    }
}
