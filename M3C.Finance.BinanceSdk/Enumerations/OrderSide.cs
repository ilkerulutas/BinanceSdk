using System.ComponentModel.DataAnnotations;

namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class OrderSide
    {
        private OrderSide(string value) { Value = value; }

        public string Value { get; }

        public static OrderSide Buy => new OrderSide("BUY");
        public static OrderSide Sell => new OrderSide("SELL");

        public static implicit operator string(OrderSide side) => side.Value;
        public static implicit operator OrderSide(string text) => new OrderSide(text);
        public override string ToString() => Value;
    }


}
