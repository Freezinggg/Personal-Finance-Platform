namespace PersonalFinancePlatform.API.Contracts.Wallet
{
    public class UpdateWalletRequest
    {
        public Guid WalletId { get; set; }
        public string WalletName { get; set; }
    }
}
