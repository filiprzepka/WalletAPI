using AutoMapper;
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
using System.Security.Claims;

namespace WalletAPI.Controllers
{
    [Route("api/month")]
    [ApiController]
    [Authorize]
    public class MonthController : ControllerBase
    {
        private readonly IMonthService _monthService;

        public MonthController(IMonthService monthService)
        {
            _monthService = monthService;
        }

        [HttpGet("{id}/sum")]
        public ActionResult GetSumOfTransactions(int id)
        {
            var sum = _monthService.SumOfTransactions(id);

            return Ok(sum);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateMonth([FromBody] UpdateMonthDto dto, [FromRoute] int id)
        {
            _monthService.UpdateMonth(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _monthService.Delete(id);

            return NoContent();
        }

        [HttpPost]
        public ActionResult CreateMonth([FromBody] CreateMonthDto dto)
        {
            var id = _monthService.Create(dto);

            return Created($"/api/month/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Month>> GetAll()
        {
            var months = _monthService.GetAll();
            return Ok(months);
        }

        [HttpGet("{id}")]
        public ActionResult<Month> GetById([FromRoute] int id)
        {
            var month = _monthService.GetById(id);

            return Ok(month);
        }
    }
}
