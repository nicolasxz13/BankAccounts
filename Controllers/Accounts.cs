using BankAccounts.Data;
using BankAccounts.Filters;
using BankAccounts.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankAccounts.Controllers
{
    public class Accounts : Controller
    {
        private LoginContext _context;

        public Accounts(LoginContext loginContext)
        {
            _context = loginContext;
        }

        [SessionCheck]
        [AccessFilter]
        [HttpGet("accounts/{id}")]
        public IActionResult Index(int id)
        {
            TransactionsView? transactionsView = Calc(id);
            return View(transactionsView);
        }

        [SessionCheck]
        [AccessFilter]
        [HttpPost("accounts/{id}/deposit")]
        public IActionResult Deposit(int id, Transactions transaction)
        {
            if (ModelState.IsValid)
            {
                TransactionsView? resultemp = Calc(id);
                if (resultemp != null)
                {
                    if (transaction.Amount <= resultemp.Totalamount)
                    {
                        transaction.T_User = _context.Users.FirstOrDefault(a=> a.UserId == id);
                        _context.transactions.Add(transaction);
                        _context.SaveChanges();
                        return RedirectToAction("Index",new {id = id});
                    }
                    else
                    {
                        ModelState.AddModelError(
                            "Amount",
                            "The amount is greater than your balance"
                        );
                    }
                }
            }
            TransactionsView? transactionsView = Calc(id);
            return View("Index", transactionsView);
        }

        private TransactionsView? Calc(int id)
        {
            return _context.Users
                .Where(u => u.UserId == id)
                .Select(
                    u =>
                        new TransactionsView
                        {
                            Totalamount = u.Transactions.Sum(t => t.Amount),
                            Transactions = u.Transactions.ToList()
                        }
                )
                .FirstOrDefault();
        }
    }
}
