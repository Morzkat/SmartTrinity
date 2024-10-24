using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartTrinity.Shared.Core.Models;
using SmartTrinity.Shared.Core.Services;
using Microsoft.AspNetCore.Authorization;
using SmartTrinity.App.Migrations.Core.Services;

namespace SmartTrinity.App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISalesService _salesService;
        private readonly ISalesMigrator _salesMigrator;

        public SalesController(ILogger<SalesController> logger, ISalesService salesService, ISalesMigrator salesMigrator)
        {
            _logger = logger;
            _salesService = salesService;
            _salesMigrator = salesMigrator;
        }

        /// <summary>
        /// Retrieves X quantity of sales 
        /// </summary>
        /// <response code="200">List of sales</response>
        /// <response code="500">Error getting sales</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Sale>), 200)]
        [ProducesResponseType(typeof(ErrorResponseDetails), 500)]
        public async Task<ActionResult> GetSales()
        {
            try
            {
                return Ok(await _salesService.GetSales());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Test")]
        public async Task<ActionResult> Test()
        {
            await _salesMigrator.MigrateSalesToCentral();
            return Ok("Complete test...");
        }
    }

    public class ErrorResponseDetails
    {
        public string Message { get; set; }
    }
}