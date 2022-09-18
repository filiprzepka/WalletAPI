using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAPI.Authorization;
using WalletAPI.Entities;
using WalletAPI.Exceptions;
using WalletAPI.Models;

namespace WalletAPI.Services
{
    public interface IIncomeService
    {
        int Create(int monthId, CreateIncomeDto dto);
        Income GetById(int monthId, int incomeId);
        List<IncomeDto> GetAll(int monthId);
        void RemoveAll(int monthId);
        void RemoveById(int monthId, int incomeId);
        void UpdateIncome(UpdateIncomeDto dto, int id, int monthId);
    }
    public class IncomeService : IIncomeService
    {
        private readonly WalletDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public IncomeService(WalletDBContext context, IMapper mapper, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public void UpdateIncome(UpdateIncomeDto dto, int incomeId, int monthId)
        {

            var month = GetMonthById(monthId);
            var income = _context.Incomes
                .FirstOrDefault(x => x.Id == incomeId);

            if (income is null || income.MonthID != month.Id)
            {
                throw new NotFoundException("Transaction not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            income.MonthID = dto.MonthID;
            income.UserId = dto.UserId;
            income.Name = dto.Name;
            income.Description = dto.Description;
            income.DayOfTransaction = dto.DayOfTransaction;

            _context.SaveChanges();

        }
        public int Create(int monthId, CreateIncomeDto dto)
        {
            var month = GetMonthById(monthId);
            var userId = (int)_userContextService.GetUserId;

            var incomeEntity = _mapper.Map<Income>(dto);

            incomeEntity.MonthID = month.Id;
            incomeEntity.UserId = userId;

            _context.Incomes.Add(incomeEntity);
            _context.SaveChanges();

            return incomeEntity.Id;
        }
        public Income GetById(int monthId, int incomeId)
        {
            var month = GetMonthById(monthId);

            var income = _context.Incomes.FirstOrDefault(m => m.Id == incomeId);

            if (income is null || income.MonthID != monthId)
            {
                throw new NotFoundException("Income not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            return income;
        }
        public List<IncomeDto> GetAll(int monthId)
        {
            var month = GetMonthById(monthId);
            var incomes = _mapper.Map<List<IncomeDto>>(month.Incomes);

            return incomes;
        }

        public void RemoveAll(int monthId)
        {
            var month = GetMonthById(monthId);

            _context.RemoveRange(month.Incomes);
            _context.SaveChanges();
        }

        public void RemoveById(int monthId, int incomeId)
        {
            var month = GetMonthById(monthId);

            var income = _context.Incomes.FirstOrDefault(m => m.Id == incomeId);

            if (income is null || income.MonthID != monthId)
            {
                throw new NotFoundException("Income not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            _context.Remove(income);
            _context.SaveChanges();
        }

        private Month GetMonthById(int monthId)
        {
            var month = _context
                .Months
                .Include(m => m.Incomes)
                .FirstOrDefault(m => m.Id == monthId);

            if (month is null)
                throw new NotFoundException("Month not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            return month;
        }
    }
}
