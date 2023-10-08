namespace BankAccounts.Models
{
    public class TransactionsView
    {
        public decimal Totalamount { get; set; }
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();

        public Transactions? Temptransactions { get; set; }
    }
}
