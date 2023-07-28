using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Data;
using PiggyBank.DTOs;
using PiggyBank.Models;
using PiggyBank.Repositories;
using System.Security.Claims;

namespace PiggyBank.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public TransactionController(ITransactionRepository transactionRepository, SignInManager<ApplicationUser> signInManager,
                                     ICategoryRepository categoryRepository, IApplicationUserRepository applicationUserRepository)
        {
            _transactionRepository = transactionRepository;
            _signInManager = signInManager;
            _categoryRepository = categoryRepository;
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<IActionResult> Index()
        {
            IList<Transaction> userTransactions = await _transactionRepository.GetAllUserTransactionsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(userTransactions);
        }


        public ActionResult Create([FromQuery] bool isIncome)
        {
            if (isIncome)
            {
                var userCategories = _categoryRepository.GetUserIncomeCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var transactionCreationDto = new TransactionCreationDto { IsIncome = true, UserCategories = userCategories};
                return View(transactionCreationDto);
            }
            else
            {
                var userCategories = _categoryRepository.GetUserExpenseCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var transactionCreationDto = new TransactionCreationDto { IsIncome = false, UserCategories = userCategories};
                return View(transactionCreationDto);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(TransactionCreationDto transactionCreationDto)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetCategoryAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), transactionCreationDto.CategoryId);
                if (category == null)
                {
                    ViewData["Error"] = "Неверная категория";
                    return View(transactionCreationDto);
                }

                if(!transactionCreationDto.IsIncome) 
                {
                    transactionCreationDto.Amount = transactionCreationDto.Amount * -1;
                }

                var transaction = new Transaction
                {
                    Amount = transactionCreationDto.Amount,
                    Time = transactionCreationDto.Time.ToUniversalTime(),
                    Category = category,
                    IsIncome = transactionCreationDto.IsIncome,
                    Comment = transactionCreationDto.Comment,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };

                await _transactionRepository.CreateAsync(transaction);
                await _applicationUserRepository.UpdateBalance(await _signInManager.UserManager.GetUserAsync(User), transaction.Amount);
                return RedirectToAction("Index", "Transaction");
            }

            var userCategories = _categoryRepository.GetUserIncomeCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
            transactionCreationDto.UserCategories = userCategories;
            return View(transactionCreationDto);
        }

    }
}
