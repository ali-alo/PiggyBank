using Microsoft.AspNetCore.Mvc.ModelBinding;
using PiggyBank.CustomValidation;
using PiggyBank.Models;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.DTOs
{
    public class TransactionUpdateDto : TransactionCreationDto
    {
        public int Id { get; set; }
    }
}
