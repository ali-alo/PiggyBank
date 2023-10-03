using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int? categoryId)
        {
            IList<Transaction> userTransactions;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (categoryId != null)
                userTransactions = await _transactionRepository.GetAllUserTransactionsByCategoryAsync(userId, categoryId.Value);
            else
                userTransactions = await _transactionRepository.GetAllUserTransactionsAsync(userId);
            return View(userTransactions);
        }

        [HttpGet]
        public ActionResult Create([FromQuery] bool isIncome)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userCategories = GetRelevantCategories(userId, isIncome);

            var transactionCreationDto = new TransactionCreationDto
            {
                IsIncome = isIncome,
                UserCategories = userCategories
            };

            return View(transactionCreationDto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TransactionCreationDto transactionCreationDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
                return ResubmitCreateForm(userId, transactionCreationDto);

            var category = await _categoryRepository.GetCategoryAsync(userId, transactionCreationDto.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("", "Данной категории не существует. Пожалуйста попробуйте снова.");
                return ResubmitCreateForm(userId, transactionCreationDto);
            }

            Transaction transaction = CreationDtoToModel(userId, category, transactionCreationDto);
            await _transactionRepository.CreateAsync(transaction);
            return RedirectToAction("Index", "Transaction");
        }

        private ActionResult ResubmitCreateForm(string userId, TransactionCreationDto dto)
        {
            var userCategories = GetRelevantCategories(userId, dto.IsIncome);
            dto.UserCategories = userCategories;
            return View(dto);
        }

        private Transaction CreationDtoToModel(string userId, Category category, TransactionCreationDto transactionCreationDto)
        {
            transactionCreationDto.Amount = ConvertToNegativeIfExpense(transactionCreationDto.Amount, transactionCreationDto.IsIncome);
            transactionCreationDto.Time = transactionCreationDto.Time.ToUniversalTime();

            var transaction = _mapper.Map<Transaction>(transactionCreationDto);
            transaction.Category = category;
            transaction.UserId = userId;
            return transaction;
        }

        private decimal ConvertToNegativeIfExpense(decimal amount, bool isIncome)
        {
            if (!isIncome)
                amount = amount * -1;
            return amount;
        }

        [HttpGet]
        public async Task<ActionResult<Transaction>> Edit(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var transaction = await _transactionRepository.FindTransactionAsync(userId, id);

            if (transaction == null)
                return NotFound();

            var dto = _mapper.Map<TransactionUpdateDto>(transaction);
            dto.UserCategories = GetRelevantCategories(userId, transaction.IsIncome);
            dto.Amount = Math.Abs(dto.Amount);
            return View(dto);
        }

        private ICollection<Category> GetRelevantCategories(string userId, bool isIncome) =>
            isIncome
                ? _categoryRepository.GetUserIncomeCategories(userId)
                : _categoryRepository.GetUserExpenseCategories(userId);

        [HttpPut]
        public async Task<ActionResult> Edit(TransactionUpdateDto dto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                var userCategories = GetRelevantCategories(userId, dto.IsIncome);
                dto.UserCategories = userCategories;
                return BadRequest(ModelState);
            }


            var result = await _transactionRepository.UpdateTransactionAsync(userId, dto);
            if (!result)
                return NotFound();
            return Ok();


        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool success = await _transactionRepository.DeleteTransactionAsync(userId, id);
            return success ? Ok() : BadRequest();
        }
    }
}
