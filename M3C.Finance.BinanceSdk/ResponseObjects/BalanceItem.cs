namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class BalanceItem
    {
        public string Asset { get; set; }
        public decimal Free { get; set; }
        public decimal Locked { get; set; }
    }
}
