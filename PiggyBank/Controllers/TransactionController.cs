using Microsoft.AspNetCore.Mvc;
using PiggyBank.Repositories;
using System.Security.Claims;

namespace PiggyBank.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userTransactions = await _transactionRepository.GetAllUserTransactionsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(userTransactions);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
    }
}
