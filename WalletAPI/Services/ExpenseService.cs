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
    public interface IExpenseService
    {
        int Create(int monthId, CreateExpenseDto dto);
        Expense GetById(int monthId, int expenseId);
        List<ExpenseDto> GetAll(int monthId);
        void RemoveAll(int monthId);
        void RemoveById(int monthId, int expenseId);
        void UpdateExpense(UpdateExpenseDto dto, int id, int monthId);
    }
    public class ExpenseService : IExpenseService
    {
        private readonly WalletDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public ExpenseService(WalletDBContext context, IMapper mapper, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public void UpdateExpense(UpdateExpenseDto dto, int expenseId, int monthId)
        {

            var month = GetMonthById(monthId);
            var expense = _context.Expenses
                .FirstOrDefault(x => x.Id == expenseId);

            if (expense is null || expense.MonthID != month.Id)
            {
                throw new NotFoundException("Transaction not found");
            }

            expense.MonthID = dto.MonthID;
            expense.UserId = dto.UserId;
            expense.Name = dto.Name;
            expense.Description = dto.Description;
            expense.DayOfTransaction = dto.DayOfTransaction;

            _context.SaveChanges();

        }

        public int Create(int monthId, CreateExpenseDto dto)
        {
            var month = GetMonthById(monthId);
            var userId = (int)_userContextService.GetUserId;

            var expenseEntity = _mapper.Map<Expense>(dto);
            expenseEntity.MonthID = month.Id;
            expenseEntity.UserId = userId;

            _context.Expenses.Add(expenseEntity);
            _context.SaveChanges();

            return expenseEntity.Id;
        }
        public Expense GetById(int monthId, int expenseId)
        {
            var month = GetMonthById(monthId);

            var expense = _context.Expenses.FirstOrDefault(m => m.Id == expenseId);

            if (expense is null || expense.MonthID != monthId)
            {
                throw new NotFoundException("Expense not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            return expense;
        }
        public List<ExpenseDto> GetAll(int monthId)
        {
            var month = GetMonthById(monthId);
            var expense = _mapper.Map<List<ExpenseDto>>(month.Expenses);

            return expense;
        }

        public void RemoveAll(int monthId)
        {
            var month = GetMonthById(monthId);

            _context.RemoveRange(month.Expenses);
            _context.SaveChanges();
        }

        public void RemoveById(int monthId, int expenseId)
        {
            var month = GetMonthById(monthId);

            var expense = _context.Expenses.FirstOrDefault(m => m.Id == expenseId);

            if (expense is null || expense.MonthID != monthId)
            {
                throw new NotFoundException("Expense not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            _context.Remove(expense);
            _context.SaveChanges();
        }

        private Month GetMonthById(int monthId)
        {
            var month = _context
                .Months
                .Include(m => m.Expenses)
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
