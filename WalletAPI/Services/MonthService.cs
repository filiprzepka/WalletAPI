using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAPI.Entities;
using WalletAPI.Exceptions;
using WalletAPI.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WalletAPI.Authorization;

namespace WalletAPI.Services
{
    public interface IMonthService
    {
        Month GetById(int id);
        IEnumerable<Month> GetAll();
        int Create(CreateMonthDto dto);
        void Delete(int id);
        void UpdateMonth(int id, UpdateMonthDto dto);
        decimal SumOfTransactions(int id);


    }
    public class MonthService : IMonthService
    {
        private readonly WalletDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<MonthService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public MonthService(WalletDBContext dbContext, IMapper mapper, ILogger<MonthService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public decimal SumOfTransactions(int id)
        {
            var userId = (int)_userContextService.GetUserId;
            var month = _dbContext
                .Months
                .Where(m => m.UserId == userId)
                .Include(m => m.Expenses)
                .Include(m => m.Incomes)
                .FirstOrDefault(x => x.Id == id);

            if (month is null)
                throw new NotFoundException("Month not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            decimal result = 0;
            foreach (var expense in _dbContext.Expenses)
            {
                result -= expense.Amount;
            }

            foreach (var income in _dbContext.Incomes)
            {
                result += income.Amount;
            }

            return result;
        }

        public void UpdateMonth(int id, UpdateMonthDto dto)
        {
            var month = _dbContext
                .Months
                .FirstOrDefault(x => x.Id == id);

            if (month is null)
                throw new NotFoundException("Month not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            month.Name = dto.Name;
            month.BeginningOfTheMonth = dto.BeginningOfTheMonth;
            month.EndOfTheMonth = dto.EndOfTheMonth;

            _dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            _logger.LogError($"Month with id: {id} DELETE action invoked");

            var month = _dbContext
                .Months
                .FirstOrDefault(x => x.Id == id);

            if (month is null)
                throw new NotFoundException("Month not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            _dbContext.Months.Remove(month);
            _dbContext.SaveChanges();
        }
        public Month GetById(int id)
        {
            var userId = (int)_userContextService.GetUserId;
            var month = _dbContext
                .Months
                .Where(m => m.UserId == userId)
                .Include(m => m.Expenses)
                .Include(m => m.Incomes)
                .FirstOrDefault(x => x.Id == id);

            if (month == null)
                throw new NotFoundException("Month not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, month,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbidden action");
            }

            return month;
        }
        public IEnumerable<Month> GetAll()
        {
            var userId = (int)_userContextService.GetUserId;
            var months = _dbContext
                .Months
                .Where(m => m.UserId == userId)
                .Include(m => m.Expenses)
                .Include(m => m.Incomes)
                .ToList();
                

            if (months is null || months.Count() == 0)
                throw new NotFoundException("Month not found");

            

            return months;
        }
        public int Create(CreateMonthDto dto)
        {
            var month = _mapper.Map<Month>(dto);
            month.UserId = (int)_userContextService.GetUserId;
            _dbContext.Months.Add(month);
            _dbContext.SaveChanges();

            return month.Id;
        }
    }
}

