using AutoMapper;
using PiggyBank.DTOs;
using PiggyBank.Models;

namespace PiggyBank.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<TransactionUpdateDto, Transaction>();
            CreateMap<Transaction, TransactionUpdateDto>();
            CreateMap<TransactionCreationDto, Transaction>();
        }
    }
}
