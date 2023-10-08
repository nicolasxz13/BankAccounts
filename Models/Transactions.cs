using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }
        [RegularExpression(@"^\d+.\d{0,2}$",ErrorMessage = "Price can't have more than 2 decimal places")]
        [Required(ErrorMessage ="Amount is required")]
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? T_User { get; set; }
    }
}
