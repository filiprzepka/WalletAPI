using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WalletAPI.Entities;
using WalletAPI.Models;

namespace WalletAPI
{
    public class WalletMappingProfile : Profile
    {
        public WalletMappingProfile()
        {
            CreateMap<CreateMonthDto, Month>();

            CreateMap<CreateIncomeDto, Income>();

            CreateMap<CreateExpenseDto, Expense>();

            CreateMap<Month, MonthDto>();

            CreateMap<Income, IncomeDto>();

            CreateMap<Expense, ExpenseDto>();

            CreateMap<RegisterUserDto, User>();
        }
    }
}
