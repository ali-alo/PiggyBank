using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Models;

namespace PiggyBank.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        [PersonalData]
        public DbSet<Transaction> Transactions { get; set; }
        [PersonalData]
        public DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Заработная плата",
                    IsIncome = true,
                },
                new Category
                {
                    Id = 2,
                    Name = "Доход с сдачи в аренду недвижимости",
                    IsIncome = true,
                },
                new Category
                {
                    Id = 3,
                    Name = "Иные доходы",
                    IsIncome = true,
                },
                new Category
                {
                    Id = 4,
                    Name = "Продукты питания",
                    IsIncome = false,
                },
                new Category
                {
                    Id = 5,
                    Name = "Транспорт",
                    IsIncome = false,
                },
                new Category
                {
                    Id = 6,
                    Name = "Мобильная связь",
                    IsIncome = false,
                },
                new Category
                {
                    Id = 7,
                    Name = "Интернет",
                    IsIncome = false,
                },
                new Category
                {
                    Id = 8,
                    Name = "Развлечения",
                    IsIncome = false,
                },
                new Category
                {
                    Id = 9,
                    Name = "Заработная плата",
                    IsIncome = false,
                },
                new Category
                {
                    Id = 10,
                    Name = "Другое",
                    IsIncome = false,
                }
            );
        }
    }
}
