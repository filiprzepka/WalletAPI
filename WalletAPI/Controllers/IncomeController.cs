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
    [Route("api/month/{monthId}/income")]
    [ApiController]
    [Authorize]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpPut("{id}")]
        public ActionResult UpdateIncome([FromBody] UpdateIncomeDto dto, int incomeId, int monthId)
        {
            _incomeService.UpdateIncome(dto, incomeId, monthId);

            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int monthId)
        {
            _incomeService.RemoveAll(monthId);

            return NoContent();
        }

        [HttpDelete("{incomeId}")]
        public ActionResult DeleteById([FromRoute] int monthId, [FromRoute] int incomeId)
        {
            _incomeService.RemoveById(monthId, incomeId);

            return NoContent();
        }

        [HttpPost]
        public ActionResult CreateIncome([FromRoute]int monthId, [FromBody]CreateIncomeDto dto)
        {
            var newIncomeId = _incomeService.Create(monthId, dto);

            return Created($"api/month/{monthId}/income/{newIncomeId}", null);
        }

        [HttpGet("{incomeId}")]
        public ActionResult<Income> GetIncomeById([FromRoute] int monthId, [FromRoute] int incomeId)
        {
            Income income = _incomeService.GetById(monthId, incomeId);
            return Ok(income);
        }

        [HttpGet]
        public ActionResult<List<Income>> Get([FromRoute] int monthId)
        {
            var income = _incomeService.GetAll(monthId);
            return Ok(income);
        }

    }
}
