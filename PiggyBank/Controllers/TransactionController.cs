using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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

        public async Task<IActionResult> Index([FromQuery] int? categoryId)
        {
            IList<Transaction> userTransactions;
            if (categoryId != null)
                userTransactions = await _transactionRepository.GetAllUserTransactionsByCategoryAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), categoryId.Value);
            else
                userTransactions = await _transactionRepository.GetAllUserTransactionsAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View(userTransactions);
        }


        public ActionResult Create([FromQuery] bool isIncome)
        {
            if (isIncome)
            {
                var userCategories = _categoryRepository.GetUserIncomeCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var transactionCreationDto = new TransactionCreationDto { IsIncome = true, UserCategories = userCategories };
                return View(transactionCreationDto);
            }
            else
            {
                var userCategories = _categoryRepository.GetUserExpenseCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var transactionCreationDto = new TransactionCreationDto { IsIncome = false, UserCategories = userCategories };
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

                if (!transactionCreationDto.IsIncome)
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

        public async Task<ActionResult<Transaction>> Edit(int id)
        {
            var transaction = await _transactionRepository.FindTransactionAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            var userCategories = _categoryRepository.GetUserIncomeCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (transaction == null)
            {
                return NotFound();
            }

            var transactionUpdateDto = new TransactionUpdateDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Time = transaction.Time,
                Comment = transaction.Comment,
                IsIncome = transaction.IsIncome,
                CategoryId = transaction.CategoryId,
                UserCategories = userCategories
            };

            return View(transactionUpdateDto);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TransactionUpdateDto transactionUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                var userCategories = _categoryRepository.GetUserIncomeCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
                transactionUpdateDto.UserCategories = userCategories;
                return View(transactionUpdateDto);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tr = await _transactionRepository.FindTransactionAsync(userId, transactionUpdateDto.Id);
            if (tr == null)
            {
                return NotFound();
            }

            // the logic to adjust the balance after changing the income/expense amount
            decimal amountDifference = tr.IsIncome ? (transactionUpdateDto.Amount - tr.Amount) : (transactionUpdateDto.Amount - Math.Abs(tr.Amount)) * -1;
            await _applicationUserRepository.UpdateBalance(await _signInManager.UserManager.GetUserAsync(User), amountDifference);

            tr.Id = transactionUpdateDto.Id;
            tr.Amount = transactionUpdateDto.Amount;
            tr.Time = transactionUpdateDto.Time;
            tr.Comment = transactionUpdateDto.Comment;
            tr.IsIncome = transactionUpdateDto.IsIncome;
            tr.CategoryId = transactionUpdateDto.CategoryId;
            tr.UserId = userId;


            await _transactionRepository.UpdateTransactionAsync(userId, tr);
            return RedirectToAction("Index", "Transaction");


        }

        public async Task<ActionResult> Delete(int id)
        {
            var transaction = await _transactionRepository.FindTransactionAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            bool success = await _transactionRepository.DeleteTransactionAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), id);
            if (success)
            {
                await _applicationUserRepository.UpdateBalance(await _signInManager.UserManager.GetUserAsync(User), transaction!.Amount * -1);
                return RedirectToAction("Index");
            }
            return BadRequest();
        }

    }
}
