using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletAPI.Entities;
using WalletAPI.Models;
using WalletAPI.Services;

namespace WalletAPI.Controllers
{
    [Route("api/month/{monthId}/expense")]
    [ApiController]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPut("{id}")]
        public ActionResult UpdateExpense([FromBody] UpdateExpenseDto dto, int expenseId, int monthId)
        {
            _expenseService.UpdateExpense(dto, expenseId, monthId);

            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int monthId)
        {
            _expenseService.RemoveAll(monthId);

            return NoContent();
        }

        [HttpDelete("{expenseId}")]
        public ActionResult DeleteById([FromRoute] int monthId, [FromRoute] int expenseId)
        {
            _expenseService.RemoveById(monthId, expenseId);

            return NoContent();
        }

        [HttpPost]
        public ActionResult CreateExpense([FromRoute] int monthId, [FromBody] CreateExpenseDto dto)
        {
            var newExpenseId = _expenseService.Create(monthId, dto);

            return Created($"api/month/{monthId}/expense/{newExpenseId}", null);
        }

        [HttpGet("{expenseId}")]
        public ActionResult<Expense> GetExpenseById([FromRoute] int monthId, [FromRoute] int expenseId)
        {
            Expense expense = _expenseService.GetById(monthId, expenseId);
            return Ok(expense);
        }

        [HttpGet]
        public ActionResult<List<Expense>> Get([FromRoute] int monthId)
        {
            var expense = _expenseService.GetAll(monthId);
            return Ok(expense);
        }

    }
}
