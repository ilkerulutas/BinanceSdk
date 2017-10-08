using System.Collections.Generic;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class AccountResponse : ResponseBase
    {
        public int BuyerCommission { get; set; }
        public int SellerCommission { get; set; }
        public int MakerCommission { get; set; }
        public int TakerCommission { get; set; }
        public bool CanDeposit { get; set; }
        public bool CanTrade { get; set; }
        public bool CanWithdraw { get; set; }
        public List<BalanceItem> Balances { get; set; }
    }
}
