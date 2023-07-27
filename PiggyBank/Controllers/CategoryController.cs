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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }
        public ActionResult<ICollection<Category>> Index()
        {
            var userCategories = _categoryRepository.GetUserCategories(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(userCategories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryCreateDto createDto)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = createDto.Name,
                    IsIncome = createDto.IsIncome
                };
                if (await _categoryRepository.TryAddCategoryAsync(category, await _userManager.GetUserAsync(User)))
                {
                    return RedirectToAction("Index");
                }
                ViewData["Error"] = "У вас уже есть эта категория в списке категорий";
            }
            return View();
        }
    }
}
