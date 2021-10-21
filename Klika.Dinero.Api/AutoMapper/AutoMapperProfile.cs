using AutoMapper;
using Klika.Dinero.Model.Entities;
using Klika.Dinero.Model.DTO.Bank.Response;
using Klika.Dinero.Model.DTO.TransactionCategory.Response;
using Klika.Dinero.Model.DTO.Account.Response;
using Klika.Dinero.Model.DTO.Account.Request;

namespace Klika.Dinero.Api.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountResponseDTO>().ReverseMap();

            CreateMap<AccountRequestDTO, Account>();

            CreateMap<Bank, BankResponseDTO>();

            CreateMap<TransactionCategory, TransactionCategoryResponseDTO>();
        }
    }
}
